using System;

namespace One.Core.Helpers.DataProcessHelpers
{
    public class NumberHelper
    {
        #region 转换为大端模式

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat"> </param>
        /// <returns> </returns>
        public static ushort EndianUINT16(ushort Dat)
        {
            ushort Val;
            byte[] Buf = new byte[8];
            Buf[0] = (byte)(Dat >> 0x00);
            Buf[1] = (byte)(Dat >> 0x08);
            Val = (ushort)(((Buf[0] & 0xFF) << 0x08) | ((Buf[1] & 0xFF) << 0x00));
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat"> </param>
        /// <returns> </returns>
        public static uint EndianUINT32(uint Dat)
        {
            uint Val;
            byte[] Buf = new byte[8];
            Buf[0] = (byte)(Dat >> 0);
            Buf[1] = (byte)(Dat >> 8);
            Buf[2] = (byte)(Dat >> 16);
            Buf[3] = (byte)(Dat >> 24);
            Val = (uint)(((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) |
                          ((Buf[3] & 0xFF) << 0));
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat"> </param>
        /// <returns> </returns>
        public static ulong EndianUINT64(ulong Dat)
        {
            ulong Val = 0;
            uint Val1 = 0, Val2 = 0;
            byte[] Buf = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                Buf[i] = (byte)(Dat >> i * 8);
            }

            Val1 = (uint)(((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) |
                           ((Buf[3] & 0xFF) << 0));
            Val2 = (uint)(((Buf[4] & 0xFF) << 24) | ((Buf[5] & 0xFF) << 16) | ((Buf[6] & 0xFF) << 8) |
                           ((Buf[7] & 0xFF) << 0));
            Val = (((ulong)Val1 & 0xFFFFFFFF) << 32) | (((ulong)Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        /// <summary> 转换为对应长度的大端模式 </summary>
        /// <param name="Dat"> </param>
        /// <returns> </returns>
        public static long EndianINT64(long Dat)
        {
            long Val = 0;
            int Val1 = 0, Val2 = 0;
            byte[] Buf = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                Buf[i] = (byte)(Dat >> i * 8);
            }

            Val1 = ((Buf[0] & 0xFF) << 24) | ((Buf[1] & 0xFF) << 16) | ((Buf[2] & 0xFF) << 8) | ((Buf[3] & 0xFF) << 0);
            Val2 = ((Buf[4] & 0xFF) << 24) | ((Buf[5] & 0xFF) << 16) | ((Buf[6] & 0xFF) << 8) | ((Buf[7] & 0xFF) << 0);
            Val = ((Val1 & 0xFFFFFFFF) << 32) | ((Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        #endregion 转换为大端模式

        #region 获取对应类型的大端模式

        public static ushort GetUINT16(byte[] Buf, int Index = 0)
        {
            ushort Val;
            Val = (ushort)(((Buf[Index + 0] & 0xFF) << 0x08) | ((Buf[Index + 1] & 0xFF) << 0x00));
            return Val;
        }

        public static uint GetUINT32(byte[] Buf, int Index = 0)
        {
            uint Val;
            Val = (uint)(((Buf[Index + 0] & 0xFF) << 24) | ((Buf[Index + 1] & 0xFF) << 16) |
                          ((Buf[Index + 2] & 0xFF) << 8) | ((Buf[Index + 3] & 0xFF) << 0));
            return Val;
        }

        public static ulong GetUINT64(byte[] Buf, int Index = 0)
        {
            ulong Val = 0;
            uint Val1 = 0, Val2 = 0;
            Val1 = (uint)(((Buf[Index + 0] & 0xFF) << 24) | ((Buf[Index + 1] & 0xFF) << 16) |
                           ((Buf[Index + 2] & 0xFF) << 8) | ((Buf[Index + 3] & 0xFF) << 0));
            Val2 = (uint)(((Buf[Index + 4] & 0xFF) << 24) | ((Buf[Index + 5] & 0xFF) << 16) |
                           ((Buf[Index + 6] & 0xFF) << 8) | ((Buf[Index + 7] & 0xFF) << 0));
            Val = (((ulong)Val1 & 0xFFFFFFFF) << 32) | (((ulong)Val2 & 0xFFFFFFFF) << 0);
            return Val;
        }

        #endregion 获取对应类型的大端模式

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

        /// <summary> 另一种BlockCopy方案 </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public static float[] UshortToFloat2(ushort[] data)
        {
            //data = new ushort[2] { 19311, 65529 };
            float[] floatData = new float[data.Length / 2];
            Buffer.BlockCopy(data, 0, floatData, 0, data.Length * 2);

            return floatData;
        }

        /// <summary> 另一种BlockCopy方案 </summary>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public static ushort[] FloatToUshort2(float data)
        {
            ushort[] ushortData = new ushort[sizeof(float)];
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, ushortData, 0, sizeof(float));

            return ushortData;
        }

        #endregion ushort 和 float 互转
    }
}