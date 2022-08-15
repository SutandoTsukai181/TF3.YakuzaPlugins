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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.MapCmn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Yakuza Kenzan MapCmn file.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, MapCmn>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="MapCmn"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="MapCmn"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public MapCmn Convert(BinaryFormat source)
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

            var strings = new List<string>();
            var stringOffsets = new List<uint>();

            reader.Stream.Seek(0x08);
            var structStart = reader.ReadUInt32();
            var structCount = reader.ReadUInt32();

            for (int i = 0; i < structCount; i++)
            {
                reader.Stream.Seek(structStart + (i * 0x20) + 0x10);
                ReadStringOffset(reader, stringOffsets, strings);
            }

            // We read the whole file, including all of the strings
            // When writing, we just ignore the old strings and write at the end
            reader.Stream.Seek(0);
            var fileBuffer = reader.ReadBytes((int)reader.Stream.Length);

            return new MapCmn(fileBuffer, stringOffsets, strings);
        }

        private void ReadStringOffset(DataReader reader, List<uint> stringOffsets, List<string> strings)
        {
            uint pos = (uint)reader.Stream.Position;
            var offset = reader.ReadUInt32();

            if (offset != 0)
            {
                stringOffsets.Add(pos);
                reader.Stream.RunInPosition(() => strings.Add(reader.ReadString()), offset);
            }
        }
    }
}
