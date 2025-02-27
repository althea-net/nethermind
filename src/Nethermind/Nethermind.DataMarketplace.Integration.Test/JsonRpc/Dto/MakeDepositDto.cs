// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

namespace Nethermind.DataMarketplace.Integration.Test.JsonRpc.Dto
{
    public class MakeDepositDto
    {
        public string DataAssetId { get; set; }
        public uint Units { get; set; }
        public string Value { get; set; }
    }
}
