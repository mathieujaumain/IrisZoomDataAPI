using System.IO;
using System;
using Ionic.Zlib;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace IrisZoomDataApi.Compressing
{
    public static class Compressor
    {
        /// <summary>
        /// good ol' zlib.NET implementation likes Eugen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Decomp(byte[] input)
        {
            using (var output = new MemoryStream())
            {
                using (var zipStream = new ZlibStream(output, Ionic.Zlib.CompressionMode.Decompress))
                {
                    using (var inputStream = new MemoryStream(input))
                    {
                        var buffer = new byte[4096];
                        int size = 1;

                        while (size > 0)
                        {
                            size = inputStream.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, size);
                        }
                    }
                }
                return output.ToArray();
            }
        }

        public static byte[] Comp(byte[] input)
        {
            using (var sourceStream = new MemoryStream(input))
            {
                using (var compressed = new MemoryStream())
                {
                    using (var zipSteam = new ZlibStream(compressed, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.Level9, true))
                    {
                        zipSteam.FlushMode = FlushType.Full;

                        //var buffer = new byte[1024];
                        //int len = sourceStream.Read(buffer, 0, buffer.Length);
                        
                        //while (len > 0)
                        //{
                        //    zipSteam.Write(buffer, 0, len);
                        //    len = sourceStream.Read(buffer, 0, buffer.Length);
                        //}

                        sourceStream.CopyTo(zipSteam);

                        zipSteam.Flush();

                        return compressed.ToArray();
                    }
                }
            }
        }

    }
}
