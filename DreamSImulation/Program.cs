using System;
using System.IO;

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
            if (PTSString == string.Empty) {
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
            if (PearlsToStop < 1) {
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
            PearlChance = (int)(Convert.ToDouble(PCstring)*10);
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
            int[] tradeCountArray = new int[Simulations];
            for (int i = 0; i < tradeCountArray.Length; i++)
            {
                tradeCountArray[i] = Simulate(PearlChance, PearlsToStop);
            }
            Array.Sort(tradeCountArray);
            string[] tradeStringArray = new string[tradeCountArray.Length];
            for (int i = 0; i < tradeStringArray.Length; i++)
            {
                float percentage =  (float)PearlsToStop / tradeCountArray[i] * 100F;
                tradeStringArray[i] = $"{tradeCountArray[i]} - {percentage}% | the #{i+1} luckiest simulation";
            }
            System.IO.Directory.CreateDirectory(@"Output");
            System.IO.File.WriteAllLines(@"Output\DreamSim.txt", tradeStringArray);

        }
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
}
