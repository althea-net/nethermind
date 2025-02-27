// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

using System;
using FluentAssertions;
using Nethermind.Core;
using Nethermind.DataMarketplace.Consumers.Shared;
using Nethermind.DataMarketplace.Core.Events;
using Nethermind.DataMarketplace.Initializers;
using Nethermind.Logging;
using Nethermind.Network;
using Nethermind.Network.P2P;
using Nethermind.Network.P2P.EventArg;
using Nethermind.Network.P2P.ProtocolHandlers;
using Nethermind.Stats.Model;
using NSubstitute;
using NUnit.Framework;

namespace Nethermind.DataMarketplace.Test.Initializers
{
    public class NdmCapabilityConnectorTests
    {
        private readonly Capability _capability = new Capability(Protocol.Ndm, 1);
        private IProtocolsManager _protocolsManager;
        private IProtocolHandlerFactory _protocolHandlerFactory;
        private IAccountService _accountService;
        private ILogManager _logManager;
        private Address _providerAddress;
        private Address _consumerAddress;
        private INdmCapabilityConnector _capabilityConnector;

        [SetUp]
        public void Setup()
        {
            _protocolsManager = Substitute.For<IProtocolsManager>();
            _protocolHandlerFactory = Substitute.For<IProtocolHandlerFactory>();
            _accountService = Substitute.For<IAccountService>();
            _logManager = LimboLogs.Instance;
            _providerAddress = Address.Zero;
            _consumerAddress = Address.Zero;
            _capabilityConnector = new NdmCapabilityConnector(
                _protocolsManager,
                _protocolHandlerFactory,
                _accountService,
                _logManager,
                _providerAddress);
        }

        [Test]
        public void init_should_add_capability_for_valid_address()
        {
            _consumerAddress = Address.FromNumber(1);
            _accountService.GetAddress().Returns(_consumerAddress);
            _capabilityConnector.Init();
            _protocolsManager.Received().AddProtocol(Protocol.Ndm, Arg.Any<Func<ISession, IProtocolHandler>>());
            _accountService.Received().GetAddress();
            _protocolsManager.Received().AddSupportedCapability(_capability);
            _protocolsManager.P2PProtocolInitialized += Raise.EventWith(_protocolsManager, new ProtocolInitializedEventArgs(null));
            _capabilityConnector.CapabilityAdded.Should().BeTrue();
        }

        [Test]
        public void init_should_not_add_capability_for_invalid_address()
        {
            _consumerAddress = Address.Zero;
            _providerAddress = Address.Zero;
            _accountService.GetAddress().Returns(_consumerAddress);
            _capabilityConnector.Init();
            _accountService.Received().GetAddress();
            _protocolsManager.DidNotReceiveWithAnyArgs().AddSupportedCapability(_capability);
            _capabilityConnector.CapabilityAdded.Should().BeFalse();
        }

        [Test]
        public void capability_should_be_added_when_consumer_address_is_changed_to_valid_one()
        {
            _consumerAddress = Address.Zero;
            _providerAddress = Address.Zero;
            _accountService.GetAddress().Returns(_consumerAddress);
            _capabilityConnector.Init();
            _accountService.Received().GetAddress();
            _protocolsManager.DidNotReceiveWithAnyArgs().AddSupportedCapability(_capability);
            _capabilityConnector.CapabilityAdded.Should().BeFalse();

            var newConsumerAddress = Address.FromNumber(2);
            _accountService.AddressChanged += Raise.EventWith(_accountService,
                new AddressChangedEventArgs(_consumerAddress, newConsumerAddress));
            _capabilityConnector.CapabilityAdded.Should().BeTrue();
        }

        [Test]
        public void capability_should_not_be_added_again_when_consumer_address_is_changed()
        {
            _consumerAddress = Address.FromNumber(1);
            _accountService.GetAddress().Returns(_consumerAddress);
            _capabilityConnector.Init();
            _protocolsManager.Received().AddProtocol(Protocol.Ndm, Arg.Any<Func<ISession, IProtocolHandler>>());
            _accountService.Received().GetAddress();
            _protocolsManager.Received().AddSupportedCapability(_capability);
            _protocolsManager.P2PProtocolInitialized += Raise.EventWith(_protocolsManager, new ProtocolInitializedEventArgs(null));
            _capabilityConnector.CapabilityAdded.Should().BeTrue();
            _protocolsManager.ClearReceivedCalls();

            var newConsumerAddress = Address.FromNumber(2);
            _accountService.AddressChanged += Raise.EventWith(_accountService,
                new AddressChangedEventArgs(_consumerAddress, newConsumerAddress));

            _protocolsManager.DidNotReceiveWithAnyArgs().AddSupportedCapability(_capability);
        }

        [Test]
        public void add_capability_should_succeed()
        {
            _capabilityConnector.AddCapability();
            _protocolsManager.Received().AddSupportedCapability(_capability);
            _protocolsManager.Received().SendNewCapability(_capability);
            _capabilityConnector.CapabilityAdded.Should().BeTrue();
        }
    }
}
