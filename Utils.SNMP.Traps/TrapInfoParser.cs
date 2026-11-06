namespace Skyline.DataMiner.Utils.SNMP.Traps.Protocol
{
	using System;
	using System.Globalization;
	using System.Net;

	internal class TrapInfoParser
	{
		private TrapInfoParser()
		{
		}

		/// <summary>
		/// Parses the object returned from SLProtocol when using allBindingInfo on a parameter in a DataMiner Protocol.
		/// </summary>
		/// <param name="trapInfo">The object returned from SLProtocol when using allBindingInfo on a parameter in a DataMiner Protocol.</param>
		/// <exception cref="ArgumentNullException"><paramref name="trapInfo"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="trapInfo"/> is invalid.</exception>
		/// <exception cref="FormatException"><paramref name="trapInfo"/>Source is not a valid IP address.</exception>
		/// <returns>An instance of the <see cref="TrapInfo"/> class.</returns>
		/// <example>
		/// <code>TrapInfo trap = TrapInfoParser.Parse(trapInfo);</code>
		/// </example>
		public static TrapInfo Parse(object trapInfo)
		{
			if (trapInfo == null)
			{
				throw new ArgumentNullException(nameof(trapInfo));
			}

			if (!(trapInfo is object[] trapInfoArray) || trapInfoArray.Length < 1)
			{
				throw new ArgumentException("Retrieved invalid trap information.", nameof(trapInfo));
			}

			if (!(trapInfoArray[0] is object[] generalTrapInfo) || generalTrapInfo.Length < 3)
			{
				throw new ArgumentException("Retrieved invalid trap information: generalTrapInfo was not the correct array.", nameof(trapInfo));
			}

			string oid = Convert.ToString(generalTrapInfo[0], CultureInfo.CurrentCulture);

			IPAddress source = IPAddress.Parse(Convert.ToString(generalTrapInfo[1], CultureInfo.CurrentCulture));
			int ticks = GetTicks(generalTrapInfo[2]);

			TrapInfoVariableBindingCollection trapBindings = GetBindings(trapInfoArray);

			return new TrapInfo(oid, source, ticks, new ReadOnlyTrapInfoVariableBindingCollection(trapBindings));
		}

		private static TrapInfoVariableBindingCollection GetBindings(object[] trapInfoArray)
		{
			TrapInfoVariableBindingCollection bindings = new TrapInfoVariableBindingCollection();

			for (int i = 1; i < trapInfoArray.Length; i++)
			{
				if (!(trapInfoArray[i] is object[] binding) || binding.Length < 2)
				{
					throw new ArgumentException("Retrieved invalid trap information.");
				}

				string oid = Convert.ToString(binding[0], CultureInfo.CurrentCulture);
				string value = Convert.ToString(binding[1], CultureInfo.CurrentCulture);

				// some devices return the same item twice!
				// https://community.dataminer.services/question/snmp-traps-with-duplicate-binding-oids-causing-issues-parsing/

				if(!bindings.Contains(oid))
				{
					bindings.Add(new TrapInfoVariableBinding(oid, value));
				}
			}

			return bindings;
		}

		private static int GetTicks(object timeInfo)
		{
			if (Int32.TryParse(Convert.ToString(timeInfo, CultureInfo.CurrentCulture), out int tick))
			{
				return tick;
			}

			throw new ArgumentException("Retrieved invalid time ticks in trap information");
		}
	}
}