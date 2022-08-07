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
    /// Yakuza Kenzan general bin file (item/shop/*.bin and scenario/*.bin).
    /// </summary>
    public sealed class GeneralBin : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralBin"/> class.
        /// </summary>
        public GeneralBin() : base()
        {
            FileBuffer = null;
            StringOffsets = new List<List<uint>>();
            HeaderStrings = new List<string>();
            StructStrings = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralBin"/> class.
        /// </summary>
        /// <param name="fileBuffer">The file buffer, not including the strings buffer.</param>
        /// <param name="stringOffsets">A list containing a list of offsets for each string.</param>
        /// <param name="headerStrings">Strings whose offsets are in the file header.</param>
        /// <param name="structStrings">Strings whose offsets are in the structs.</param>
        public GeneralBin(byte[] fileBuffer, List<List<uint>> stringOffsets, List<string> headerStrings, List<string> structStrings) : base()
        {
            FileBuffer = fileBuffer;
            StringOffsets = stringOffsets;
            HeaderStrings = headerStrings;
            StructStrings = structStrings;
        }

        /// <summary>
        /// Gets or sets the file buffer.
        /// </summary>
        public byte[] FileBuffer { get; set; }

        /// <summary>
        /// Gets the string offsets list.
        /// </summary>
        public List<List<uint>> StringOffsets { get; }

        /// <summary>
        /// Gets the header strings.
        /// </summary>
        public List<string> HeaderStrings { get; }

        /// <summary>
        /// Gets the struct strings.
        /// </summary>
        public List<string> StructStrings { get; }
    }
}
