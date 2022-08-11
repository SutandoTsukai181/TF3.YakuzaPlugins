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
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Y2007Bin files.
    /// </summary>
    public class Writer : IConverter<Y2007Bin, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="Y2007Bin"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(Y2007Bin source)
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

            var header = new Y2007BinFileHeader
            {
                Magic = 0x20070319,
                ColumnCount = (uint)source.Columns.Length,
                RowCount = source.RowCount,
                Padding = 0,
            };

            writer.WriteOfType(header);
            writer.WriteTimes(0, 0x40 * source.Columns.Length);

            WriteColumns(writer, source.Columns);

            return new BinaryFormat(stream);
        }

        private static void WriteColumns(DataWriter writer, Y2007BinColumn[] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];

                var dataStart = writer.Stream.Position;
                switch (column.ColumnType)
                {
                    case Y2007BinColumnType.String:
                        for (int j = 0; j < column.ColumnStrings.Count; j++)
                        {
                            writer.Write(column.ColumnStrings[j]);
                        }

                        break;
                    case Y2007BinColumnType.StringTable:
                        for (int j = 0; j < column.ColumnStrings.Count; j++)
                        {
                            writer.Write(column.ColumnStrings[j]);
                        }

                        for (int j = 0; j < column.ColumnValues.Count; j++)
                        {
                            writer.WriteOfType((byte)column.ColumnValues[j]);
                        }

                        break;
                    case Y2007BinColumnType.StringIndex:
                        for (int j = 0; j < column.ColumnStrings.Count; j++)
                        {
                            writer.WriteOfType((short)column.ColumnValues[j]);
                            writer.Write(column.ColumnStrings[j]);
                        }

                        break;
                    default:
                        writer.Write(column.ColumnDataBuffer);
                        break;
                }

                // Update the column data size.
                column.ColumnDataSize = (uint)(writer.Stream.Position - dataStart);

                // Write the column struct
                writer.Stream.RunInPosition(() => writer.WriteOfType(column), 0x10 + (0x40 * i));
            }
        }
    }
}
