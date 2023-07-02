using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace One.Core.Helpers.DataProcessHelpers
{
    public class ByteHelper
    {
        #region Bytes-Struct

        /// <summary> Byte数组转为StrData，实现了反序列化 </summary>
        /// <param name="bytes">   </param>
        /// <param name="StrData"> </param>
        /// <returns> </returns>
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

        /// <summary> StrData转为Byte数组，实现了序列化 </summary>
        /// <param name="StrData"> </param>
        /// <returns> </returns>
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

        #endregion Bytes-Struct

        #region Byte-Int

        /// <summary>
        /// 该方法将一个int类型的数据转换为byte[]形式，因为int为32bit，而byte为8bit所以在进行类型转换时，只会获取低8位，丢弃高24位。
        /// <para> 通过位移的方式，将32bit的数据转换成4个8bit的数据。 </para>
        /// <para> 注意 0xff，在这当中， 0xff简单理解为一把剪刀，将想要获取的8位数据截取出来。 </para>
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        public static byte[] IntToByteArray(int i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }

        /// <summary>
        /// 利用int2ByteArray方法，将一个int转为byte[]，但在解析时，需要将数据还原。
        /// <para> 同样使用移位的方式，将适当的位数进行还原，0xFF为16进制的数据，所以在其后每加上一位，就相当于二进制加上4位。 </para>
        /// <para> 同时，使用|=号拼接数据，将其还原成最终的int数据 </para>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <returns> </returns>
        public static int ByteArrayToInt(byte[] bytes)
        {
            int num = bytes[3] & 0xFF;
            num |= ((bytes[2] << 8) & 0xFF00);
            num |= ((bytes[1] << 16) & 0xFF0000);
            num |= ((bytes[0] << 24) & 0xFF0000);
            return num;
        }

        #endregion Byte-Int

        #region Compare

        public static bool ArrayCompare(IEnumerable<byte> baseArray, IEnumerable<byte> targetArray, int length, int index = 0)
        {
            int allLength = baseArray.Count();
            if (length > allLength)
            {
                length = allLength;
            }
            if (index > length)
            {
                throw new Exception("index > length");
            }
            for (int i = index; i < length; i++)
            {
                if (baseArray.ElementAt(i) != targetArray.ElementAt(i))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Compare
    }
}