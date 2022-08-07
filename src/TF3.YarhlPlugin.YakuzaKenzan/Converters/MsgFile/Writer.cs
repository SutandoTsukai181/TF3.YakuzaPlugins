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
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Kenzan .msg files.
    /// </summary>
    public class Writer : IConverter<MsgFile, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="MsgFile"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(MsgFile source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            DataStream stream = DataStreamFactory.FromMemory();

            var writer = new DataWriter(stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            writer.Write(source.FileBuffer);

            for (int i = 0; i < source.TextStrings.Count; i++)
            {
                uint pos = (uint)writer.Stream.Position;
                writer.Write(source.TextStrings[i]);
                uint length = (uint)writer.Stream.Position - pos - 1;

                writer.Stream.RunInPosition(() => writer.Write((ushort)length), source.TextStringOffsets[i] - 4);
                writer.Stream.RunInPosition(() => writer.Write(pos), source.TextStringOffsets[i]);
            }

            writer.WritePadding(0, 4);
            uint talkerPos = (uint)writer.Stream.Position;

            writer.Stream.PushToPosition(0x3C);
            writer.Write(talkerPos);
            writer.Write((ushort)source.TalkerStrings.Count);
            writer.Stream.PopPosition();

            writer.WriteTimes(0, source.TalkerStrings.Count * 4);

            for (int i = 0; i < source.TalkerStrings.Count; i++)
            {
                uint pos = (uint)writer.Stream.Position;
                writer.Write(source.TalkerStrings[i]);

                writer.Stream.RunInPosition(() => writer.Write(pos), talkerPos + (i * 4));
            }

            return new BinaryFormat(stream);
        }
    }
}
