using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using ParseBinary;

namespace ParseBinaryTest
{

    [TestFixture]
   public class ParseBinaryTest
   {
       public ParseBinary.ParseBinary SmallBinary;
       public ParseBinary.ParseBinary LargeBinary;
       public ParseBinary.ParseBinary NewBinary;
       public ParseBinary.ParseBinary BluetoothDump;
       public ParseBinary.ParseBinary SerialDump;

        public ParseAnalysis ParseAnalysis;
       public ParseAnalysis ParseAnalysisLarge;
       public ParseAnalysis ParseAnalysisNew;
       public ParseAnalysis BluetoothDumpAnalysis;
       public ParseAnalysis SerialDumpAnalysis;
       private ParseJSON pj;
       private String Filename;
       private List<(string Timestamp, Bin16Msg)> JsonData;


        [OneTimeSetUp]
       public void Setup()
       {
/*           SmallBinary = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\TestBin.bin");
           SmallBinary.ParseData();
           LargeBinary =
               new ParseBinary.ParseBinary(
                   "C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\BTDumpXcode 2.bin");
           LargeBinary.ParseData();
           ParseAnalysis = new ParseAnalysis(SmallBinary.BinaryMsgs);
           ParseAnalysisLarge = new ParseAnalysis(LargeBinary.BinaryMsgs);
           NewBinary = new ParseBinary.ParseBinary(
               "C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FirstUsefulRun\\SecondRun.bin");
            NewBinary.ParseData();
            ParseAnalysisNew = new ParseAnalysis(NewBinary.BinaryMsgs);*/

            BluetoothDump = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FourthRun\\run3.bin");
            BluetoothDump.ParseData();
            BluetoothDumpAnalysis = new ParseAnalysis(BluetoothDump.BinaryMsgs);

            SerialDump = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FourthRun\\run3Serial.bin");
            SerialDump.ParseData();
            SerialDumpAnalysis = new ParseAnalysis(SerialDump.BinaryMsgs);

            pj = new ParseJSON();
            Filename = "C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FourthRun\\run3.json";
            JsonData = pj.JSONParse(Filename);
          




        }

        /*[Test]
        public void LoadBinaryFile()
        {
           //var pb = new ParseBinary.ParseBinary();
           var pb = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\BTDumpXcode 2.bin");


           var result = pb.LoadBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\BTDumpXcode 2.bin");
           Assert.AreEqual(true, result);
        }*/

        /*     [Test]
           public void getHeader()
           {
               // Tests to make sure it pulls the header for a binary message


               List<byte> expectedList = new List<byte>();

               // Decimal values for the binary file
               var byteArray = new byte[] { 36, 66, 73, 78, 16, 0, 56, 1 };
     *//*          var holdThis = new byte[] {24, 42, 49, 4E};*//*
               expectedList.AddRange(byteArray.ToList());

               Assert.AreEqual(expectedList, SmallBinary.BinaryMsgs[0].GetHeader());
           }

           [Test]
           public void getTime()
           {
               List<byte> expectedList = new List<byte>();
               var byteArray = new byte[] { 28, 0, 0, 0, 232, 25, 20, 65 };
               expectedList.AddRange(byteArray.ToList());

               Assert.AreEqual(expectedList, SmallBinary.BinaryMsgs[0].GetTimeHeader());
           }

           [Test]
           public void getGPSWeekNumberHeader()
           {
               List<byte> expectedList = new List<byte>();
               var byteArray = new byte[] {147, 8};
               expectedList.AddRange(byteArray.ToList());

               Assert.AreEqual(expectedList, SmallBinary.BinaryMsgs[0].GetGPSWeekNumberHeader());
           }
           [Test]
           public void getSpare1()
           {
               List<byte> expectedList = new List<byte>();
               var byteArray = new byte[] {0, 0};
               expectedList.AddRange(byteArray.ToList());

               Assert.AreEqual(expectedList, SmallBinary.BinaryMsgs[0].GetSpare1());
           }

           [Test] 
           public void getPageCount()
           {
               List<byte> expectedList = new List<byte>();
               var byteArray = new byte[] {0, 0, 2, 0 };
               expectedList.AddRange(byteArray.ToList());

               Assert.AreEqual(expectedList, SmallBinary.BinaryMsgs[0].GetPageCountHeader());
           }

           [Test]
           public void SmallAvgStdTest()
           {
               Assert.AreEqual(0.0, ParseAnalysis.GetAvgDifference());
               Assert.AreEqual(0.0, ParseAnalysis.StdDeviation());
           }


           [Test]
           public void LargeAvgStdTest()
           {
                 Console.WriteLine("Max Difference: " + ParseAnalysisNew.maxDifference, ConsoleColor.Red);
                 Console.WriteLine("Max Difference at location: " + ParseAnalysisNew.MaxDifferenceLocation + " - " + ParseAnalysisLarge.Bin16Msgs[ParseAnalysisLarge.MaxDifferenceLocation].DisplayTimeStamp(), ConsoleColor.Red);
                 Console.WriteLine("Avg Difference: " + ParseAnalysisNew.GetAvgDifference(), ConsoleColor.Green);
                 Console.WriteLine("Std. Deviation " + ParseAnalysisNew.StdDeviation(), ConsoleColor.Blue);
                 Console.WriteLine("Last Binary Message was at " + ParseAnalysisNew.GetLastBinMessage());
                 ParseAnalysisNew.PrintBigDifferences();
           }
     */

        [Test]
      public void ReadBluetoothDump()
      {
          Console.WriteLine("Max Difference: " + BluetoothDumpAnalysis.maxDifference, ConsoleColor.Red);
          Console.WriteLine("Max Difference at location: " + BluetoothDumpAnalysis.MaxDifferenceLocation + " - " + BluetoothDumpAnalysis.Bin16Msgs[BluetoothDumpAnalysis.MaxDifferenceLocation].DisplayTimeStamp(), ConsoleColor.Red);
          Console.WriteLine("Avg Difference: " + BluetoothDumpAnalysis.GetAvgDifference(), ConsoleColor.Green);
          Console.WriteLine("Std. Deviation " + BluetoothDumpAnalysis.StdDeviation(), ConsoleColor.Blue);
          Console.WriteLine("Last Binary Message was at " + BluetoothDumpAnalysis.GetLastBinMessage());
          BluetoothDumpAnalysis.PrintBigDifferences();
      }
        [Test]
      public void ReadSerialDump()
      {
          Console.WriteLine("Max Difference: " + SerialDumpAnalysis.maxDifference, ConsoleColor.Red);
          Console.WriteLine("Max Difference at location: " + SerialDumpAnalysis.MaxDifferenceLocation + " - " + SerialDumpAnalysis.Bin16Msgs[SerialDumpAnalysis.MaxDifferenceLocation].DisplayTimeStamp(), ConsoleColor.Red);
          Console.WriteLine("Avg Difference: " + SerialDumpAnalysis.GetAvgDifference(), ConsoleColor.Green);
          Console.WriteLine("Std. Deviation " + SerialDumpAnalysis.StdDeviation(), ConsoleColor.Blue);
          Console.WriteLine("Last Binary Message was at " + SerialDumpAnalysis.GetLastBinMessage());
          SerialDumpAnalysis.PrintBigDifferences();
      }

      [Test]
      public void PrintAllJSONTimestamps()
      {
          for (int i = 0; i < JsonData.Count; i++)
          {
              Console.WriteLine($"{JsonData[i].Item2.DisplayTimeStamp()}");
          }
      }


      [Test]
      public void PrintAllSerialTimestamps()
      {
          for (int i = 0; i < SerialDump.BinaryMsgs.Count; i++)
          {
              Console.WriteLine($"{SerialDump.BinaryMsgs[i].DisplayTimeStamp()}");
          }
      }



        [Test]
      public void CompareGPSTimestamps()
      {
          bool foundMatch = false;

            var longerMessage = Math.Max(JsonData.Count, SerialDump.BinaryMsgs.Count) - 1;

          int counter1 = 0;
          int counter2 = 0;
          int timesItMissedData = 0;

          while (counter1 < longerMessage && counter1 < SerialDump.BinaryMsgs.Count - 1 && counter2 < longerMessage && counter2 < JsonData.Count - 1)
          {

              DateTime JsonDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, JsonData[counter2].Item2.Hour,
                  JsonData[counter2].Item2.Minute, JsonData[counter2].Item2.Second);

              DateTime SerialDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SerialDump.BinaryMsgs[counter1].Hour,
                  SerialDump.BinaryMsgs[counter1].Minute, SerialDump.BinaryMsgs[counter1].Second);

                TimeSpan timespan = SerialDateTime - JsonDateTime;



                if (timespan == TimeSpan.Zero)
                {
                    bool added = false;

                    var missingJSONData = JsonData[counter2].Item2._rawByteData
                        .Except(SerialDump.BinaryMsgs[counter1]._rawByteData).ToList();
                    var missingSerialData = SerialDump.BinaryMsgs[counter1]._rawByteData
                        .Except(JsonData[counter2].Item2._rawByteData).ToList();

                    if (missingJSONData.Count > 0)
                    {
                        if (!added)
                        {
                            timesItMissedData++;
                            added = true;
                        }
/*                            Console.WriteLine("Hmm.. Timestamp matched, but they weren't the same data (Bluetooth Missing data)");
*/                    }

                    if (missingSerialData.Count > 0)
                    {
                        if (!added)
                        {
                            timesItMissedData++;
                        }
/*                        Console.WriteLine("Hmm.. Timestamp matched, but they weren't the same data (Serial Missing data)");
*/                    }


                    // If they matched, then you have to check the next of both. If the next also matches, you advance that counter. Greedy, so allow
                    // Counter 1 to progress faster than counter 2.
                    // Must do this type of crawling in order to account for rounding. 

                    DateTime JsonDateTimeNext = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, JsonData[counter2 + 1].Item2.Hour,
                        JsonData[counter2 + 1].Item2.Minute, JsonData[counter2 + 1].Item2.Second);

                    DateTime SerialDateTimeNext = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, SerialDump.BinaryMsgs[counter1 + 1].Hour,
                        SerialDump.BinaryMsgs[counter1 + 1].Minute, SerialDump.BinaryMsgs[counter1 + 1].Second);

                    TimeSpan timespanNext1 = SerialDateTimeNext - JsonDateTime;
                    TimeSpan timespanNext2 = SerialDateTime - JsonDateTimeNext;

                    if (timespanNext1 == TimeSpan.Zero)
                    {
                        counter1++;
                    }
                    else if (timespanNext2 ==TimeSpan.Zero)
                    {
                        counter2++;
                    }
                    else
                    {
                        counter1++;
                        counter2++;
                    }


                    foundMatch = true;
                }


                else if (timespan < -TimeSpan.Zero)
                {
                    if (foundMatch)
                    {
                        Console.WriteLine($"Missing Data detected at {JsonDateTime} from JSON data with {timespan} seconds between");
                        foundMatch = false;
                    }
                    counter1++;
                }
                else
                {
                    if (foundMatch)
                    {
                        Console.WriteLine($"Missing Data detected at {SerialDateTime} from Serial Data with {timespan} seconds between");
                        foundMatch = false;
                    }

                    counter2++;
                }
          }
            Console.WriteLine($"{timesItMissedData} times data didn't match, {JsonData.Count} total bluetooth data points, {SerialDump.BinaryMsgs.Count} total serial data points");

      }




        /*    [Test]
            public void convertTime()
            {
                  var test = new List<string> { "88", "00", "00", "00", "60", "2A", "1E", "41" };

                  SmallBinary.GetTimePassedIn(test);
            }*/




        /*      [Test]
              public void GetBinaryMessage()
              {
        *//*          var pb = new ParseBinary.ParseBinary();
        *//*          var portedParse = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\TestBin.bin");

                    portedParse.ReadFile("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\TestBin.bin");


              }*/

        /*      [Test]
              public void LocateBin16Message()
              {
                  var pb = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\BTDumpXcode 2.bin");


              }*/



    }
}
