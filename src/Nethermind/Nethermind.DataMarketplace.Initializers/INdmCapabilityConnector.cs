// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

namespace Nethermind.DataMarketplace.Initializers
{
    public interface INdmCapabilityConnector
    {
        void Init();
        void AddCapability();
        bool CapabilityAdded { get; }
    }
}
