using System;
using System.Text;

namespace Helper
{
    class BinaryConverter
    {
        /* BTYE ARRAY AND HEX STRING CONVERTER */
        //https://stackoverflow.com/questions/311165
        public static string ByteToHexBitFiddle(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }

        //Byte Manipulation 2 (via CodesInChaos)
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        /* BYTE ARRAY AND STRING CONVERTER */
        public static string ByteToString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes).Replace("\0", string.Empty); //Regex.Replace(Encoding.ASCII.GetString(bytes), @"[^\w\.@-]", "", RegexOptions.None);
        }
    }
}
