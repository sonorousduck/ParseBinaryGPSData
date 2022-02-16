/*using System;
using System.Dynamic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace ParseBinary
{
   public class Bin16Msg
   {
      public const int MaxObservations = 192; //bytes
      public const int MaxChannels = 64; //bytes
      public const int MaxBin16MsgLength = 280; //bytes

      private ulong _msgHeadBytes; //(8-bytes)

      //Binary Message Header (8-Bytes)
      public string StartOfHead { get; private set; }
      public short BlockId { get; private set; }
      public int DataLength { get; private set; }

      //message structure
      public double TimeInSeconds { get; private set; } //(8-Bytes)

      public short GpsWeekNumber { get; private set; } //(8-Bytes)

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

      public void Parse(byte[] dataBytes)
      {
         //TODO:
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
      public ParseBinary()
      {
         
      }
      private string _binaryFilePath;
      private byte[] _binaryFileData;

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

      public byte[] ChunkData(string filePath)
      {



          return new byte[512];
      }


      public static byte[] ReadFile(string filePath)
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
               fileStream.Read(buffer,0,1);
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

            //read the size of the msg header
            const int maxMsgHeaderSize = 8;
            byte[] msgHeader = new byte[maxMsgHeaderSize];
            //reset the read position back one to pickup the found $
            fileStream.Seek(position-1, SeekOrigin.Begin);
            var readBytes = fileStream.Read(msgHeader, 0, maxMsgHeaderSize);
            position += maxMsgHeaderSize;

            var msgId = EndianAwareBitConverter.ToInt16(msgHeader, 4, Endianness.Little);
            var msgLength = EndianAwareBitConverter.ToInt16(msgHeader, 6, Endianness.Little);

            var ReadBytes = fileStream.Read(buffer, 0, msgLength);
            Console.WriteLine(ReadBytes);
            //TODO: Store packet in Bin16MessageStructure
            //TODO: loop until file is read in


         }
         finally
         {
            fileStream.Close();
         }
         return buffer;
      }
   }
}
*/