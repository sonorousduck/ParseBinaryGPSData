using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;

namespace ParseBinary
{
    public class Bin16Msg
    {
        public const int MaxObservations = 192; //bytes
        public const int MaxChannels = 64; //bytes
        public const int MaxBin16MsgLength = 280; //bytes
        public const int SecondsInDay = 86400; // Seconds in a day
        public const int SecondsInHour = 3600;
        public const int SecondsInMinute = 60;

        private int _msgHeadBytes; //(8-bytes)

        //Binary Message Header (8-Bytes)
        public string StartOfHead { get; private set; }
        public short BlockId { get; private set; }
        public int DataLength { get; private set; }

        //message structure
        public double TimeInSeconds { get; private set; } //(8-Bytes)

        public short GpsWeekNumber { get; private set; } //(8-Bytes)
        
        public ulong PageCount { get; private set; }


        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }


        public short NumberOfPages { get; private set; } //bits 16-21
        public short PageNumber { get; private set; } //bits 22-27

        private ulong _allSignalsIncluded_01;
        public GPSSignalsIncluded GpsSignals { get; private set; }
        public GloSignalsIncluded GloSignals { get; private set; }
        public GalSignalsIncluded GalSignals { get; private set; }
        public BdsSignalsIncluded BdsSignals { get; private set; }

        public ulong _allSignalsIncluded_02;

        public QzsSignalsIncluded QzsSignals { get; private set; }

        public short[] Observations { get; private set; }
        public short[] CodeMsbPrn { get; private set; }

        public short CheckSum { get; private set; }
        public List<char> RawData { get; set; }
        public List<byte> _rawByteData { get; private set; }

        public Bin16Msg(List<char> msg, List<byte> byteMsg)
        {
            _msgHeadBytes = 8; //8 bytes for message head


            RawData = msg;
            _rawByteData = byteMsg.ToList();

            Parse();
            ConvertTimeToTimestamp();
        }

        public Bin16Msg(List<byte> byteMsg)
        {
            _msgHeadBytes = 8; //8 bytes for message head

            _rawByteData = byteMsg.ToList();

            Parse();
            ConvertTimeToTimestamp();
        }




        public List<byte> GetHeader()
        {
            return _rawByteData.GetRange(0, _msgHeadBytes);
        }

        public List<byte> GetTimeHeader()
        {
            // 8 bytes (8-16)
            return _rawByteData.GetRange(8, 8);
        }

        public List<byte> GetGPSWeekNumberHeader()
        {
            // 2 bytes (16-18)
            return _rawByteData.GetRange(16, 2);
        }

        public List<byte> GetSpare1()
        {
            // 2 bytes (18-20)
            return _rawByteData.GetRange(18, 2);
        }

        public List<byte> GetPageCountHeader()
        {
            // 4 bytes (20-24)
            return _rawByteData.GetRange(20, 4);
        }

        public short GetGPSWeekNumber()
        {
            var gpsHeader = GetGPSWeekNumberHeader();
            var result = EndianAwareBitConverter.ToInt16(gpsHeader.ToArray(), 0, Endianness.Little);

            return result;
        }

        public ulong GetPageCount()
        {
            // Seems like a strange, large number for the PageCount/Page Information,
            // but it could be counting characters, and not pages, and I have no other way to verify this..
            var pageCount = GetPageCountHeader();
            var result = EndianAwareBitConverter.ToInt32(pageCount.ToArray(), 0, Endianness.Little);


            return (ulong)result;


        }


        public double GetTime()
        {
            // Fairly certain this is returning the time since the GPS week, not since 1970.

            var timeHeader = GetTimeHeader();
            var result = EndianAwareBitConverter.ToDouble(timeHeader.ToArray(), 0, Endianness.Little);


            return Convert.ToDouble(result);
        }

        


        public void ConvertTimeToTimestamp()
        {
            var daySeconds = (this.TimeInSeconds / SecondsInDay) - Math.Floor(this.TimeInSeconds / SecondsInDay);
            var hour = daySeconds * 24;
            this.Hour = ((int) Math.Floor(hour)) - 7; // To account for UTC -> MNT time, since that is what EZSurv does
            var minute = (hour - Math.Floor(hour)) * 60; // Convert Hours to minutes
            this.Minute = (int) Math.Floor(minute);
            var second = (minute - Math.Floor(minute)) * 60; // Convert minutes to seconds
            this.Second = (int) Math.Floor(second);
        }


        public string DisplayTimeStamp()
        {
            return "" + this.Hour.ToString("##00") + ":" + this.Minute.ToString("##00") + ":" + this.Second.ToString("##00");
        }

        private void Parse()
        {
            this.GpsWeekNumber = GetGPSWeekNumber();
            this.TimeInSeconds = GetTime();
            this.PageCount = GetPageCount();


        }

        [Flags]
        public enum GPSSignalsIncluded : byte
        {
            GPS_L1CA = 1,  //1_bit0
            GPS_L2P = 2,   //2_bit1
            GPS_LC2 = 4,   //3_bit2
            GPS_L5 = 8,    //4_bit3
            Spare1_1 = 16, //5_bit4
            Spare1_2 = 32, //6_bit5
            Spare1_3 = 64, //7_bit6
            Spare1_4 = 128,//8_bit7
        }

        [Flags]
        public enum GloSignalsIncluded : byte
        {

            GLO_G1C_P = 1, //1_bit8
            GLO_G2C_P = 2, //2_bit9
            Spare2_1 = 4, //3_bit10
            Spare2_2 = 8, //4_bit11
            Spare2_3 = 16, //5_bit12
            Spare2_4 = 32, //6_bit13
            Spare2_5 = 64, //7_bit14
            Spare2_6 = 128 //8_bit15
        }

        [Flags]
        public enum GalSignalsIncluded : byte
        {
            GAL_E1BC = 1,  //1_bit16
            GAL_E5A = 2,   //2_bit17
            GAL_E5B = 4,   //3_bit18
            Spare3_1 = 8,  //4_bit19
            Spare3_2 = 16, //5_bit20
            Spare3_3 = 32, //6_bit21
            Spare3_4 = 64, //7_bit22
            Spare3_5 = 128,//8_bit23
        }

        [Flags]
        public enum BdsSignalsIncluded : byte
        {
            BDS_B1 = 1,       //1_bit24
            BDS_B2 = 2,       //2_bit25
            BDS_B3 = 4,       //3_bit26
            BDS_B1BOC = 8,    //4_bit27
            BDS_B2A = 16,     //5_bit28
            BDS_B2B = 32,     //6_bit29
            BDS_B3C = 64,     //7_bit30
            BDS_ACEBOOC = 128,//8_bit31
        }

        [Flags]
        public enum QzsSignalsIncluded : byte
        {
            QZS_L2C = 1,      //1_bit0
            QZS_L5 = 2,       //2_bit1
            QZS_L1C = 4,      //3_bit2
            Spare1_1 = 8,     //4_bit3
            Spare1_2 = 16,    //5_bit4
            Spare1_3 = 32,    //6_bit5
            Spare1_4 = 64,    //7_bit6
            Spare1_5 = 128,   //8_bit7
        }

    }


    public class ParseBinary
    {
        private string _binaryFilePath;
        private byte[] _binaryFileData;
        private int MinDataLength = 16;
        private int _dataLength;
        protected List<byte> Data { get; set; }
        public  List<Bin16Msg> BinaryMsgs { get; set; }
        private List<List<char>> Messages { get; set; }
        public List<List<byte>> ByteMessages { get; set; }


        public double GetTimePassedIn(List<string> timestamp)
        {
            // Fairly certain this is returning the time since the GPS week, not since 1970.

            /*var timeHeader = GetTimeHeader();*/


            var result = EndianAwareBitConverter.ToDouble(Array.ConvertAll(timestamp.ToArray(), byte.Parse), 0, Endianness.Little);


            return Convert.ToDouble(result);
        }

        public ParseBinary()
        {
            ByteMessages = new List<List<byte>>();
            BinaryMsgs = new List<Bin16Msg>();
        }

        public ParseBinary(string fileName)
        {
            ByteMessages = new List<List<byte>>();
            Messages = LocateBin16Messages(fileName);
            BinaryMsgs = new List<Bin16Msg>();
        }
        
        
        [StructLayout(LayoutKind.Explicit)]
        public struct BinaryMessageHeader
        {
            [FieldOffset(0)]
            public ushort ID;     /* ID of message (1,2,99,98,97,96,95,94,93 or 80 ) */

            [FieldOffset(2)]
            public ushort DataLength; /* 52 16,304,68,28,300,128,96,56, or 40 */
        }

        public void ParseData()
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                // If it doesn't have all the info... Don't use it. Sophisticated!
                if (Messages[i].Count > 32)
                {
                    Bin16Msg msg = new Bin16Msg(Messages[i], ByteMessages[i]);
                    BinaryMsgs.Add(msg);
                }


            }
        }




        public bool LoadBinary(string binaryFilePath)
        {
            if (string.IsNullOrEmpty(binaryFilePath))
                return false;

            _binaryFileData = ReadFile(binaryFilePath);
            return _binaryFileData.Length > 0;
        }

        private enum ParseState
        {
            Reset = 0,
            DollarSign,
            B,
            I,
            N,
            Found
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }



        public void ParseBinaryData(string RawBytes)
        {
            ParseState expectedState = ParseState.DollarSign;
            ParseState state = ParseState.Reset;
            List<List<char>> messages = new List<List<char>>();
            List<char> charList = new List<char>();
            List<byte> byteList = new List<byte>();
            bool hasMessages = false;


            RawBytes = RawBytes.Trim(' ', ';', ',', '\"');

            var bytes = StringToByteArray(RawBytes);

      /*      for (int i = 0; i < RawBytes.Length; i+=2)
            {
                ConvertedRawBytes.Add($"{RawBytes[i]}{RawBytes[i+1]}");
            }

            foreach (var byteChunk in ConvertedRawBytes)
            {
*//*                Console.Write($"{Convert.ToByte(byteChunk)}");
*//*                ConvertedBytes.Add((byte) byteChunk);
            }*/


            /*byte[] bytes = Encoding.ASCII.GetBytes(ConvertedRawBytes);*/


            foreach (byte byteLetter in bytes)
            {
                char letter = (char) byteLetter;

                switch (letter)
                {
                    case '$':
                        if (expectedState == ParseState.DollarSign && state == ParseState.Reset)
                        {
                            state = ParseState.DollarSign;
                            expectedState = ParseState.B;
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;

                    case 'B':
                        if (expectedState == ParseState.B && state == ParseState.DollarSign)
                        {
                            state = ParseState.B;
                            expectedState = ParseState.I;
                            charList.Add(letter);
                            byteList.Add(byteLetter);

                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;
                    case 'I':
                        if (expectedState == ParseState.I && state == ParseState.B)
                        {
                            state = ParseState.I;
                            expectedState = ParseState.N;
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;
                    case 'N':
                        if (expectedState == ParseState.N && state == ParseState.I)
                        {
                            state = ParseState.Found;
                            expectedState = ParseState.Reset;
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;


                    default:
                        if (letter == '\r')
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                            charList.Add(letter);
                            byteList.Add(byteLetter);

                            if (hasMessages)
                            {
                                messages.Add(charList.ToList());
                                ByteMessages.Add(byteList.ToList());
                            }

                            
                            charList.Clear();
                            byteList.Clear();
                        }

                        else if (letter == '$')
                        {
                            if (hasMessages)
                            {
                                messages.Add(charList.ToList());
                                ByteMessages.Add(byteList.ToList());
                            }
                            charList.Clear();
                            byteList.Clear();

                            state = ParseState.DollarSign;
                            expectedState = ParseState.B;
                        }

                        else if (state == ParseState.Found)
                        {
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                            hasMessages = true;
                        }

                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                            hasMessages = false;
                        }

                        break;
                }
            }
        }




        public byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[512];            // create buffer
                int count;                            // actual number of bytes read
                int position = 0;
                int data;

                //find the start of the packet $BIN read to Carriage Return (0d) Line Feed (0a)
                //Ignore everything else

                //TODO: need to make this a function; FindBin16MessagePosition

                //Keep reading until $BIN (24 42 49 4e)
                var state = ParseState.Reset;
                var nextExpectedState = ParseState.DollarSign;
                do
                {
                    fileStream.Read(buffer, 0, 1);
                    position++;
                    switch ((char)buffer[0])
                    {
                        case '$':
                            if (state == ParseState.Reset && nextExpectedState == ParseState.DollarSign)
                            {
                                state = ParseState.DollarSign;
                                nextExpectedState = ParseState.B;
                            }
                            else
                            {
                                state = ParseState.Reset;
                                nextExpectedState = ParseState.DollarSign;
                            }
                            break;
                        case 'B':
                            if (state == ParseState.DollarSign && nextExpectedState == ParseState.B)
                            {
                                state = ParseState.B;
                                nextExpectedState = ParseState.I;
                            }
                            else
                            {
                                state = ParseState.Reset;
                                nextExpectedState = ParseState.DollarSign;
                            }
                            break;
                        case 'I':
                            if (state == ParseState.B && nextExpectedState == ParseState.I)
                            {
                                state = ParseState.I;
                                nextExpectedState = ParseState.N;
                            }
                            else
                            {
                                state = ParseState.Reset;
                                nextExpectedState = ParseState.DollarSign;
                            }
                            break;
                        case 'N':
                            if (state == ParseState.I && nextExpectedState == ParseState.N)
                            {
                                state = ParseState.Found;
                                nextExpectedState = ParseState.Reset;
                                Data = new List<byte>();
                                _dataLength = MinDataLength;
                            }
                            else
                            {
                                state = ParseState.Reset;
                                nextExpectedState = ParseState.DollarSign;
                            }
                            break;
                        default:
                            state = ParseState.Reset;
                            nextExpectedState = ParseState.DollarSign;

                            break;

                    }
                } while (state != ParseState.Found);

                while (Data.Count != _dataLength)
                {
                    Data.Add((byte) fileStream.Read(buffer, 0, 1));
                    position++;

                    if (Data.Count == 4)
                    {
                        BinaryMessageHeader header = new BinaryMessageHeader
                        {
                            ID = BitConverter.ToUInt16(Data.ToArray(), 0),
                            DataLength = BitConverter.ToUInt16(Data.ToArray(), 2)
                        };
                        _dataLength = header.DataLength + 4 + 2;
                        if (_dataLength > 352 + 4 + 2)
                            // maximum is 352, include 4 for ID and data length bytes, and 2 more for checksum
                        {
                            // Loop back through everything (The do while loop again)
                            // ResetState()
                            break;
                        }
                    }

                    

                }
                //read the size of the msg header
                /*const int maxMsgHeaderSize = 8;
                byte[] msgHeader = new byte[maxMsgHeaderSize];
                //reset the read position back one to pickup the found $
                fileStream.Seek(position - 1, SeekOrigin.Begin);
                var readBytes = fileStream.Read(msgHeader, 0, maxMsgHeaderSize);
                position += maxMsgHeaderSize;

                var msgId = EndianAwareBitConverter.ToInt16(msgHeader, 4, Endianness.Little);
                var msgLength = EndianAwareBitConverter.ToInt16(msgHeader, 6, Endianness.Little);

                var ReadBytes = fileStream.Read(buffer, 0, msgLength);
                Console.WriteLine(ReadBytes);*/
                //TODO: Store packet in Bin16MessageStructure
                //TODO: loop until file is read in


            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }



        private List<List<char>> LocateBin16Messages(string fileName)
        {
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), Encoding.Default);
            ParseState expectedState = ParseState.DollarSign;
            ParseState state = ParseState.Reset;
            List<List<char>> messages = new List<List<char>>();
            List<char> charList = new List<char>();
            List<byte> byteList = new List<byte>();



            while (binReader.BaseStream.Position < binReader.BaseStream.Length)
            {
                
                byte byteLetter = binReader.ReadByte();
                char letter = (char) byteLetter;

                switch (letter)
                {
                    case '$':
                        if (expectedState == ParseState.DollarSign && state == ParseState.Reset)
                        {
                            state = ParseState.DollarSign;
                            expectedState = ParseState.B;
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;

                    case 'B':
                        if (expectedState == ParseState.B && state == ParseState.DollarSign)
                        {
                            state = ParseState.B;
                            expectedState = ParseState.I;
                            charList.Add(letter);
                            byteList.Add(byteLetter);

                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;
                    case 'I':
                        if (expectedState == ParseState.I && state == ParseState.B)
                        {
                            state = ParseState.I;
                            expectedState = ParseState.N;
                            charList.Add(letter);
                            byteList.Add(byteLetter);


                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;
                    case 'N':
                        if (expectedState == ParseState.N && state == ParseState.I)
                        {
                            state = ParseState.Found;
                            expectedState = ParseState.Reset;
                            charList.Add(letter);
                            byteList.Add(byteLetter);


                        }
                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }

                        break;


                    default:
                        if (letter == '\r')
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                            charList.Add(letter);
                            byteList.Add(byteLetter);

                            messages.Add(charList.ToList());
                            ByteMessages.Add(byteList.ToList());
                            charList.Clear();
                            byteList.Clear();
                        }

                        else if (state == ParseState.Found)
                        {
                            charList.Add(letter);
                            byteList.Add(byteLetter);
                        }

                        else
                        {
                            state = ParseState.Reset;
                            expectedState = ParseState.DollarSign;
                        }
                        
                        break;
                    }




                /* if (binReader.BaseStream.Position == binReader.BaseStream.Length) break;


                 byte ch = binReader.ReadByte();
                 Console.Write((char)ch);
             }
             Console.Write((char) binReader.ReadByte());
             Console.WriteLine();*/

            }


            return messages;



        }



    }
}
