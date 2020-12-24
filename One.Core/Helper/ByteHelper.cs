using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace One.Core.Helper
{
    public class ByteHelper
    {
        /// <summary>
        /// 计算字符串的特征码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetStringMD5(string source)
        {
            //计算字符串的MD5
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = string.Empty;
            for (int i = 0; i < md5Data.Length; i++)
            {
                //返回一个新字符串，该字符串通过在此实例中的字符左侧填充指定的
                //Unicode 字符来达到指定的总长度，从而使这些字符右对齐。
                // string num=12; num.PadLeft(4, '0'); 结果为为 '0012' 看字符串长度是否满足4位,
                //不满足则在字符串左边以"0"补足
                //调用Convert.ToString(整型,进制数) 来转换为想要的进制数
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }

            //使用 PadLeft 和 PadRight 进行轻松地补位
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// Byte数组转为StrData，实现了反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="StrData"></param>
        /// <returns></returns>
        public static object BytesToStruct(byte[] bytes, Type StrData)
        {
            int size = Marshal.SizeOf(StrData);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, StrData);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// StrData转为Byte数组，实现了序列化
        /// </summary>
        /// <param name="StrData"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object StrData)
        {
            int size = Marshal.SizeOf(StrData);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(StrData, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        #region 转换为大端模式

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat">  </param>
        /// <returns>  </returns>
        public static ushort EndianUINT16(ushort Dat)
        {
            ushort Val;
            byte[] Buf = new byte[8];
            Buf[0] = (byte) (Dat >> 0x00);
            Buf[1] = (byte) (Dat >> 0x08);
            Val = (ushort) (((Buf[0] & 0xFF) << 0x08) | ((Buf[1] & 0xFF) << 0x00));
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat">  </param>
        /// <returns>  </returns>
        public static uint EndianUINT32(uint Dat)
        {
            uint Val;
            byte[] Buf = new byte[8];
            Buf[0] = (byte) (Dat >> 0);
            Buf[1] = (byte) (Dat >> 8);
            Buf[2] = (byte) (Dat >> 16);
            Buf[3] = (byte) (Dat >> 24);
            Val = (uint) (((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) |
                          ((Buf[3] & 0xFF) << 0));
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat">  </param>
        /// <returns>  </returns>
        public static ulong EndianUINT64(ulong Dat)
        {
            ulong Val = 0;
            uint Val1 = 0, Val2 = 0;
            byte[] Buf = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                Buf[i] = (byte) (Dat >> i * 8);
            }

            Val1 = (uint) (((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) |
                           ((Buf[3] & 0xFF) << 0));
            Val2 = (uint) (((Buf[4] & 0xFF) << 24) | ((Buf[5] & 0xFF) << 16) | ((Buf[6] & 0xFF) << 8) |
                           ((Buf[7] & 0xFF) << 0));
            Val = (((ulong) Val1 & 0xFFFFFFFF) << 32) | (((ulong) Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat">  </param>
        /// <returns>  </returns>
        public static long EndianINT64(long Dat)
        {
            long Val = 0;
            int Val1 = 0, Val2 = 0;
            byte[] Buf = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                Buf[i] = (byte) (Dat >> i * 8);
            }

            Val1 = ((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) | ((Buf[3] & 0xFF) << 0);
            Val2 = ((Buf[4] & 0xFF) << 24) | ((Buf[5] & 0xFF) << 16) | ((Buf[6] & 0xFF) << 8) | ((Buf[7] & 0xFF) << 0);
            Val = ((Val1 & 0xFFFFFFFF) << 32) | ((Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        #endregion 转换为大端模式

        #region 获取对应类型的大端模式

        public static ushort GetUINT16(byte[] Buf, int Index=0)
        {
            ushort Val;
            Val = (ushort) (((Buf[Index + 0] & 0xFF) << 0x08) | ((Buf[Index + 1] & 0xFF) << 0x00));
            return Val;
        }

        public static uint GetUINT32(byte[] Buf, int Index=0)
        {
            uint Val;
            Val = (uint) (((Buf[Index + 0] & 0xFF) << 24) | ((Buf[Index + 1] & 0xFF) << 16) |
                          ((Buf[Index + 2] & 0xFF) << 8) | ((Buf[Index + 3] & 0xFF) << 0));
            return Val;
        }

        public static ulong GetUINT64(byte[] Buf, int Index=0)
        {
            ulong Val = 0;
            uint Val1 = 0, Val2 = 0;
            Val1 = (uint) (((Buf[Index + 0] & 0xFF) << 24) | ((Buf[Index + 1] & 0xFF) << 16) |
                           ((Buf[Index + 2] & 0xFF) << 8) | ((Buf[Index + 3] & 0xFF) << 0));
            Val2 = (uint) (((Buf[Index + 4] & 0xFF) << 24) | ((Buf[Index + 5] & 0xFF) << 16) |
                           ((Buf[Index + 6] & 0xFF) << 8) | ((Buf[Index + 7] & 0xFF) << 0));
            Val = (((ulong) Val1 & 0xFFFFFFFF) << 32) | (((ulong) Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        #endregion 获取对应类型的大端模式



        #region 校验
        public static ushort GetCRC(byte[] pchMsg, ushort wDataLen)
        {
            byte[] chCRCHTalbe = // CRC 高位字节值表
            {
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                0x00, 0xC1, 0x81, 0x40
            };

            byte[] chCRCLTalbe = // CRC 低位字节值表
            {
                0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
                0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
                0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
                0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
                0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
                0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
                0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
                0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
                0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
                0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
                0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
                0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
                0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
                0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
                0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
                0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
                0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
                0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
                0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
                0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
                0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
                0x41, 0x81, 0x80, 0x40
            };
            byte chCRCHi = 0xFF; // 高CRC字节初始化
            byte chCRCLo = 0xFF; // 低CRC字节初始化
            ushort wIndex = 0; // CRC循环中的索引
            ushort nIndex = 0;
            while (wDataLen > 0)
            {
                wDataLen--;
                // 计算CRC
                wIndex = (ushort)(chCRCLo ^ pchMsg[nIndex]);
                nIndex++;
                chCRCLo = (byte)(chCRCHi ^ chCRCHTalbe[wIndex]);
                chCRCHi = chCRCLTalbe[wIndex];
            }

            ushort CRC16 = (ushort)((chCRCHi << 8) | chCRCLo);
            return CRC16;
        }

        #endregion

        /*
        /// <summary> 校验 </summary>
        /// <returns>  </returns>
        public static bool Verify()
        {
            bool bFlag = true;

            byte[] Buf = new byte[100];

            for (byte i = 0; i < 10; i++)
            {
                Buf[i] = i;
            }
            ushort GetVol = GetCRC(Buf, 10);
            if (GetVol != 17780)
            {
                logger.Error("CRC校验功能出错");
                bFlag = false;
            }
            else
            {
                logger.Info("CRC校验功能通过");
            }
            if ((EndianUINT16(0x1234) != 0x3412) || (EndianUINT32(0x12345678) != 0x78563412) || (EndianUINT64(0x123456789abcdef0) != 0xf0debc9a78563412))
            {
                logger.Error("程序故障0");
                bFlag = false;
            }
            for (byte i = 1; i <= 16; i++)
            {
                Buf[i - 1] = i;
            }
            if ((GetUINT16(Buf, 0) != 0x0102) || (GetUINT16(Buf, 1) != 0x0203) || (GetUINT16(Buf, 2) != 0x0304))
            {
                logger.Error("程序故障1");
                bFlag = false;
            }
            if ((GetUINT32(Buf, 0) != 0x01020304) || (GetUINT32(Buf, 1) != 0x02030405) || (GetUINT32(Buf, 2) != 0x03040506))
            {
                logger.Error("程序故障2");
                bFlag = false;
            }
            if ((GetUINT64(Buf, 0) != 0x0102030405060708) || (GetUINT64(Buf, 1) != 0x0203040506070809) || (GetUINT64(Buf, 2) != 0x030405060708090a))
            {
                logger.Error("程序故障3");
                bFlag = false;
            }

            return bFlag;
        }
        */

        #region ushort 和 float 互转
        /// <summary> float 数据变为 ushort 数组 </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public static ushort[] FloatToUshort(float data)
        {
            unsafe
            {
                ushort* pdata = (ushort*)&data;
                ushort[] byteArray = new ushort[sizeof(float)];
                for (int i = 0; i < sizeof(float); ++i)
                    byteArray[i] = *pdata++;
                return byteArray;
            }
        }
        public static float UshortToFloat(ushort[] data)
        {
            unsafe
            {
                float a = 0.0F;
                byte i;
                ushort[] x = data;
                void* pf;
                fixed (ushort* px = x)
                {
                    pf = &a;
                    for (i = 0; i < data.Length; i++)
                    {
                        *((ushort*)pf + i) = *(px + i);
                    }
                }
                return a;
            }
        }
        /// <summary>
        /// 另一种BlockCopy方案
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float[] UshortToFloat2(ushort[] data)
        {
            //data = new ushort[2] { 19311, 65529 };
            float[] floatData = new float[data.Length / 2];
            Buffer.BlockCopy(data, 0, floatData, 0, data.Length * 2);

            return floatData;

        }

        /// <summary> 另一种BlockCopy方案</summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public static ushort[] FloatToUshort2(float data)
        {
            ushort[] ushortData = new ushort[sizeof(float)];
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, ushortData, 0, 4);

            return ushortData;
        }
        #endregion



    }
}