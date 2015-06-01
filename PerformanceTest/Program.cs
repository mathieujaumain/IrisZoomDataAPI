using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;

namespace PerformanceTest
{


    class Program
    {

        static string ndfbinfile = @"pc\ndf\patchable\gfx\everything.ndfbin";
        static string className = "TAmmunitionDescriptor";
        static string property = "Arme"; //uint32
        static string ndffile = @"C:\Users\mja\Documents\perso\mods\NDF_Win.dat";

        static string trans = @"C:\Users\mja\Documents\perso\mods\ZZ_Win.dat";
        static string transFile = "pc\\localisation\\us\\localisation\\unites.dic";

        static void Main(string[] args)
        {
            EdataManager datamana = new EdataManager(ndffile);

            datamana.ParseEdataFile();
            NdfbinManager ndfbin = datamana.ReadNdfbin(ndfbinfile);
        }
    }
}
