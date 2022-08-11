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
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.Media.Text;

    /// <summary>
    /// Extracts Yakuza Y2007Bin translatable strings to a Po file.
    /// </summary>
    public class ExtractStrings : IConverter<Y2007Bin, Po>, IInitializer<PoHeader>
    {
        private PoHeader _poHeader = new ("NoName", "dummy@dummy.com", "en");

        /// <summary>
        /// Converter initializer.
        /// </summary>
        /// <param name="parameters">Header to use in created Po elements.</param>
        public void Initialize(PoHeader parameters) => _poHeader = parameters;

        /// <summary>
        /// Extracts strings to a Po file.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The po file.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public Po Convert(Y2007Bin source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var po = new Po(_poHeader);

            Extract(source, po);

            return po;
        }

        private void Extract(Y2007Bin bin, Po po)
        {
            for (int i = 0; i < bin.Columns.Length; i++)
            {
                var column = bin.Columns[i];

                switch (column.ColumnType)
                {
                    case Y2007BinColumnType.String:
                    case Y2007BinColumnType.StringTable:
                    case Y2007BinColumnType.StringIndex:
                        for (int j = 0; j < column.ColumnStrings.Count; j++)
                        {
                            if (column.ColumnStrings[j] == string.Empty)
                            {
                                continue;
                            }

                            var entry = new PoEntry()
                            {
                                Original = column.ColumnStrings[j],
                                Translated = column.ColumnStrings[j],
                                Context = $"{i}_{j}",
                                ExtractedComments = $"Column name: {column.ColumnName}",
                            };
                            po.Add(entry);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
