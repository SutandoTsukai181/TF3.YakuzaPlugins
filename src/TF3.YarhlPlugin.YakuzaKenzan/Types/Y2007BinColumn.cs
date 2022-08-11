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
namespace TF3.YarhlPlugin.YakuzaKenzan.Types
{
    using System.Collections.Generic;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// Yakuza 2007 bin column.
    /// </summary>
    [Serializable]
    public class Y2007BinColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Y2007BinColumn"/> class.
        /// </summary>
        public Y2007BinColumn()
        {
            ColumnName = string.Empty;
            ColumnTypeName = string.Empty;

            ColumnType = Y2007BinColumnType.String;
            EntryCount = 0;
            ColumnDataSize = 0;
            Padding = 0;

            ColumnStrings = new List<string>();
            ColumnValues = new List<int>();
        }

        /// <summary>
        /// Gets or sets the column string data.
        /// <remarks>Contains column name and column type name, each with a null terminator.</remarks>
        /// </summary>
        [BinaryString(CodePage = 932, FixedSize = 0x30)]
        public string ColumnNameData
        {
            get
            {
                return ColumnName + '\0' + ColumnTypeName;
            }
            set
            {
                var splits = value.Split('\0', 2);

                ColumnName = splits[0];
                ColumnTypeName = splits.Length == 2 ? splits[1].TrimEnd('\0') : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        [BinaryEnum(ReadAs = typeof(uint), WriteAs = typeof(uint))]
        public Y2007BinColumnType ColumnType { get; set; }

        /// <summary>
        /// Gets or sets the entry count.
        /// </summary>
        public uint EntryCount { get; set; }

        /// <summary>
        /// Gets or sets the column data size.
        /// </summary>
        public uint ColumnDataSize { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// <remarks>Can be ignored.</remarks>
        /// </summary>
        public uint Padding { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        [BinaryIgnore]
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the column type name.
        /// </summary>
        [BinaryIgnore]
        public string ColumnTypeName { get; set; }

        /// <summary>
        /// Gets or sets the column data buffer. This will only be used if the column type is appropriate.
        /// </summary>
        [BinaryIgnore]
        public byte[] ColumnDataBuffer { get; set; }

        /// <summary>
        /// Gets the column strings list. This will only be used if the column type is appropriate.
        /// </summary>
        [BinaryIgnore]
        public List<string> ColumnStrings { get; }

        /// <summary>
        /// Gets the column values list. This will only be used if the column type is appropriate.
        /// </summary>
        [BinaryIgnore]
        public List<int> ColumnValues { get; }
    }
}
