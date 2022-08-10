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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.PacBin
{
    using System;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Kenzan wdr/pac PacBin file.
    /// </summary>
    public class Writer : IConverter<PacBin, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="PacBin"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(PacBin source)
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

            writer.Write(source.PacBinEntries.Length);

            var offsetsPos = writer.Stream.Position;
            writer.WriteTimes(0, source.PacBinEntries.Length * 0x14);
            writer.WritePadding(0, 16);

            for (int i = 0; i < source.PacBinEntries.Length; i++)
            {
                var entry = source.PacBinEntries[i];

                // Write the MsgFile
                entry.MsgFileOffset = (uint)writer.Stream.Position;
                var msgWriter = new Converters.MsgFile.Writer();
                msgWriter.Convert(entry.MsgFile).Stream.WriteTo(writer.Stream);

                entry.MsgFileSize = (uint)writer.Stream.Position - entry.MsgFileOffset;
                writer.WritePadding(0, 16);

                // Write the pac struct
                entry.PacStructOffset = (uint)writer.Stream.Position;
                writer.Write(entry.PacStructBuffer);
                writer.WritePadding(0, 16);

                writer.Stream.RunInPosition(() => writer.WriteOfType(entry), offsetsPos + (i * 0x14));
            }

            return new BinaryFormat(stream);
        }
    }
}
