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
namespace TF3.YarhlPlugin.YakuzaKenzan.Formats
{
    using System.Collections.Generic;
    using Yarhl.FileFormat;

    /// <summary>
    /// Yakuza Kenzan yactplaydata.bin file.
    /// </summary>
    public sealed class YactPlayData : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YactPlayData"/> class.
        /// </summary>
        public YactPlayData() : base()
        {
            FileBuffer = null;
            StringOffsets = new List<List<uint>>();
            Strings = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YactPlayData"/> class.
        /// </summary>
        /// <param name="fileBuffer">Whole file buffer.</param>
        /// <param name="stringOffsets">Text offset.</param>
        /// <param name="strings">Text.</param>
        public YactPlayData(byte[] fileBuffer, List<List<uint>> stringOffsets, List<string> strings) : base()
        {
            FileBuffer = fileBuffer;
            StringOffsets = stringOffsets;
            Strings = strings;
        }

        /// <summary>
        /// Gets or sets the file buffer.
        /// </summary>
        public byte[] FileBuffer { get; set; }

        /// <summary>
        /// Gets or sets the text offset.
        /// </summary>
        public List<List<uint>> StringOffsets { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public List<string> Strings { get; set; }
    }
}
