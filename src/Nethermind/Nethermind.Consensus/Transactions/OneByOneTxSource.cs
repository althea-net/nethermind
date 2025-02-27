// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only 

using System.Collections.Generic;
using Nethermind.Core;

namespace Nethermind.Consensus.Transactions
{
    public class OneByOneTxSource : ITxSource
    {
        private readonly ITxSource _txSource;

        public OneByOneTxSource(ITxSource txSource)
        {
            _txSource = txSource;
        }

        public IEnumerable<Transaction> GetTransactions(BlockHeader parent, long gasLimit)
        {
            foreach (Transaction transaction in _txSource.GetTransactions(parent, gasLimit))
            {
                yield return transaction;
                break;
            }
        }
    }
}
