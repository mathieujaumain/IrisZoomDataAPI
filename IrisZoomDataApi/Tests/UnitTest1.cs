using System;
using System.Linq;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Edata;
using IrisZoomDataApi.Model.Texture;
using IrisZoomDataApi.BL.ImageService;
using IrisZoomDataApi.BL.TGV;
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
        public static string UNIT_REA_COST = "Modules.Production.Default.ProductionRessourcesNeeded.14";
        public static string UNIT_ICON = "Modules.TypeUnit.Default.TextureForInterface.FileName";


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
            NdfObject obj = claass.Instances[1];

            NdfString uint32;
            obj.TryGetValueFromQuery<NdfString>(UNIT_ICON, out uint32);
        }

        [TestMethod]
        public void QueryReference()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass("TUniteDescriptor");
            NdfObject obj = claass.Instances[1];
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
            string query = "Modules.TypeUnit.Default.DescriptionHintToken";
            Assert.IsTrue(obj.TryGetValueFromQuery<NdfLocalisationHash>(query, out refef));
            
            EdataManager dic = new EdataManager(trans);
            dic.ParseEdataFile();
            TradManager trad = dic.ReadDictionary(transFile);
            string output = string.Empty;
            Assert.IsTrue(trad.TryGetString(refef.Value, out output)); // get LOSAT's description

        }

        [TestMethod]
        public void ReadCost()
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
            NdfClass claass = ndfbin.GetClass("TUniteDescriptor");
            NdfObject obj = claass.Instances[1];
            NdfUInt32 refef;
            Assert.IsTrue(obj.TryGetValueFromQuery<NdfUInt32>(UNIT_REA_COST, out refef));

        }

        [TestMethod]
        public void  LoadTGV()
        {
            EdataManager manager = new EdataManager(@"C:\Users\mja\Documents\perso\mods\commoninterface.ppk");
            manager.ParseEdataFile();

            string filename = @"pc\texture\assets\2d\interface\common\unitsicons\us\stryker_icv_upgrade_1.tgv";
            Bitmap bitmap = null;
            Assert.IsTrue(manager.TryToLoadTgv(filename, out bitmap));
            Assert.IsNotNull(bitmap);
            bitmap.Save("Unit2.png");
        }

        [TestMethod]
        public void LoadTGVFromZZ4()
        {
            string zz4 = @"C:\Users\mja\Documents\perso\mods\ZZ_4.dat";

            EdataManager zz4File = new EdataManager(zz4);
            zz4File.ParseEdataFile();

            string filename = @"pc\texture\assets\2d\interface\common\unitsicons\cs\m8_losat_upb.tgv";

            string ICON_PACKAGE = @"pc\texture\pack\commoninterface.ppk";
            EdataManager pack = zz4File.ReadPackage(ICON_PACKAGE);


            Bitmap bitmap = null;
            Assert.IsTrue(pack.TryToLoadTgv(filename, out bitmap));
            Assert.IsNotNull(bitmap);
            bitmap.Save("UnitDXT1.png");
        }
    }
}
