// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

namespace Nethermind.DataMarketplace.Core.Domain
{
    public enum SessionState
    {
        Unknown,
        Started,
        FinishedByConsumer,
        ConsumerDisconnected,
        FinishedByProvider,
        ProviderDisconnected
    }
}
