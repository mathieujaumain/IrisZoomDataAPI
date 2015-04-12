using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Edata;
using IrisZoomDataApi.Model.Trad;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;


namespace IrisZoomTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGettingFile()
        {
            EdataManager datamanager = new EdataManager(Environment.CurrentDirectory + "/data/NDF_Win.dat");
            datamanager.ParseEdataFile();

            NdfbinManager ndfFile = datamanager.ReadNdfbin("pc\\ndf\\patchable\\gfx\\everything.ndfbin");

            Assert.IsNotNull(ndfFile);
            Assert.IsTrue(ndfFile.Classes.Count > 1);


            NdfClass ammoClass = ndfFile.GetClass("TAmmunition");

            foreach (NdfObject ammo in ammoClass.Instances)
            {
                NdfValueWrapper val;
                Assert.IsTrue(ammo.TryGetValueFromPath("TempsEntreDeuxTirs", out val));
            }
        }
    }
}
