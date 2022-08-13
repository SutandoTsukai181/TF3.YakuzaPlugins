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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.GeneralBin
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types.GeneralBin;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes GeneralBin files.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, GeneralBin>, IInitializer<ReaderParameters>
    {

        private ReaderParameters _readerParameters = new ();

        private List<List<uint>> _stringOffsets;

        private Dictionary<uint, int> _offsetIndexMap;

        /// <summary>
        /// Initializes the reader parameters.
        /// </summary>
        /// <param name="parameters">Reader configuration.</param>
        public void Initialize(ReaderParameters parameters) => _readerParameters = parameters;

        /// <summary>
        /// Converts a BinaryFormat into a <see cref="GeneralBin"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="GeneralBin"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public GeneralBin Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Type binType = Type.GetType(_readerParameters.BinTypeName);

            if (binType == null)
            {
                throw new ArgumentNullException(nameof(binType));
            }

            _stringOffsets = new List<List<uint>>();
            _offsetIndexMap = new Dictionary<uint, int>();

            source.Stream.Position = 0;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var reader = new DataReader(source.Stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            // Read the file header
            var header = reader.ReadByType(binType);

            var headerStrings = new List<string>();
            var structStrings = new List<string>();

            uint headerStringCount = (header.StructStart - (uint)reader.Stream.Position) / 4;

            // Read the header strings. If headerStringCount <= 0, this won't read anything.
            ReadHeaderStrings(reader, headerStrings, headerStringCount);

            if (header.StructCount != 0 && header.StructStride != 0)
            {
                reader.Stream.Seek(header.StructStart);

                if (binType == typeof(MailBin))
                    ReadStructStringsMail(reader, structStrings, header);
                else if (binType == typeof(MapTutorialBin))
                    ReadStructStringsMapTutorial(reader, structStrings, header);
                else if (binType == typeof(RandomBattleBin))
                    ReadStructStringsRandomBattle(reader, structStrings, header);
                else
                    ReadStructStringsGeneral(reader, structStrings, header);
            }

            // Find the offset of the first string so we can store the file in bytes up to the first string
            int firstStringStart = (int)reader.Stream.Length;

            if (_offsetIndexMap.Count != 0)
            {
                firstStringStart = (int)_offsetIndexMap.MinBy(pair => pair.Key).Key;
            }

            reader.Stream.Seek(0);
            byte[] fileBuffer = reader.ReadBytes(firstStringStart);

            return new GeneralBin(fileBuffer, _stringOffsets, headerStrings, structStrings);
        }

        private void ReadHeaderStrings(DataReader reader, List<string> headerStrings, uint count)
        {
            for (int i = 0; i < count; i++)
            {
                ReadStringOffset(reader, headerStrings);
            }
        }

        private void ReadStructStringsGeneral(DataReader reader, List<string> structStrings, dynamic header)
        {
            for (int i = 0; i < header.StructCount; i++)
            {
                var structStart = reader.Stream.Position;

                if (header.StructStringOffsets != null)
                {
                    foreach (uint structOffset in header.StructStringOffsets)
                    {
                        reader.Stream.Seek(structStart + structOffset);
                        ReadStringOffset(reader, structStrings);
                    }
                }

                reader.Stream.Seek(structStart + header.StructStride);
            }
        }

        private void ReadStructStringsMail(DataReader reader, List<string> structStrings, dynamic header)
        {
            for (int i = 0; i < header.StructCount; i++)
            {
                var structStart = reader.Stream.Position;

                foreach (uint structOffset in header.StructStringOffsets)
                {
                    reader.Stream.Seek(structStart + structOffset);
                    ReadStringOffset(reader, structStrings);
                }

                reader.Stream.Seek(structStart + 0x08);

                var offset = reader.ReadUInt32();
                var count = reader.ReadUInt32();

                if (offset != 0 && count != 0)
                {
                    reader.Stream.Seek(offset);
                    ReadHeaderStrings(reader, structStrings, count);
                }

                reader.Stream.Seek(structStart + header.StructStride);
            }
        }

        private void ReadStructStringsMapTutorial(DataReader reader, List<string> structStrings, dynamic header)
        {
            for (int i = 0; i < header.StructCount; i++)
            {
                var structStart = reader.Stream.Position;

                var count = reader.ReadUInt32();
                reader.Stream.Seek(0x04, SeekOrigin.Current);
                var offset = reader.ReadUInt32();

                if (offset != 0 && count != 0)
                {
                    reader.Stream.Seek(offset);

                    for (int j = 0; j < count; j++)
                    {
                        reader.Stream.Seek(0x08, SeekOrigin.Current);
                        ReadStringOffset(reader, structStrings);
                        reader.Stream.Seek(0x14, SeekOrigin.Current);
                    }
                }

                reader.Stream.Seek(structStart + header.StructStride);
            }
        }

        private void ReadStructStringsRandomBattle(DataReader reader, List<string> structStrings, dynamic header)
        {
            // This whole format is a mess to go through, so only the structs that have text are traversed here.
            var binHeader = header as RandomBattleBin;

            for (int i = 0; i < binHeader.StructCount1; i++)
            {
                reader.Stream.Seek(binHeader.StructStart1 + (i * 0x10));

                switch (reader.ReadUInt32())
                {
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x07:
                    case 0x14:
                        ReadStringOffset(reader, structStrings);
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < binHeader.StructCount2; i++)
            {
                reader.Stream.Seek(binHeader.StructStart2 + (i * 0x20) + 0x04);
                ReadStringOffset(reader, structStrings);
            }

            reader.Stream.Seek(binHeader.StructStart3 + 0x10);

            var structStart3 = reader.ReadUInt32();
            var structCount3 = reader.ReadUInt32();

            var structHeader3Start = reader.ReadUInt32();
            var structHeader3End = reader.ReadUInt32();

            var structHeader4Start = reader.ReadUInt32();

            for (int i = 0; i < structCount3; i++)
            {
                reader.Stream.Seek(structStart3 + (i * 0x20));
                ReadStringOffset(reader, structStrings);
                ReadStringOffset(reader, structStrings);
            }

            reader.Stream.Seek(structHeader3Start + 0x04);
            reader.Stream.Seek(reader.ReadUInt32());
            while (reader.Stream.Position < structHeader3End)
            {
                ReadStringOffset(reader, structStrings);
            }

            reader.Stream.Seek(structHeader4Start);

            var structStart4 = reader.ReadUInt32();
            var structCount4 = reader.ReadUInt32();

            for (int i = 0; i < structCount4; i++)
            {
                reader.Stream.Seek(structStart4 + (i * 0x10));

                var structStart5 = reader.ReadUInt32();
                var structCount5 = reader.ReadUInt32();

                for (int j = 0; j < structCount5; j++)
                {
                    reader.Stream.Seek(structStart5 + (j * 0x30));
                    ReadStringOffset(reader, structStrings);
                    _ = reader.ReadUInt32();
                    ReadStringOffset(reader, structStrings);
                    ReadStringOffset(reader, structStrings);

                    reader.Stream.Seek(0x10, SeekOrigin.Current);
                    ReadStringOffset(reader, structStrings);
                }
            }
        }

        private void ReadStringOffset(DataReader reader, List<string> stringList)
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

                    reader.Stream.RunInPosition(() => stringList.Add(reader.ReadString()), offset);
                }

                _stringOffsets[index].Add(pos);
            }
        }
    }
}
