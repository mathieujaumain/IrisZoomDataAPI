# IrisZoomDataAPI

Library meant to provide an acceptable way to query data from NDF binaries and dictionaries files used 
in IrisZoom engine games but more specifically the Wargame serie and Act of Agression.

##USE :

1) Read your eDat File to make an EdataManager object :
EdataManager edata = new EdataManager(path2file);
edata.ParseEdataFile();

2) Get the binary file you want to query from :
 NdfbinManager everything = edata.ReadNdfbin("pc\\ndf\\patchable\\gfx\\everything.ndfbin");
 
3) Get an ndf object :
NdfObject TAmmoInstances = everything.GetClass("TAmmunition").Instances[0];

4) Query the data using string queries !
NdfUInt32 uintVal;
TAmmoInstances.TryGetValueFromPath<NdfUInt32>("NbrProjectilesSimultanes", out uintVal)
uint simulatenousProjectiles =  uintVal.Value;

Or, from a TUniteDescriptor (Act of Aggression unit class) instance : "Modules.WeaponManager.Default.Turrets[0].MountedWeapon[0].Ammunition.NbrProjectilesSimultanes"

Supported : All ndfbin types, type collections, map types, object reference


Of course, that suppose you know the data model, so it's always best to have enohka's modding tools nearby to check your query is making sense.

By the way, NbfNul and NdfUnset are treated as null value. Because I decided so.

Examples are avaible in the test project.

###Thanks
To Enohka and Hob and all those that directly and indirectly participated in the moddingSuite project.

Ri... Fork of :
https://github.com/enohka/moddingSuite

Meaning, it's basically the same data model cleansed from that VMMV nonsense and powered by some nasty helper methods. :D

###What's next :

TGV (Eugen's-I think- texture file) to png queries (very soon :>)

Sounds (ess files) queries (probably never)