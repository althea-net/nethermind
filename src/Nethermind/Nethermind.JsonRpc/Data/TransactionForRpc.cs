﻿/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

using System.Numerics;
using Nethermind.Core;
using Nethermind.Core.Crypto;
using Nethermind.Core.Extensions;
using Nethermind.Core.Json;
using Nethermind.Dirichlet.Numerics;

namespace Nethermind.JsonRpc.Data
{
    public class TransactionForRpc : IJsonRpcRequest
    {
        private readonly IJsonSerializer _jsonSerializer = new UnforgivingJsonSerializer();
        
        public TransactionForRpc(Keccak blockHash, BigInteger? blockNumber, int? txIndex, Transaction transaction)
        {
            Hash = transaction.Hash;
            Nonce = transaction.Nonce;
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            TransactionIndex = txIndex;
            From = transaction.SenderAddress;
            To = transaction.To;
            Value = transaction.Value;
            GasPrice = transaction.GasPrice;
            Gas = transaction.GasLimit;
            Data = transaction.Data;
        }
        
        // ReSharper disable once UnusedMember.Global
        public TransactionForRpc()
        {
        }

        public Keccak Hash { get; set; }
        public BigInteger? Nonce { get; set; }
        public Keccak BlockHash { get; set; }
        public BigInteger? BlockNumber { get; set; }
        public BigInteger? TransactionIndex { get; set; }
        public Address From { get; set; }
        public Address To { get; set; }
        public BigInteger? Value { get; set; }
        public BigInteger? GasPrice { get; set; }
        public BigInteger? Gas { get; set; }
        public byte[] Data { get; set; }

        public void FromJson(string jsonValue)
        {
            var jsonObj = new { from = string.Empty, to = string.Empty, gas = string.Empty, gasPrice = string.Empty, value = string.Empty, data = string.Empty, nonce = string.Empty };
            var transaction = _jsonSerializer.DeserializeAnonymousType(jsonValue, jsonObj);
            From = !string.IsNullOrEmpty(transaction.from) ? new Address(transaction.from) : null;
            To = !string.IsNullOrEmpty(transaction.to) ? new Address(transaction.to) : null;
            Gas = !string.IsNullOrEmpty(transaction.gas) ? BigIntegerExtensions.ParseEthHex(transaction.gas) : BigInteger.Zero;
            GasPrice = !string.IsNullOrEmpty(transaction.gasPrice) ? BigIntegerExtensions.ParseEthHex(transaction.gasPrice) : BigInteger.Zero;
            Value = !string.IsNullOrEmpty(transaction.value) ? BigIntegerExtensions.ParseEthHex(transaction.value) : BigInteger.Zero;
            Data = !string.IsNullOrEmpty(transaction.data) ? Bytes.FromHexString(transaction.data) : null;
            Nonce = !string.IsNullOrEmpty(transaction.nonce) ? BigIntegerExtensions.ParseEthHex(transaction.nonce) : BigInteger.Zero;
        }

        public Transaction ToTransaction()
        {
            Transaction tx = new Transaction();
            tx.GasLimit = (UInt256)(Gas ?? 90000);
            tx.GasPrice = (UInt256)(GasPrice ?? 0);
            tx.Nonce = (ulong)(Nonce ?? 0); // here pick the last nonce?
            tx.To = To;
            tx.SenderAddress = From;
            tx.Value = (UInt256)(Value ?? 0);
            if (tx.To == null)
            {
                tx.Init = Data;
            }
            else
            {
                tx.Data = Data;
            }

            return tx;
        }
    }
}