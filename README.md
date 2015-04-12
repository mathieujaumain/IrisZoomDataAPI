# IrisZoomDataAPI

Library meant to provide an *whatever* way to query data from NDF binaries and dictionaries files used 
in IrisZoom engine games but more specifically the Wargame serie and, hopefully, the incoming Act of Agression.

##USE :
Easy (ha !) 

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


Of course, that suppose you know the data model, so it's always best to have enohka's modding tools nearby to check your query is making sense.


By the way, NbfNul and NdfUnset are treated as null value. Because I decided so.


Ri... Fork of :
https://github.com/enohka/moddingSuite

Meaning, it's basically the same data model cleaned from that VMMV nonsense and powered by some nasty helper methods. :D
