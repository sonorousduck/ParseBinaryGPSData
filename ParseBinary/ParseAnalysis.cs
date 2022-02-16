using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseBinary
{
    public class ParseAnalysis
    {
        public List<Bin16Msg> Bin16Msgs;
        public List<double> differences { get; private set; }
        public List<double> bigDifferences { get; private set; }
        private List<int> bigDifferenceIds { get; set; }
        public double maxDifference { get; private set; }
        public int MaxDifferenceLocation { get; private set; }
        public Bin16Msg LastBin16Msg { get; set; }
        public int HzRate { get; set; }
        

        public ParseAnalysis(List<Bin16Msg> bin16Msgs)
        {
            this.Bin16Msgs = bin16Msgs;
            this.maxDifference = 0;
            this.MaxDifferenceLocation = 0;
            this.differences = new List<double>();
            this.bigDifferences = new List<double>();
            this.bigDifferenceIds = new List<int>();
            this.HzRate = 1;
            
            GetDifferences();
            

        }

        public void GetDifferences()
        {
            for (int i = 0; i < Bin16Msgs.Count - 1; i++)
            {
                double difference = this.Bin16Msgs[i + 1].TimeInSeconds - this.Bin16Msgs[i].TimeInSeconds;
                differences.Add(difference);

                if (difference > this.HzRate + 1)
                {
                    bigDifferences.Add(difference);
                    bigDifferenceIds.Add(i);
                }


                // Can't just use max(x, y) so I can track which one had the biggest difference
                if (this.maxDifference < difference)
                {
                    this.maxDifference = difference;
                    this.MaxDifferenceLocation = i;
                }
            }


        }

        public string GetLastBinMessage()
        {
            this.LastBin16Msg = Bin16Msgs[^1];
            return this.LastBin16Msg.DisplayTimeStamp();
        }


        public double GetAvgDifference()
        {
            double sumDifference = 0;
            for (int i = 0; i < differences.Count; i++)
            {
                sumDifference += differences[i];
            }

            if (differences.Count == 0)
            {
                //Fringe, testing case. Return 0.0
                return 0.0d;
            }


            return (sumDifference / differences.Count);
        }


        public double StdDeviation()
        {
            double stdDeviation = 0;
            double avg = GetAvgDifference();


            foreach (var difference in differences)
            {
                stdDeviation += Math.Pow((difference - avg), 2);
            }

            if (differences.Count == 0)
            {
                //Fringe, testing case. Return 0.0
                return 0.0d;
            }

            stdDeviation /= differences.Count;

            return Math.Sqrt(stdDeviation);
        }

        public void PrintBigDifferences()
        {
            Console.Write("Big difference in time detected. Value of: \n");
            for (var i = 0; i < bigDifferences.Count; i++)
            {

                Console.WriteLine("   " + bigDifferences[i] + " at " + bigDifferenceIds[i] + " - (" + this.Bin16Msgs[bigDifferenceIds[i]].DisplayTimeStamp() + " - " + this.Bin16Msgs[bigDifferenceIds[i] + 1].DisplayTimeStamp() + ")");
            }
        }

    }
}
