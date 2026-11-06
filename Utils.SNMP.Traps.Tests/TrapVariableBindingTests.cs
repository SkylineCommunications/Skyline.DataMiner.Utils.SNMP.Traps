namespace Skyline.DataMiner.Utils.SNMP.Traps.Tests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.SNMP.Traps.Protocol;

	[TestClass]
	public class TrapInfoVariableBindingCollectionTests
	{
		private TrapInfoVariableBindingCollection testBindings;

		public TrapInfoVariableBindingCollectionTests()
		{
			testBindings = new TrapInfoVariableBindingCollection
			{
				new TrapInfoVariableBinding("1.1.1.1", "55"),
				new TrapInfoVariableBinding("1.1.1.2", "56"),
				new TrapInfoVariableBinding("1.1.1.3", "57"),
				new TrapInfoVariableBinding("1.1.1.4", "Test")
			};
		}

		[TestMethod]
		public void Count_InstanceWithFourBindings_ReturnsFour()
		{
			// Arrange.

			// Act.
			int result = testBindings.Count;

			// Assert.
			result.Should().Be(4);
		}

		[TestMethod]
		public void Contains_ValidOid_ReturnsTrue()
		{
			// Arrange.

			// Act.
			bool result = testBindings.Contains("1.1.1.1");

			// Assert.
			result.Should().BeTrue();
		}

		[TestMethod]
		public void Contains_InvalidOid_ReturnsFalse()
		{
			// Arrange.

			// Act.
			bool result = testBindings.Contains("1.1.1.10");

			// Assert.
			result.Should().BeFalse();
		}

		[TestMethod]
		public void Contains_ValidBinding_ReturnsTrue()
		{
			// Arrange.
			var binding = new TrapInfoVariableBinding("1.1.1.2", "56");

			// Act.
			bool result = testBindings.Contains(binding);

			// Assert.
			result.Should().BeTrue();
		}

		[TestMethod]
		public void Contains_InvalidBinding_ReturnsFalse()
		{
			// Arrange.
			var binding = new TrapInfoVariableBinding("1.1.1.2", "0");

			// Act.
			bool result = testBindings.Contains(binding);

			// Assert.
			result.Should().BeFalse();
		}

		[TestMethod]
		public void Contains_InvalidOid_ThrowsArgumentNullException()
		{
			// Arrange.

			// Act.
			Action act = () => testBindings.Contains(null);

			// Assert.
			act.Should().ThrowExactly<ArgumentNullException>();
		}

		[TestMethod]
		public void IndexOf_ValidBinding_ReturnsCorrectValue()
		{
			// Arrange.
			var binding = new TrapInfoVariableBinding("1.1.1.2", "56");

			// Act.
			int index = testBindings.IndexOf(binding);

			index.Should().Be(1);
		}

		[TestMethod]
		public void IndexOf_InvalidBinding_ReturnsMinusOne()
		{
			// Arrange.
			var binding = new TrapInfoVariableBinding("1.1.1.2", "0");

			// Act.
			int index = testBindings.IndexOf(binding);

			// Assert.
			index.Should().Be(-1);
		}

		[TestMethod]
		public void TryGetValue_ValidBinding_ReturnsTrue()
		{
			// Arrange.
			var expectedBinding = new TrapInfoVariableBinding("1.1.1.4", "Test");

			// Act.
			TrapInfoVariableBinding binding;
			bool result = testBindings.TryGetValue("1.1.1.4", out binding);

			// Assert.
			result.Should().BeTrue();
			binding.Should().BeEquivalentTo(expectedBinding);
		}

		[TestMethod]
		public void TryGetValue_InvalidBinding_ReturnsFalse()
		{
			// Arrange.

			// Act.
			TrapInfoVariableBinding binding;
			bool result = testBindings.TryGetValue("1.1.1.55", out binding);

			// Assert.
			result.Should().BeFalse();
			binding.Should().BeNull();
		}

		[TestMethod]
		public void TryGetValue_NullReference_ThrowsArgumentNullException()
		{
			// Arrange.

			// Act.
			Action act = () => testBindings.TryGetValue(null, out _);

			// Assert.
			act.Should().ThrowExactly<ArgumentNullException>();
		}

		[TestMethod]
		public void IndexerInt32_IndexZero_ReturnsFirstBinding()
		{
			// Arrange.
			var expectedBinding = new TrapInfoVariableBinding("1.1.1.1", "55");

			// Act.
			var resultBinding = testBindings[0];

			// Assert.
			resultBinding.Should().BeEquivalentTo(expectedBinding);
		}

		[TestMethod]
		public void IndexerInt32_LastIndex_ReturnsLastBinding()
		{
			// Arrange.
			var expectedBinding = new TrapInfoVariableBinding("1.1.1.4", "Test");

			// Act.
			var resultBinding = testBindings[3];

			// Assert.
			resultBinding.Should().BeEquivalentTo(expectedBinding);
		}

		[TestMethod]
		public void IndexerInt32_InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange.

			// Act.
			Func<TrapInfoVariableBinding> func = () => testBindings[10];

			// Assert.
			func.Should().ThrowExactly<ArgumentOutOfRangeException>();
		}

		[TestMethod]
		public void IndexerInt32_NegativeIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange.

			// Act.
			Func<TrapInfoVariableBinding> func = () => testBindings[-1];

			// Assert.
			func.Should().ThrowExactly<ArgumentOutOfRangeException>();
		}

		[TestMethod]
		public void IndexerString_LastOid_ReturnsLastBinding()
		{
			// Arrange.
			var expectedBinding = new TrapInfoVariableBinding("1.1.1.4", "Test");

			// Act.
			var returnedBinding = testBindings["1.1.1.4"];

			// Assert.
			returnedBinding.Should().BeEquivalentTo(expectedBinding);
		}

		[TestMethod]
		public void IndexerString_FirstOid_ReturnsFirstBinding()
		{
			// Arrange.
			var expectedBinding = new TrapInfoVariableBinding("1.1.1.1", "55");

			// Act.
			var returnedBinding = testBindings["1.1.1.1"];

			// Assert.
			returnedBinding.Should().BeEquivalentTo(expectedBinding);
		}

		[TestMethod]
		public void IndexerString_InvalidOid_ThrowsKeyNotFoundException()
		{
			// Arrange.

			// Act.
			Func<TrapInfoVariableBinding> func = () => testBindings["1.1.1.74"];

			// Assert.
			func.Should().ThrowExactly<KeyNotFoundException>();
		}

		[TestMethod]
		public void IndexerString_InvalidOid_ThrowsArgumentNullException()
		{
			// Arrange.

			// Act.
			Func<TrapInfoVariableBinding> func = () => testBindings[null];

			// Assert.
			func.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}