# Skyline.DataMiner.Utils.SNMP.Traps

## About

Skyline.DataMiner.Utils.SNMP.Traps is a NuGet package in the public [nuget store](https://www.nuget.org/) that contain assemblies that assist in developing connectors dealing with SNMP Traps.

Specifically they contain the TrapInfo class which allows the creation of a TrapInfo object from the raw trapInfo object as provided by a protocol TrapReceiver parameter using allBindingInfo.

```xml
<Param id="1">
  <Name>Trap Receiver</Name>
  <Description>Trap Received</Description>
  <Type>read</Type>
  <Interprete>
     <Type>string</Type>
  </Interprete>
  <SNMP>
     <Enabled>true</Enabled>
     <TrapOID setBindings="allBindingInfo" type="wildcard" ipid="100">...</TrapOID>
  </SNMP>
</Param>
```

```cs
public class QAction
{
   public static void Run(SLProtocol protocol, object trapInfo)
  {
      // ...
  }
```

> Documentation: [SNMP Traps](https://docs.dataminer.services/develop/devguide/Connector/ConnectionsSnmpTraps.html)

## Requirements

The "DataMiner Integration Studio" Visual Studio extension is required for development of connectors and Automation scripts using NuGets.

See [Installing DataMiner Integration Studio](https://aka.dataminer.services/DisInstallation)

> [!IMPORTANT]
> NuGets are mandatory to be installed with PackageReferences. DIS was redesigned to work with PackageReferences and be future-proof.
>
> For more information on how to migrate from packages.config to PackageReferences, see [docs.microsoft.com](https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference).

## Getting started

DIS includes several snippets to allow an easy start with this library.

The following code snippet shortcuts are available and can be autocompleted:

- `clTrapInfo`

This provides the initial call to transform the object trapInfo into a more userfriendly TrapInfo class.
