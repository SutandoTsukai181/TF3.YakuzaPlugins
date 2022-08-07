// Copyright (c) 2021 Kaplas
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.PacEncount
{
    using System;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Yakuza Kenzan pac_encount.bin file.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, PacEncount>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="PacEncount"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="PacEncount"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public PacEncount Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            source.Stream.Position = 0;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var reader = new DataReader(source.Stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            uint totalMsgCount = 0;
            uint[] listCounts = new uint[reader.ReadUInt32()];
            for (int i = 0; i < listCounts.Length; i++)
            {
                listCounts[i] = reader.ReadUInt32();
                totalMsgCount += listCounts[i];
            }

            uint[] msgOffsets = new uint[totalMsgCount + 1];
            for (int i = 0; i < totalMsgCount; i++)
            {
                msgOffsets[i] = reader.ReadUInt32();
            }

            msgOffsets[totalMsgCount] = (uint)source.Stream.Length;

            MsgFile[][] msgArrays = new MsgFile[listCounts.Length][];

            int curMsgIndex = 0;
            for (int i = 0; i < msgArrays.Length; i++)
            {
                msgArrays[i] = new MsgFile[listCounts[i]];

                for (int j = 0; j < msgArrays[i].Length; j++)
                {
                    var msgReader = new Converters.MsgFile.Reader();

                    var length = msgOffsets[curMsgIndex + 1] - msgOffsets[curMsgIndex];
                    var msgFormat = new BinaryFormat(source.Stream, msgOffsets[curMsgIndex], length);
                    msgArrays[i][j] = msgReader.Convert(msgFormat);

                    ++curMsgIndex;
                }
            }

            return new PacEncount(msgArrays);
        }
    }
}
