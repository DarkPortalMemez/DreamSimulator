using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DreamSImulation
{
    class Program
    {
        static void Main(string[] args)
        {
            int PearlsToStop;
            int PearlChance;
            int Simulations;
            #region pearlbartnerset
            Console.WriteLine("How many pearls until you stop bartnering? (leave blank for 12)");
        setPTS:
            string PTSString = Console.ReadLine();
            if (PTSString == string.Empty)
            {
                PearlsToStop = 12;
                goto doneSettingPTS;
            }
            try
            {
                Convert.ToInt32(PTSString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please only use digits 1 - 9, enter a new number");
                goto setPTS;
            }
            PearlsToStop = Convert.ToInt32(PTSString);
            if (PearlsToStop < 1)
            {
                Console.WriteLine("Please only use positive numbers, enter a new number");
                goto setPTS;
            }
        doneSettingPTS:
            #endregion
            #region pearlchanceset
            Console.WriteLine("Chance for pearl to drop (accurate to .1%) (leave blank for 4.7%)");
        setPC:
            string PCstring = Console.ReadLine();

            if (PCstring == string.Empty)
            {
                PearlChance = 47;
                goto doneSettingPC;
            }
            try
            {
                Convert.ToDouble(PCstring);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please only use digits 1 - 9, and a decimal ., enter a new number");
                goto setPC;
            }
            PearlChance = (int)(Convert.ToDouble(PCstring) * 10);
            if (PearlChance <= 0)
            {
                Console.WriteLine("Please only use positive numbers, enter a new number");
                goto setPC;
            }
        doneSettingPC:
            #endregion
            #region simset
            Console.WriteLine("simulations to run? (leave blank for 100k)");
        setSims:
            string SimString = Console.ReadLine();
            if (SimString == string.Empty)
            {
                Simulations = 100000;
                goto doneSettingSims;
            }
            try
            {
                Convert.ToInt32(SimString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Please only use digits 1 - 9, enter a new number");
                goto setSims;
            }
            Simulations = Convert.ToInt32(SimString);
            if (PearlsToStop < 1)
            {
                Console.WriteLine("Please only use positive numbers, enter a new number");
                goto setSims;
            }
        doneSettingSims:
            #endregion
            int[] percentages = new int[100000];
            Parallel.For(0, Simulations, (i) =>
            {
                int amount;
                amount = Simulate(PearlChance, PearlsToStop);
                int precentage;
                precentage = (int)((PearlChance / 10f) / amount * 100000F);
                percentages[precentage]++;
            });
            Console.WriteLine("Done simulating!");
            List<DoubleInt> filteredPrecentagesList = new List<DoubleInt>();
            int j = 0;
            for (int i = 0; i < percentages.Length; i++)
            {
                if (percentages[i] != 0)
                {
                    filteredPrecentagesList.Add(new DoubleInt {Int = percentages[i], UtillityInt = i});
                    j++;
                }
            }
            string[] printArray = new string[filteredPrecentagesList.Count];
            for (int i = 0; i < filteredPrecentagesList.Count; i++)
            {
                int amount = (int)Math.Round(1F / (filteredPrecentagesList[i].UtillityInt / 100000F) * (PearlChance / 10f));
                printArray[i] = $"{(float)PearlsToStop / (float)amount * 100F}% - {PearlsToStop} of {amount} | {filteredPrecentagesList[i].Int}";
            }
            Array.Reverse(printArray);
            System.IO.File.WriteAllLines(@"Output\DreamSim.txt", printArray);
            static int Simulate(int pearlChance, int pearlsToStop)
            {
                int trades = 0;
                Random pearlRandom = new Random();
                for (int pearls = 0; pearls < pearlsToStop;)
                {
                    trades++;
                    if (pearlRandom.Next(0, 1000) < pearlChance)
                    {
                        pearls++;
                    }
                }
                return trades;
            }
        }
        public class DoubleInt
        {
            public int Int { get; set; } = 0;
            public int UtillityInt { get; set; } = 0;
        }
    } 
}
