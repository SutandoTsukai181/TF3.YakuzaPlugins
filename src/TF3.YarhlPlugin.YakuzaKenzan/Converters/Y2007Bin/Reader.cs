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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.Y2007Bin
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
    /// Deserializes Yakuza Y2007Bin files.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, Y2007Bin>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="Y2007Bin"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="Y2007Bin"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public Y2007Bin Convert(BinaryFormat source)
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

            // Read the file header
            var header = reader.Read<Y2007BinFileHeader>() as Y2007BinFileHeader;
            CheckHeader(header);

            return new Y2007Bin(header.RowCount, ReadColumns(reader, header));
        }

        private static void CheckHeader(Y2007BinFileHeader header)
        {
            if (header.Magic != 0x20070319)
            {
                throw new FormatException($"2007: Bad magic Id ({header.Magic} != 0x20070319)");
            }
        }

        private static Y2007BinColumn[] ReadColumns(DataReader reader, Y2007BinFileHeader header)
        {
            var currentDataStart = 0x10 + (0x40 * header.ColumnCount);

            var columns = new Y2007BinColumn[header.ColumnCount];
            for (int i = 0; i < columns.Length; i++)
            {
                var column = reader.Read<Y2007BinColumn>() as Y2007BinColumn;

                reader.Stream.PushToPosition(currentDataStart);
                switch (column.ColumnType)
                {
                    case Y2007BinColumnType.String:
                        for (int j = 0; j < column.EntryCount; j++)
                        {
                            column.ColumnStrings.Add(reader.ReadString());
                        }

                        break;
                    case Y2007BinColumnType.StringTable:
                        for (int j = 0; j < column.EntryCount; j++)
                        {
                            column.ColumnStrings.Add(reader.ReadString());
                        }

                        for (int j = 0; j < header.RowCount; j++)
                        {
                            column.ColumnValues.Add(reader.ReadByte());
                        }

                        break;
                    case Y2007BinColumnType.StringIndex:
                        for (int j = 0; j < column.EntryCount; j++)
                        {
                            column.ColumnValues.Add(reader.ReadInt16());
                            column.ColumnStrings.Add(reader.ReadString());
                        }

                        break;
                    default:
                        column.ColumnDataBuffer = reader.ReadBytes((int)column.ColumnDataSize);
                        break;
                }

                currentDataStart += column.ColumnDataSize;
                reader.Stream.PopPosition();

                columns[i] = column;
            }

            return columns;
        }
    }
}
