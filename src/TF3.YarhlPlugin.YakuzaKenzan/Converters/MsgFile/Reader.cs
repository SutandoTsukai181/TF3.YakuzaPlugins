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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.MsgFile
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Yakuza Kenzan .msg files.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, MsgFile>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="MsgFile"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="MsgFile"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public MsgFile Convert(BinaryFormat source)
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

            List<uint> textOffsets = new List<uint>();
            List<int> textTalkerIndices = new List<int>();

            List<string> textStrings = new List<string>();
            List<string> talkerStrings = new List<string>();

            reader.Stream.Seek(0x10);
            reader.Stream.Seek(reader.ReadUInt32());
            var sectionPos = reader.ReadUInt32();
            var sectionCount = reader.ReadUInt16();

            for (int i = 0; i < sectionCount; i++)
            {
                reader.Stream.Seek(sectionPos + (i * 12));
                ReadSection(reader, textOffsets, textTalkerIndices, textStrings);
            }

            reader.Stream.Seek(0x3C);
            var talkerPos = reader.ReadUInt32();
            var talkerCount = reader.ReadUInt16();

            reader.Stream.Seek(talkerPos);
            for (int i = 0; i < talkerCount; i++)
            {
                reader.Stream.RunInPosition(() => talkerStrings.Add(reader.ReadString()), reader.ReadUInt32());
            }

            // Find the offset of the first string so we can store the file in bytes up to the first string
            int firstStringStart = (int)talkerPos;

            if (textOffsets.Count != 0)
            {
                reader.Stream.PushToPosition(textOffsets.Min());
                firstStringStart = (int)reader.ReadInt32();
                reader.Stream.PopPosition();
            }

            reader.Stream.Seek(0);
            byte[] fileBuffer = reader.ReadBytes(firstStringStart);

            return new MsgFile(fileBuffer, textOffsets, textTalkerIndices, textStrings, talkerStrings);
        }

        private static void ReadSection(DataReader reader, List<uint> textOffsets, List<int> textTalkerIndices, List<string> textStrings)
        {
            reader.Stream.Seek(6, SeekOrigin.Current);

            var groupCount = reader.ReadUInt16();
            var groupPos = reader.ReadUInt32();

            for (int i = 0; i < groupCount; i++)
            {
                reader.Stream.Seek(groupPos + (i * 16));
                ReadGroup(reader, textOffsets, textTalkerIndices, textStrings);
            }
        }

        private static void ReadGroup(DataReader reader, List<uint> textOffsets, List<int> textTalkerIndices, List<string> textStrings)
        {
            reader.Stream.Seek(4, SeekOrigin.Current);

            var textPos = reader.ReadUInt32();
            reader.ReadByte();
            var textCount = reader.ReadByte();

            for (int i = 0; i < textCount; i++)
            {
                reader.Stream.Seek(textPos + (i * 12));
                ReadText(reader, textOffsets, textTalkerIndices, textStrings);
            }
        }

        private static void ReadText(DataReader reader, List<uint> textOffsets, List<int> textTalkerIndices, List<string> textStrings)
        {
            var stringLength = reader.ReadUInt16();
            var propCount = reader.ReadSByte();
            reader.ReadByte();

            textOffsets.Add((uint)reader.Stream.Position);
            var stringPos = reader.ReadUInt32();
            var propPos = reader.ReadUInt32();

            reader.Stream.RunInPosition(() => textStrings.Add(reader.ReadString(stringLength)), stringPos);

            var index = textTalkerIndices.Count;
            textTalkerIndices.Add(-1);

            for (int i = 0; i < propCount; i++)
            {
                reader.Stream.Seek(propPos + (i * 16));
                var propType = reader.ReadUInt16();

                if (propType == 0x020B)
                {
                    reader.Stream.Seek(6, SeekOrigin.Current);
                    textTalkerIndices[index] = reader.ReadInt16();
                    break;
                }
            }
        }
    }
}
