using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cheese
{
    public class Crc32
    {
        /// <summary>
        /// CRC32效驗
        /// </summary>
        /// <param name="byteData">要進行計算的位元組陣列</param>
        /// <returns>計算後的陣列</returns>
        public static UInt32[] crc32_table =
        {
                0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f,
                0xe963a535, 0x9e6495a3,    0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988,
                0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91, 0x1db71064, 0x6ab020f2,
                0xf3b97148, 0x84be41de,     0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
                0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec,         0x14015c4f, 0x63066cd9,
                0xfa0f3d63, 0x8d080df5,      0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172,
                0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,        0x35b5a8fa, 0x42b2986c,
                0xdbbbc9d6, 0xacbcf940,     0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
                0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423,
                0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
                0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,         0x76dc4190, 0x01db7106,
                0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
                0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d,
                0x91646c97, 0xe6635c01, 0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e,
                0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950,
                0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
                0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7,
                0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0,
                0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9, 0x5005713c, 0x270241aa,
                0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
                0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81,
                0xb7bd5c3b, 0xc0ba6cad, 0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a,
                0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683, 0xe3630b12, 0x94643b84,
                0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
                0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb,
                0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc,
                0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5, 0xd6d6a3e8, 0xa1d1937e,
                0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
                0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55,
                0x316e8eef, 0x4669be79, 0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
                0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28,
                0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
                0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f,
                0x72076785, 0x05005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38,
                0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 0x86d3d2d4, 0xf1d4e242,
                0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
                0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69,
                0x616bffd3, 0x166ccf45, 0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2,
                0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc,
                0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
                0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693,
                0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94,
                0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
        };
        //------------------------------------------------------------------------//
        public static UInt32 CRC32_Calculation(byte[] data, UInt32 size)
        {
            UInt32 i, crc;
            crc = 0xffffffff;          // initial value

            for (i = 0; i < size; i++)
            {
                crc = ((crc >> 8) & 0x00FFFFFF) ^ crc32_table[(int)(crc ^ data[i]) & 0xff];
            }
            return ~crc;
        }

        public static string I2C_CRC32(string orginal_data)
        {
            string[] orginal_array = orginal_data.Split(' ');
            byte[] orginal_bytes = new byte[orginal_array.Count()];
            int orginal_index = 0;
            foreach (string hex in orginal_array)
            {
                // Convert the number expressed in base-16 to an integer.
                byte number = Convert.ToByte(Convert.ToInt32(hex, 16));
                // Get the character corresponding to the integral value.
                orginal_bytes[orginal_index++] = number;
            }
            byte[] crc32_bytes = System.BitConverter.GetBytes(CRC32_Calculation(orginal_bytes, (uint)orginal_bytes.Length));
            Array.Reverse(crc32_bytes);
            string crc32_data = BitConverter.ToString(crc32_bytes).Replace("-", " ");
            return crc32_data;
        }
    }

    public class Crc16
    {
        private static ushort[] CrcTable = {
        0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
        0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
        0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
        0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
        0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
        0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
        0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
        0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
        0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
        0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
        0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
        0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
        0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
        0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
        0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
        0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
        0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
        0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
        0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
        0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
        0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
        0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
        0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
        0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
        0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
        0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
        0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
        0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
        0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
        0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
        0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
        0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040 };

        public static UInt16 CRC16_Modbus(byte[] data)
        {
            ushort crc = 0xFFFF;

            foreach (byte datum in data)
            {
                crc = (ushort)((crc >> 8) ^ CrcTable[(crc ^ datum) & 0xFF]);
            }

            return crc;
        }

        public static string PID_CRC16(string orginal_data)
        {
            string[] hexValuesSplit = orginal_data.Split(' ');
            byte[] bytes = new byte[hexValuesSplit.Count()];
            int hex_number = 0;
            try
            {
                foreach (string hex in hexValuesSplit)          //改為Byte陣列
                {
                    // Convert the number expressed in base-16 to an integer.
                    byte number = Convert.ToByte(Convert.ToInt32(hex, 16));
                    // Get the character corresponding to the integral value.
                    bytes[hex_number++] = number;
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Please check HEX command format.", "Format error");
            }

            ushort crc = CRC16_Modbus(bytes);       //計算CRC
            string crcHex = ((int)crc).ToString("X2").PadLeft(4, '0');
            string crcLow = crcHex.Substring(0, 2);     //低位數
            string crcHigh = crcHex.Substring(2);       //高位數
            string CRC_calu = " " + crcHigh + " " + crcLow;   //原始資料加入CRC
            return CRC_calu;
        }
    }

    
    public static class HexConverter
    {
        public static byte[] HexToByte(this string hexString)
        {
            //運算後的位元組長度:16進位數字字串長/2
            byte[] byteOUT = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i = i + 2)
            {
                //每2位16進位數字轉換為一個10進位整數
                byteOUT[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteOUT;
        }

        public static string BToHex(this byte[] Bdata)
        {
            return BitConverter.ToString(Bdata).Replace("-", "");
        }

        //取出字串右邊開始的指定數目字元
        public static string Right(this string str, int len)
        {
            return str.Substring(str.Length - len, len);
        }
        //取出字串右邊開始的指定數目字元(跳過幾個字元)
        public static string Right(this string str, int len, int skiplen)
        {
            return str.Substring(str.Length - len - skiplen, len);
        }

        public static byte[] StrToByte(this string hexString)
        {
            string[] orginal_array = hexString.Split(' ');
            byte[] orginal_bytes = new byte[orginal_array.Count()];
            int orginal_index = 0;
            try
            {
                foreach (string hex in orginal_array)
                {
                    // Convert the number expressed in base-16 to an integer.
                    byte number = Convert.ToByte(Convert.ToInt32(hex, 16));
                    // Get the character corresponding to the integral value.
                    orginal_bytes[orginal_index++] = number;
                }
            }
            catch (Exception ex)
            {
                if (ex is OverflowException || ex is FormatException)
                {
                    MessageBox.Show("Please check your schedule is in hexadecimal format.", "Format error");
                }
            }
            return orginal_bytes;
        }
    }

    public static class Converter
    {
        public static int timeToms(this string waitString)
        {
            string[] orginal_array = waitString.Split(':');
            int[] orginal_int = new int[waitString.Count()];
            int orginal_index = 0;

            foreach (string wait in orginal_array)
            {
                // Convert the number expressed in base-16 to an integer.
                int number = Convert.ToInt32(wait);
                // Get the character corresponding to the integral value.
                orginal_int[orginal_index++] = number;
            }

            orginal_int[0] = orginal_int[0] * 3600000;
            orginal_int[1] = orginal_int[1] * 60000;

            int sum_int = orginal_int[0] + orginal_int[1];
            return sum_int;
        }
    }

    public static class ChecksumCalculation
    {
        public static byte XOR(byte bHEX1, byte bHEX2)
        {
            byte bHEX_OUT = new byte();
            bHEX_OUT = (byte)(bHEX1 ^ bHEX2);
            return bHEX_OUT;
        }

        public static byte[] OR(byte[] bHEX1, byte[] bHEX2)
        {
            byte[] bHEX_OUT = new byte[bHEX1.Length];
            for (int i = 0; i < bHEX1.Length; i++)
            {
                bHEX_OUT[i] = (byte)(bHEX1[i] | bHEX2[i]);
            }
            return bHEX_OUT;
        }

        public static byte[] AND(byte[] bHEX1, byte[] bHEX2)
        {
            byte[] bHEX_OUT = new byte[bHEX1.Length];
            for (int i = 0; i < bHEX1.Length; i++)
            {
                bHEX_OUT[i] = (byte)(bHEX1[i] & bHEX2[i]);
            }
            return bHEX_OUT;
        }

        public static byte XOR_Byte(byte[] bHEX1, int length)
        {
            byte bHEX_OUT = bHEX1[0];
            for (int i = 1; i < length; i++)
            {
                bHEX_OUT = (byte)(bHEX_OUT ^ bHEX1[i]);
            }
            return bHEX_OUT;
        }

        public static byte XOR_List(List<byte> bHEX1, int length)
        {
            byte bHEX_OUT = bHEX1[0];
            for (int i = 1; i < length; i++)
            {
                bHEX_OUT = (byte)(bHEX_OUT ^ bHEX1[i]);
            }
            return bHEX_OUT;
        }

        public static string DEV_XOR8(string original_data)
        {
            string[] hexValuesSplit = original_data.Split(' ');
            byte[] bytes = new byte[hexValuesSplit.Count()];
            byte XOR_value = new byte();
            int hex_number = 0;
            try
            {
                foreach (string hex in hexValuesSplit)          //改為Byte陣列
                {
                    // Convert the number expressed in base-16 to an integer.
                    byte number = Convert.ToByte(Convert.ToInt32(hex, 16));
                    // Get the character corresponding to the integral value.
                    bytes[hex_number++] = number;
                    if (hex_number > 0)
                        XOR_value = XOR(bytes[hex_number - 1], XOR_value);
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("Please check HEX command format.", "Format error");
            }

            string XOR_Hex = ((int)XOR_value).ToString("X2").PadLeft(2, '0');
            string XOR_calu = " " + XOR_Hex;   //原始資料加入XOR
            return XOR_calu;
        }

        public static byte Mod256_Byte(List<byte> packetList)
        {
            byte toBeSummed = 0, chksumByte_calculation = 0;
            int sumValue = 0;
            if (packetList.Count > 4)
            {
                List<byte> tempList = packetList.GetRange(2, packetList.Count - 4);
                foreach (byte partByte in tempList)
                {
                    toBeSummed += partByte;
                }
                sumValue = (int)toBeSummed;
                chksumByte_calculation = (byte)(sumValue % 256);
            }
            

            return chksumByte_calculation;
        }

        public static byte Mod256_Byte(byte[] packetBytes)
        {
            byte chksumByte_calculation = 0;
            int sumValue = 0, pktLength = packetBytes.Length;
            if (pktLength > 4)
            {
                for (int i = 2; i <= pktLength - 3; i++)
                {
                    sumValue += (int)packetBytes[i];
                }

                chksumByte_calculation = (byte)(sumValue % 256);
            }


            return chksumByte_calculation;
        }

        public static bool Mod256(byte packetLen, byte chksumByte_packet, List<byte> partList, ref List<byte> fullList)
        {
            bool res = false;
            byte chksumByte_calculation = 0;
            int toBeSummed = 0;
            foreach (byte partByte in partList)
            {
                toBeSummed += partByte;
            }
            //sumValue = (int)toBeSummed;
            chksumByte_calculation = (byte)(toBeSummed % 256);

            if (chksumByte_calculation == chksumByte_packet)
            {
                res = true;
                /*
                fullList.Add(0x42);
                fullList.Add(packetLen);
                fullList.AddRange(partList);
                fullList.Add(chksumByte_calculation);
                fullList.Add(0x51);
                */
            }
            else
                res = false;

            fullList.Add(0x42);
            fullList.Add(packetLen);
            fullList.AddRange(partList);
            fullList.Add(chksumByte_calculation);
            fullList.Add(0x51);

            return res;
        }

    }
}
