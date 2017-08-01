using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://trekhiringassignments.blob.core.windows.net/interview/bikes.json");
                
                List<Family> families= JsonConvert.DeserializeObject<List<Family>>(json);


                int TakeCombination = 25;
                List< BikesCombination> BikeCombo= CombinationsoFBike(families,TakeCombination);
                Console.WriteLine($"================ {TakeCombination} Combinations of bikes============");
                byte Counter = 1;
                foreach (var item in BikeCombo)
                {
                    Console.WriteLine($"  {Counter}: {item.Combination} = {item.Counter} families combination");
                    Counter++;
                }


                //MaximumBikeOwnedByFamily(families);
                Console.ReadLine();
                               
            }

        }

        private static List<BikesCombination> CombinationsoFBike(List<Family> families,int Take)
        {
            var Query = (from x in families
                         select new { x.Bikes }
                              ).ToList().Select(x => new { BikeCombi = string.Join(", ", x.Bikes) });
            Dictionary<string, int> ArrayCombibationCounter = new Dictionary<string, int>();

            foreach (var item in Query)
            {
                if (!ArrayCombibationCounter.Keys.Contains(item.BikeCombi))
                {
                    ArrayCombibationCounter.Add(item.BikeCombi, 1);
                }
                else
                {
                    int counter = ArrayCombibationCounter[item.BikeCombi];
                    ArrayCombibationCounter[item.BikeCombi] = (counter + 1);
                }
            }

            var TopTwentyCombination = (from x in ArrayCombibationCounter
                                        orderby x.Value descending
                                        select new BikesCombination { Combination= x.Key,Counter= x.Value }).Take(Take).ToList();

            return TopTwentyCombination;

        }

        private static void MaximumBikeOwnedByFamily(List<Family> families)
        {
            Console.WriteLine(""); Console.WriteLine("");
            var Query = (from x in families
                         select new { x.Bikes }
                            ).ToList();

            Dictionary<string, int> bikWithFamilyCounter = new Dictionary<string, int>();

            foreach (var item in Query)
            {
                foreach (var bik in item.Bikes)
                {

                    if (!bikWithFamilyCounter.Keys.Contains(bik))
                    {
                        bikWithFamilyCounter.Add(bik, 1);
                    }
                    else
                    {
                        int counter = bikWithFamilyCounter[bik];
                        bikWithFamilyCounter[bik] = (counter + 1);
                    }
                }
            }

            var query2 = (from x in bikWithFamilyCounter
                          orderby x.Value descending
                          select new { TotalFamily = x.Value, BikeName = x.Key }).ToList();

            byte Counter = 1;
            foreach (var item in query2)
            {
                Console.WriteLine($"  {Counter}: {item.BikeName} {item.TotalFamily} ");
                Counter++;
            }
        }
    }
    public class BikesCombination
    {
        public string Combination { get; set; }
        public int Counter{ get; set; }
    }

    public class Bikes
    {
        public string BikeName { get; set; }
        public int FamilyCounter { get; set; }

    }

    public class Family
    {
        public List<string> Bikes{ get; set; }
    }
}
