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
    using System.IO;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Karuta files.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, KarutaFile>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="KarutaFile"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="KarutaFile"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public KarutaFile Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            source.Stream.Position = 0;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var reader = new DataReader(source.Stream)
            {
                DefaultEncoding = Encoding.UTF8,
                Endianness = EndiannessMode.BigEndian,
            };

            // Read the file header
            var header = reader.Read<FileHeader>() as FileHeader;
            CheckHeader(header);

            if (header!.Endianness == Endianness.LittleEndian)
            {
                reader.Endianness = EndiannessMode.LittleEndian;
            }

            uint cardCount = reader.ReadUInt32();
            reader.Stream.Seek(12, SeekOrigin.Current);

            return new KarutaFile(ReadCards(reader, cardCount));
        }

        private static void CheckHeader(FileHeader header)
        {
            if (header.Magic != "KRTA")
            {
                throw new FormatException($"KRTA: Bad magic Id ({header.Magic} != KRTA)");
            }

            if (header.Version != 0x65)
            {
                throw new FormatException($"KRTA: Bad version number ({header.Version} != 0x65)");
            }
        }

        private static KarutaCard[] ReadCards(DataReader reader, uint count)
        {
            KarutaCard[] cards = new KarutaCard[count];

            for (int i = 0; i < count; i++)
            {
                cards[i] = reader.Read<KarutaCard>() as KarutaCard;
                cards[i].Text = cards[i].Text.TrimEnd('\0');
            }

            return cards;
        }
    }
}
