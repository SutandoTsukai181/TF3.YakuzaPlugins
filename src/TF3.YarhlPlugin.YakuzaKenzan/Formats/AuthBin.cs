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
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;

    /// <summary>
    /// Yakuza Kenzan Auth.bin file.
    /// </summary>
    public sealed class AuthBin : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthBin"/> class.
        /// </summary>
        public AuthBin() : base()
        {
            FileBuffer = null;
            SubtitleNodeOffsets = new List<uint>();
            SubtitleNodes = new List<AuthSubtitleNode>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthBin"/> class.
        /// </summary>
        /// <param name="fileBuffer">The file buffer.</param>
        /// <param name="subtitleNodeOffsets">A list containing the offset for each subtitle node.</param>
        /// <param name="subtitleNodes">A list containing the subtitle nodes</param>
        public AuthBin(byte[] fileBuffer, List<uint> subtitleNodeOffsets, List<AuthSubtitleNode> subtitleNodes) : base()
        {
            FileBuffer = fileBuffer;
            SubtitleNodeOffsets = subtitleNodeOffsets;
            SubtitleNodes = subtitleNodes;
        }

        /// <summary>
        /// Gets or sets the file buffer.
        /// </summary>
        public byte[] FileBuffer { get; set; }

        /// <summary>
        /// Gets the subtitle node offsets list.
        /// </summary>
        public List<uint> SubtitleNodeOffsets { get; }

        /// <summary>
        /// Gets the subtitle nodes list.
        /// </summary>
        public List<AuthSubtitleNode> SubtitleNodes { get; }
    }
}
