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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.YactPlayData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Yakuza Kenzan YactPlayData file.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, YactPlayData>
    {
        private List<string> _strings;

        private List<List<uint>> _stringOffsets;

        private Dictionary<uint, int> _offsetIndexMap;

        /// <summary>
        /// Converts a BinaryFormat into a <see cref="YactPlayData"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="YactPlayData"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public YactPlayData Convert(BinaryFormat source)
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

            var magic = reader.ReadString(4);
            if (magic != "_MMP")
            {
                throw new FormatException($"YactPlayData: Bad magic Id ({magic} != _MMP)");
            }

            _strings = new List<string>();
            _stringOffsets = new List<List<uint>>();
            _offsetIndexMap = new Dictionary<uint, int>();

            reader.Stream.Seek(0x10);
            for (int i = 0; i < 4; i++)
            {
                var structStart = reader.ReadUInt32();
                var structCount = reader.ReadUInt32();

                if (i == 3)
                {
                    // Hack: Avoid reading last struct because its offset leads to an unwanted section
                    --structCount;
                }

                reader.Stream.PushCurrentPosition();
                for (int j = 0; j < structCount; j++)
                {
                    reader.Stream.Seek(structStart + (j * 0x6C));
                    ReadStruct(reader);
                }

                reader.Stream.PopPosition();
            }

            // We read the whole file, including all of the strings
            // When writing, we just ignore the old strings and write at the end
            reader.Stream.Seek(0);
            var fileBuffer = reader.ReadBytes((int)reader.Stream.Length);

            return new YactPlayData(fileBuffer, _stringOffsets, _strings);
        }

        private void ReadStruct(DataReader reader)
        {
            reader.Stream.Seek(0x5C, SeekOrigin.Current);
            var structOffset = reader.ReadUInt32();

            if (structOffset != 0)
            {
                reader.Stream.Seek(structOffset + 0x08);

                // There seems to be 5 strings at most per struct
                for (int i = 0; i < 5; i++)
                {
                    ReadStringOffset(reader);
                }
            }
        }

        private void ReadStringOffset(DataReader reader)
        {
            int index;
            uint pos = (uint)reader.Stream.Position;
            var offset = reader.ReadUInt32();

            if (offset != 0)
            {
                if (!_offsetIndexMap.TryGetValue(offset, out index))
                {
                    index = _stringOffsets.Count;

                    _stringOffsets.Add(new List<uint>());
                    _offsetIndexMap.Add(offset, index);

                    reader.Stream.RunInPosition(() => _strings.Add(reader.ReadString()), offset);
                }

                _stringOffsets[index].Add(pos);
            }
        }
    }
}
