// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

using Nethermind.Core;
using Nethermind.Core.Specs;
using Nethermind.Int256;
using Nethermind.Specs.Forks;

namespace Nethermind.Specs
{
    public class SingleReleaseSpecProvider : ISpecProvider
    {
        private ForkActivation? _theMergeBlock = null;

        public void UpdateMergeTransitionInfo(long? blockNumber, UInt256? terminalTotalDifficulty = null)
        {
            if (blockNumber is not null)
                _theMergeBlock = blockNumber;
            if (terminalTotalDifficulty is not null)
                TerminalTotalDifficulty = terminalTotalDifficulty;
        }

        public ForkActivation? MergeBlockNumber => _theMergeBlock;
        public UInt256? TerminalTotalDifficulty { get; set; }
        public ulong ChainId { get; }
        public ForkActivation[] TransitionBlocks { get; } = { 0 };

        private readonly IReleaseSpec _releaseSpec;

        public SingleReleaseSpecProvider(IReleaseSpec releaseSpec, ulong networkId)
        {
            ChainId = networkId;
            _releaseSpec = releaseSpec;
            if (_releaseSpec == Dao.Instance)
            {
                DaoBlockNumber = 0;
            }
        }

        public IReleaseSpec GenesisSpec => _releaseSpec;

        public IReleaseSpec GetSpec(ForkActivation forkActivation) => _releaseSpec;

        public long? DaoBlockNumber { get; }
    }
}
