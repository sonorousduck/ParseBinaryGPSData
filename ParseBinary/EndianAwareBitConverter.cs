using System;
using System.Diagnostics.CodeAnalysis;

namespace ParseBinary
{
  
   /// <summary>
   ///    Provides values that correspond to endianness.
   /// </summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Endianness")]
   public enum Endianness
   {
      /// <summary>
      ///    Represents big endian.
      /// </summary>
      Big,

      /// <summary>
      ///    Represent little endian.
      /// </summary>
      Little
   }

   /// <summary>
   ///    Provides a bit conveter that is aware of valueEndianness and corrects based on the system's valueEndianness.
   /// </summary>
   public static class EndianAwareBitConverter
   {
      /// <summary>
      ///    Gets the current valueEndianness.
      /// </summary>
      public static readonly Endianness CurrentEndianness = BitConverter.IsLittleEndian
                                                                ? Endianness.Little
                                                                : Endianness.Big;

      /// <summary>
      ///    Returns the specified single-precision floating point value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 4. </returns>
      public static byte[] GetBytes(float value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified 16-bit unsigned integer value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 2. </returns>
      public static byte[] GetBytes(ushort value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified Boolean value as an array of bytes.
      /// </summary>
      /// <param name="value"> if set to <c>true</c> [value]. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 1. </returns>
      public static byte[] GetBytes(bool value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified Unicode character value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 2. </returns>
      public static byte[] GetBytes(char value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified double-precision floating point value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 8. </returns>
      public static byte[] GetBytes(double value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      ///<summary>
      ///   Returns the specified 16-bit signed integer value as an array of bytes.
      ///</summary>
      ///<param name="value"> The value. </param>
      ///<param name="valueEndianness"> The value endianness. </param>
      ///<returns> An array of bytes with length 2. </returns>
      public static byte[] GetBytes(short value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified 32-bit signed integer value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 4. </returns>
      public static byte[] GetBytes(int value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified 64-bit signed integer value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 8. </returns>
      public static byte[] GetBytes(long value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified 32-bit unsigned integer value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 4. </returns>
      public static byte[] GetBytes(uint value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

      /// <summary>
      ///    Returns the specified 64-bit unsigned integer value as an array of bytes.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> An array of bytes with length 8. </returns>
      public static byte[] GetBytes(ulong value, Endianness valueEndianness)
      {
         byte[] bytes = BitConverter.GetBytes(value);

         if (CurrentEndianness != valueEndianness)
         {
            Reverse(bytes);
         }

         return bytes;
      }

     

      /// <summary>
      ///    Returns a Boolean value converted from one byte at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> <c>true</c> if the byte at startIndex in value is nonzero; otherwise, <c>false</c> . </returns>
      public static bool ToBoolean(byte[] value, int startIndex, Endianness valueEndianness)
      {
         return BitConverter.ToBoolean(value, startIndex);
      }

      /// <summary>
      ///    Returns a Unicode character converted from two bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A character formed by two bytes beginning at startIndex. </returns>
      public static char ToChar(byte[] value, int startIndex, Endianness valueEndianness)
      {
         return BitConverter.ToChar(value, startIndex);
      }

      /// <summary>
      /// To the char.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <param name="startIndex">The start index.</param>
      /// <param name="length">The length.</param>
      /// <param name="valueEndianness">The value endianness.</param>
      /// <returns>a character array formed from a single bytes of specified length.</returns>
      /// <exception cref="System.NotImplementedException">Not currently implemented for Big Endian, you have to do it.</exception>
      public static char[] ToChar(byte[] value, int startIndex, int length, Endianness valueEndianness)
      {
         if (valueEndianness != Endianness.Little)
         {
            throw new NotImplementedException("Not currently implemented for Big Endian, you have to do it.");
         }

         char[] chars = new char[length];
         for (int count = 0; count < length; count++)
         {
            char ch = (char)value[startIndex + count];
            chars[count] = ch;
         }

         return chars;
      }
      /// <summary>
      ///    Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A double precision floating point number formed by eight bytes beginning at startIndex. </returns>
      public static double ToDouble(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 8);
            return BitConverter.ToDouble(reversedChunk, 0);
         }

         return BitConverter.ToDouble(value, startIndex);
      }

      /// <summary>
      ///    Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The valueEndianness. </param>
      /// <returns> A 16-bit signed integer formed by two bytes beginning at startIndex. </returns>
      public static short ToInt16(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 2);
            return BitConverter.ToInt16(reversedChunk, 0);
         }

         return BitConverter.ToInt16(value, startIndex);
      }

      /// <summary>
      ///    Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A 32-bit signed integer formed by four bytes beginning at startIndex. </returns>
      public static int ToInt32(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 4);
            return BitConverter.ToInt32(reversedChunk, 0);
         }

         return BitConverter.ToInt32(value, startIndex);
      }

      /// <summary>
      ///    Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A 64-bit signed integer formed by eight bytes beginning at startIndex. </returns>
      public static long ToInt64(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 8);
            return BitConverter.ToInt64(reversedChunk, 0);
         }

         return BitConverter.ToInt64(value, startIndex);
      }

      /// <summary>
      ///    Toes the single.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> </returns>
      public static float ToSingle(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 4);
            return BitConverter.ToSingle(reversedChunk, 0);
         }

         return BitConverter.ToSingle(value, startIndex);
      }

      /// <summary>
      ///    Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of value; for example, "7F-2C-4A-00" </returns>
      public static string ToString(byte[] value, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedCopy(value);
            return BitConverter.ToString(reversedChunk, 0);
         }

         return BitConverter.ToString(value);
      }

      /// <summary>
      ///    Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of value; for example, "7F-2C-4A-00" </returns>
      public static string ToString(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, value.Length - startIndex);
            return BitConverter.ToString(reversedChunk, 0);
         }

         return BitConverter.ToString(value, startIndex);
      }

      /// <summary>
      ///    Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="length"> The length. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of value; for example, "7F-2C-4A-00" </returns>
      public static string ToString(byte[] value, int startIndex, int length, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, length);
            return BitConverter.ToString(reversedChunk, 0);
         }

         return BitConverter.ToString(value, startIndex,length);
      }

      /// <summary>
      ///    Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The valueEndianness. </param>
      /// <returns> A 16-bit unsigned integer converted from two bytes at a specified position in a byte array. </returns>
      public static ushort ToUInt16(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 2);
            return BitConverter.ToUInt16(reversedChunk, 0);
         }

         return BitConverter.ToUInt16(value, startIndex);
      }

      /// <summary>
      ///    Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A 32-bit unsigned integer formed by four bytes beginning at startIndex. </returns>
      public static uint ToUInt32(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 4);
            return BitConverter.ToUInt32(reversedChunk, 0);
         }

         return BitConverter.ToUInt32(value, startIndex);
      }

      /// <summary>
      ///    Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <param name="startIndex"> The start index. </param>
      /// <param name="valueEndianness"> The value endianness. </param>
      /// <returns> A 64-bit unsigned integer formed by the eight bytes beginning at startIndex. </returns>
      public static ulong ToUInt64(byte[] value, int startIndex, Endianness valueEndianness)
      {
         if (valueEndianness != CurrentEndianness)
         {
            byte[] reversedChunk = GetReversedChunk(value, startIndex, 8);
            return BitConverter.ToUInt64(reversedChunk, 0);
         }

         return BitConverter.ToUInt64(value, startIndex);
      }

      /// <summary>
      ///    Gets the reverse copy.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <returns> </returns>
      private static void Reverse(byte[] value)
      {
         int length = value.Length;

         for (int index = 0; index < length / 2; index++)
         {
            byte tmp = value[index];
            value[index] = value[length - index - 1];
            value[length - index - 1] = tmp;
         }
      }

      private static byte[] GetReversedCopy(byte[] value)
      {
         int length = value.Length;
         byte[] copy = new byte[length];

         for (int index = 0; index < length; index++)
         {
            value[index] = value[length - index - 1];
         }

         return copy;
      }

      private static byte[] GetReversedChunk(byte[] value, int startIndex, int size)
      {
         if (startIndex + size > value.Length)
         {
            throw new ArgumentException("The value array is not big enough for the specified chunk size.");
         }

         byte[] copy = new byte[size];

         for (int sourceIndex = startIndex + size - 1, destIndex = 0; destIndex < size; sourceIndex--, destIndex++)
         {
            copy[destIndex] = value[sourceIndex];
         }

         return copy;
      }
   }
}
