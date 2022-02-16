using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ParseBinary;


namespace ParseBinary
{

    public class ParseJSON
    {
        public ParseBinary pb;
        List<string> usefulInformation;
        List<(int Index, string Timestamp, string RawData)> timeToRawData;
        List<(string Timestamp, List<byte>)> timestampToData;
        List<(string Timestamp, Bin16Msg)> bin16Timestamp;
        /*            ParseBinary.ParseBinary SecondRun = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FirstUsefulRun\\2ndRun");
        */
        bool lookingForData;
        List<Bin16Msg> bin16Msgs;
        private float AverageTime;
        private double StandardDeviation;



        

        public ParseJSON()
        {
            usefulInformation = new List<string>();
            timeToRawData = new List<(int Index, string Timestamp, string RawData)>();
            timestampToData = new List<(string Timestamp, List<byte>)>();
            bin16Timestamp = new List<(string Timestamp, Bin16Msg)>();
            lookingForData = false;
            bin16Msgs = new List<Bin16Msg>();
            AverageTime = 0f;
            StandardDeviation = 0;


        }


        public List<(string Timestamp, Bin16Msg)> JSONParse(string filename)
        {
            pb = new ParseBinary();

            string[] jsonString = File.ReadAllLines(filename);
            

            foreach (string line in jsonString)
            {
                if (lookingForData && line.Contains("\"System Time Formatted\""))
                {
                    usefulInformation.Add(line.Split(" : ")[1]);
                    lookingForData = false;
                }

                else if (line.Contains("\"Raw Bytes\""))
                {
                    usefulInformation.Add(line.Split(":")[1]);
                    lookingForData = true;
                }
            }


            for (int i = 0; i < usefulInformation.Count; i+=2)
            {
                timeToRawData.Add((i, usefulInformation[i + 1], usefulInformation[i]));
            }

            foreach (var element in timeToRawData)
            {
                int lengthOfByteMessage = pb.ByteMessages.Count;

                pb.ParseBinaryData(element.RawData);
                if (pb.ByteMessages.Count != lengthOfByteMessage)
                {
                    timestampToData.Add((element.Timestamp.Trim('"', ' ', ','), pb.ByteMessages[^1]));
                }
            }

            foreach (var ( timestamp, message) in timestampToData)
            {
                if (message.Count > 4)
                {

                    if (message.Count < 26)
                    {
                        Console.WriteLine($"At {timestamp} was too Short... It only contained {message.Count} bytes.");

                    }
                    else
                    {
                        bin16Msgs.Add(new Bin16Msg(message));
                        bin16Timestamp.Add((timestamp, bin16Msgs[^1]));

                        DateTime GPSMessage = new DateTime(2022, 2, 14, bin16Msgs[^1].Hour, bin16Msgs[^1].Minute,
                            bin16Msgs[^1].Second);

                        string[] splitStrings = timestamp.Split(':');
                        splitStrings[2] = splitStrings[2].Remove(2);

                        DateTime BluetoothTimestamp = new DateTime(2022, 2, 14, int.Parse(splitStrings[0]),
                            int.Parse(splitStrings[1]), int.Parse(splitStrings[2]));


                        TimeSpan timeDifference = GPSMessage - BluetoothTimestamp;

                        AverageTime += timeDifference.Seconds + timeDifference.Minutes * 60 +
                                       timeDifference.Hours * 3600;
                    }
                }
            }



            AverageTime /= bin16Timestamp.Count;

            Console.WriteLine($"Average Time: {AverageTime} seconds");


            // Find the standard Deviation
            foreach (var (timestamp, message) in bin16Timestamp)
            {
                DateTime GPSMessage = new DateTime(2022, 2, 14, message.Hour, message.Minute, message.Second);
                string[] splitStrings = timestamp.Split(':');
                splitStrings[2] = splitStrings[2].Remove(2);

                DateTime BluetoothTimestamp = new DateTime(2022, 2, 14, int.Parse(splitStrings[0]),
                    int.Parse(splitStrings[1]), int.Parse(splitStrings[2]));

                TimeSpan timeDifference = GPSMessage - BluetoothTimestamp;

                StandardDeviation += Math.Pow(timeDifference.Seconds + timeDifference.Minutes * 60f + timeDifference.Hours * 3600f - AverageTime, 2);
            }

            StandardDeviation /= bin16Timestamp.Count;
            StandardDeviation = Math.Sqrt(StandardDeviation);

            Console.WriteLine($"Standard Deviation: {StandardDeviation} seconds");

            foreach (var (timestamp, message) in bin16Timestamp)
            {
                DateTime GPSMessage = new DateTime(2022, 2, 14, message.Hour, message.Minute, message.Second);
                string[] splitStrings = timestamp.Split(':');
                splitStrings[2] = splitStrings[2].Remove(2);

                DateTime BluetoothTimestamp = new DateTime(2022, 2, 14, Int32.Parse(splitStrings[0]),
                    Int32.Parse(splitStrings[1]), Int32.Parse(splitStrings[2]));

                TimeSpan timeDifference = GPSMessage - BluetoothTimestamp;

                if (timeDifference.Seconds + timeDifference.Minutes * 60 + timeDifference.Hours * 3600 > 2 * StandardDeviation + AverageTime)
                {
                    Console.WriteLine($"Timestamp: {timestamp} compared to: {message.DisplayTimeStamp()}. Difference of {timeDifference} seconds");
                }



                /*                SecondRun = new ParseBinary.ParseBinary("C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FirstUsefulRun\\2ndRun");
                                SecondRun.ParseData();*/
            }


            /*foreach (var message in bin16Msgs)
            {
                Console.WriteLine($"{message.DisplayTimeStamp()}");
            }*/


            return bin16Timestamp;

        }


    }





}
