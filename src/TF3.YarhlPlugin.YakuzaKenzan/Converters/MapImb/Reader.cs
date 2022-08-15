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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.MapImb
{
    using System;
    using System.IO;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes Yakuza Kenzan MapImb file.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, MapImb>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="MapImb"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="MapImb"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public MapImb Convert(BinaryFormat source)
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

            string text = null;

            reader.Stream.Seek(0x20);
            var offset = reader.ReadUInt32();

            if (offset != 0)
            {
                reader.Stream.Seek(offset);
                text = reader.ReadString();
            }

            reader.Stream.Seek(0);
            var fileBuffer = reader.ReadBytes((int)(offset == 0 ? reader.Stream.Length : offset));

            return new MapImb(fileBuffer, offset, text);
        }
    }
}
