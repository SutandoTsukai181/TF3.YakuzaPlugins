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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
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

            List<uint> msgStringOffsets = new List<uint>();

            List<MsgFileString> msgStrings = new List<MsgFileString>();
            List<string> talkerStrings = new List<string>();

            reader.Stream.Seek(0x10);
            reader.Stream.Seek(reader.ReadUInt32());
            var sectionPos = reader.ReadUInt32();
            var sectionCount = reader.ReadUInt16();

            for (int i = 0; i < sectionCount; i++)
            {
                reader.Stream.Seek(sectionPos + (i * 12));
                ReadSection(reader, msgStringOffsets, msgStrings);
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
            if (msgStrings.Count != 0)
            {
                firstStringStart = (int)msgStrings.MinBy(str => str.StringOffset).StringOffset;
            }

            reader.Stream.Seek(0);
            byte[] fileBuffer = reader.ReadBytes(firstStringStart);

            return new MsgFile(fileBuffer, msgStringOffsets, msgStrings, talkerStrings);
        }

        private static void ReadSection(DataReader reader, List<uint> msgStringOffsets, List<MsgFileString> msgStrings)
        {
            reader.Stream.Seek(6, SeekOrigin.Current);

            var groupCount = reader.ReadUInt16();
            var groupPos = reader.ReadUInt32();

            for (int i = 0; i < groupCount; i++)
            {
                reader.Stream.Seek(groupPos + (i * 16));
                ReadGroup(reader, msgStringOffsets, msgStrings);
            }
        }

        private static void ReadGroup(DataReader reader, List<uint> msgStringOffsets, List<MsgFileString> msgStrings)
        {
            reader.Stream.Seek(4, SeekOrigin.Current);

            var stringPos = reader.ReadUInt32();
            reader.ReadByte();
            var stringCount = reader.ReadByte();

            reader.Stream.Seek(stringPos);
            for (int i = 0; i < stringCount; i++)
            {
                msgStringOffsets.Add((uint)reader.Stream.Position);
                msgStrings.Add(ReadMsgString(reader));
            }
        }

        private static MsgFileString ReadMsgString(DataReader reader)
        {
            var msgString = reader.Read<MsgFileString>() as MsgFileString;
            reader.Stream.PushCurrentPosition();

            reader.Stream.Seek(msgString.StringOffset);
            msgString.Text = reader.ReadString(msgString.StringSize);

            msgString.TalkerIndex = -1;

            for (int i = 0; i < msgString.PropCount; i++)
            {
                reader.Stream.Seek(msgString.PropOffset + (i * 16));
                var propType = (MsgPropType)reader.ReadUInt16();

                switch (propType)
                {
                    case MsgPropType.TalkerProp:
                        reader.Stream.Seek(6, SeekOrigin.Current);
                        msgString.TalkerIndex = reader.ReadInt16();
                        break;
                    default:
                        break;
                }
            }

            reader.Stream.PopPosition();
            return msgString;
        }
    }
}
