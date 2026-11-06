namespace Skyline.DataMiner.Utils.SNMP.Traps.Tests
{
    using System;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Skyline.DataMiner.Utils.SNMP.Traps.Protocol;

    [TestClass]
    public class TrapInfoTests
    {
        private object testTrap;
        private int nowMinusTenMinutes;

        public TrapInfoTests()
        {
            // The TrapInfo Object.
            // The trapInfo object is an array holding arrays.
            //  The first entry contains an array with general trap information:
            //		* trap OID.
            //		* trap source(IP address).
            //		* tick count indicating when the trap was received by SLSNMPManager.
            //  The remaining entries contain arrays where each array holds two items representing the binding:
            //		* binding OID.
            //		* binding value.
            nowMinusTenMinutes = Environment.TickCount - 600000;

            object[] generalData = new object[] { "5574.114.22.1.3.4", "10.1.0.5", nowMinusTenMinutes };
            object[] bindingOne = new object[] { "1.1.1.1", 55 };
            object[] bindingTwo = new object[] { "1.1.1.2", 56 };
            object[] bindingThree = new object[] { "1.1.1.3", 57 };
            object[] bindingFour = new object[] { "1.1.1.4", "Test" };

            testTrap = new object[] { generalData, bindingOne, bindingTwo, bindingThree, bindingFour };
        }

        [TestMethod]
        public void FromTrapData_NullReference_ThrowsArgumentException()
        {
            // Arrange.

            // Act.
            Action act = () => TrapInfo.FromTrapData(null);

            // Assert.
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void Bindings_FromValidObject_Succeeds()
        {
            // Arrange.
            var trap = TrapInfo.FromTrapData(testTrap);

            // Act.
            TrapInfoVariableBindingCollection testBindings = new TrapInfoVariableBindingCollection();
            testBindings.Add(new TrapInfoVariableBinding("1.1.1.1", "55"));
            testBindings.Add(new TrapInfoVariableBinding("1.1.1.2", "56"));
            testBindings.Add(new TrapInfoVariableBinding("1.1.1.3", "57"));
            testBindings.Add(new TrapInfoVariableBinding("1.1.1.4", "Test"));

            // Assert.
            trap.VariableBindings.Should().BeEquivalentTo(testBindings, options => options.WithStrictOrdering());
        }

        [TestMethod]
        public void FromTrapData_InvalidObject_ThrowsArgumentException()
        {
            // Arrange.
            object o = new object[] { new object[] { 1, 2, 3 }, 1, 2 };

            // Act.
            Action act = () => TrapInfo.FromTrapData(o);

            // Assert.
            act.Should().ThrowExactly<ArgumentException>().WithMessage("Retrieved invalid trap information.*");
        }

        [TestMethod]
        public void FromTrapData_DuplicateBinding_Succeeds()
        {
            // Code shouldn't crash when the device returns the same binding twice.
            // https://community.dataminer.services/question/snmp-traps-with-duplicate-binding-oids-causing-issues-parsing/

            // Arrange.
            object[] generalData = new object[] { "5574.114.22.1.3.4", "10.1.0.5", nowMinusTenMinutes };
            object[] bindingOne = new object[] { "1.1.1.1", 55 };

            object o = new object[] { generalData, bindingOne, bindingOne };

            // Act.
            Action act = () => TrapInfo.FromTrapData(o);

            // Assert.
            act.Should().NotThrow();
        }

        [TestMethod]
        public void Oid_FromValidObject_Succeeds()
        {
            // Arrange.
            var trap = TrapInfo.FromTrapData(testTrap);

            // Act.
            var oid = trap.Oid;

            // Assert.
            oid.Should().Be("5574.114.22.1.3.4");
        }

        [TestMethod]
        public void Source_FromValidObject_Succeeds()
        {
            // Arrange.
            var trap = TrapInfo.FromTrapData(testTrap);

            // Act.
            var source = trap.Source;

            // Assert.
            source.ToString().Should().Be("10.1.0.5");
        }

        //[TestMethod]
        //public void Ticks_FromValidObject_Succeeds()
        //{
        //	// Arrange.
        //	var trap = TrapInfo.FromTrapData(testTrap);

        //	// Act.
        //	var ticks = trap.Ticks;

        //	// Assert.
        //	ticks.Should().Be(nowMinusTenMinutes);
        //}

        [TestMethod]
        public void ReceivedTime_FromValidObject_Succeeds()
        {
            // Arrange.
            DateTime expectedDateTime = DateTime.Now - TimeSpan.FromMinutes(10);
            var trap = TrapInfo.FromTrapData(testTrap);

            // Act.
            var time = trap.ReceivedTime;

            // Assert.
            time.Should().BeWithin(new TimeSpan(0, 0, 0, 1)).After(expectedDateTime);
        }
    }
}
