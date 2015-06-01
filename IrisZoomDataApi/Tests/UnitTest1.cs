using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Trad;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        string ndfbinfile = @"pc\ndf\patchable\gfx\everything.ndfbin";
        string className = "TAmmunitionDescriptor";
        string property = "Arme"; //uint32
        string ndffile = @"C:\Users\mja\Documents\perso\mods\NDF_Win.dat";
        
        string trans = @"C:\Users\mja\Documents\perso\mods\ZZ_Win.dat";
        string transFile = "pc\\localisation\\us\\localisation\\unites.dic";


        [TestMethod]
        public void LoadNdf()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();

            Assert.IsTrue(datamana.Files.Count > 0);
        }

        [TestMethod]
        public void LoadNdfbin()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);

            Assert.IsTrue(ndfbin.Classes.Count > 0);
        }


        [TestMethod]
        public void LoadClass()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass =  ndfbin.GetClass(className);

            Assert.IsTrue(claass.Instances.Count > 0);
        }

        [TestMethod]
        public void LoadInstance()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass(className);
            NdfObject obj = claass.Instances[0];

            Assert.IsTrue(obj.PropertyValues.Count > 0);
        }

        [TestMethod]
        public void LoadProperty()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass(className);
            NdfObject obj = claass.Instances[0];

            NdfUInt32 uint32;
            obj.TryGetValueFromQuery<NdfUInt32>(property, out uint32);

            Assert.IsTrue(obj.TryGetValueFromQuery<NdfUInt32>(property, out uint32));
        }

        [TestMethod]
        public void QueryReference()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass("TUniteDescriptor");
            NdfObject obj = claass.Instances[0];
            NdfObjectReference refef;

            string query = "Modules.TypeUnit.Default";
            Assert.IsTrue(obj.TryGetValueFromQuery<NdfObjectReference>(query, out refef));
        }

        [TestMethod]
        public void QueryListItem()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass("TUniteDescriptor");
            NdfObject obj = claass.Instances[1];
            NdfObjectReference refef;

            string query = "Modules[0].Default";
            Assert.IsTrue(obj.TryGetValueFromQuery<NdfObjectReference>(query, out refef));
        }

        [TestMethod]
        public void ReadDictionaryEntry()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass("TUniteDescriptor");
            NdfObject obj = claass.Instances[1];
            NdfLocalisationHash refef;
            string query = "Modules[0].Default.DescriptionHintToken";
            Assert.IsTrue(obj.TryGetValueFromQuery<NdfLocalisationHash>(query, out refef));
            

            EdataManager dic = new EdataManager(trans);
            dic.ParseEdataFile();
            TradManager trad = dic.ReadDictionary(transFile);
            string output = string.Empty;
            Assert.IsTrue(trad.TryGetString(refef.Value, out output)); // get LOSAT's description

        }
    }
}
