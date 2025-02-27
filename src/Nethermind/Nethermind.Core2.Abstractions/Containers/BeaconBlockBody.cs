// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

using System.Collections.Generic;
using System.Linq;
using Nethermind.Core2.Crypto;
using Nethermind.Core2.Types;

namespace Nethermind.Core2.Containers
{
    public class BeaconBlockBody
    {
        private readonly List<Attestation> _attestations;
        private readonly List<AttesterSlashing> _attesterSlashings;
        private readonly List<Deposit> _deposits;
        private readonly List<ProposerSlashing> _proposerSlashings;
        private readonly List<SignedVoluntaryExit> _voluntaryExits;

        public static readonly BeaconBlockBody Zero = new BeaconBlockBody(BlsSignature.Zero, Eth1Data.Zero,
            Bytes32.Zero, new ProposerSlashing[0], new AttesterSlashing[0], new Attestation[0], new Deposit[0],
            new SignedVoluntaryExit[0]);

        public BeaconBlockBody(
            BlsSignature randaoReveal,
            Eth1Data eth1Data,
            Bytes32 graffiti,
            IEnumerable<ProposerSlashing> proposerSlashings,
            IEnumerable<AttesterSlashing> attesterSlashings,
            IEnumerable<Attestation> attestations,
            IEnumerable<Deposit> deposits,
            IEnumerable<SignedVoluntaryExit> voluntaryExits)
        {
            RandaoReveal = randaoReveal;
            Eth1Data = eth1Data;
            Graffiti = graffiti;
            _proposerSlashings = new List<ProposerSlashing>(proposerSlashings);
            _attesterSlashings = new List<AttesterSlashing>(attesterSlashings);
            _attestations = new List<Attestation>(attestations);
            _deposits = new List<Deposit>(deposits);
            _voluntaryExits = new List<SignedVoluntaryExit>(voluntaryExits);
        }

        public IReadOnlyList<Attestation> Attestations { get { return _attestations; } }
        public IReadOnlyList<AttesterSlashing> AttesterSlashings { get { return _attesterSlashings; } }
        public IReadOnlyList<Deposit> Deposits { get { return _deposits; } }
        public Eth1Data Eth1Data { get; }
        public Bytes32 Graffiti { get; private set; }
        public IReadOnlyList<ProposerSlashing> ProposerSlashings { get { return _proposerSlashings; } }
        public BlsSignature RandaoReveal { get; private set; }

        public IReadOnlyList<SignedVoluntaryExit> VoluntaryExits { get { return _voluntaryExits; } }

        public void AddAttestations(Attestation attestation) => _attestations.Add(attestation);

        public void SetGraffiti(Bytes32 graffiti) => Graffiti = graffiti;

        public override string ToString()
        {
            return $"[rr={RandaoReveal.ToString().Substring(0, 10)} ac={Attestations.Count} asc={AttesterSlashings.Count} dc={Deposits.Count} psc={ProposerSlashings.Count}]";
        }

        /*
        public IList<Transfer> Transfers { get; }
        */
    }
}
