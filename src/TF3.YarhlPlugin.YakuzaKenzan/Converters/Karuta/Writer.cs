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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.Karuta
{
    using System;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Kenzan Karuta files.
    /// </summary>
    public class Writer : IConverter<KarutaFile, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="KarutaFile"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(KarutaFile source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            DataStream stream = DataStreamFactory.FromMemory();

            var writer = new DataWriter(stream)
            {
                DefaultEncoding = Encoding.UTF8,
                Endianness = EndiannessMode.BigEndian,
            };

            var header = new FileHeader
            {
                Magic = "KRTA",
                PlatformId = 2,
                Endianness = Endianness.BigEndian,
                SizeExtended = 0,
                Relocated = 0,
                Version = 0x00000065,
                Size = 0,
            };

            writer.WriteOfType(header);

            writer.WriteOfType((uint)source.Cards.Length);
            writer.WriteTimes(0x00, 12);

            WriteCards(writer, source.Cards);

            _ = writer.Stream.Seek(0x0C, System.IO.SeekOrigin.Begin);
            writer.Write((uint)writer.Stream.Length);

            return new BinaryFormat(stream);
        }

        private static void WriteCards(DataWriter writer, KarutaCard[] cards)
        {
            foreach (var card in cards)
            {
                writer.WriteOfType(card);
            }
        }
    }
}
