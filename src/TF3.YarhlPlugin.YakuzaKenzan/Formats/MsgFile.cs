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
    /// Yakuza Kenzan .msg file.
    /// </summary>
    public sealed class MsgFile : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsgFile"/> class.
        /// </summary>
        public MsgFile() : base()
        {
            FileBuffer = null;
            TextStringOffsets = new List<uint>();
            TextTalkerIndices = new List<int>();
            TextStrings = new List<string>();
            TalkerStrings = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsgFile"/> class.
        /// </summary>
        /// <param name="fileBuffer">The file buffer.</param>
        /// <param name="textOffsets">A list containing the offset for each text string.</param>
        /// <param name="textTalkerIndices">A list containing the talker index for each text string.</param>
        /// <param name="textStrings">A list containing the text strings.</param>
        /// <param name="talkerStrings">A list containing the talker strings.</param>
        public MsgFile(byte[] fileBuffer, List<uint> textOffsets, List<int> textTalkerIndices, List<string> textStrings, List<string> talkerStrings) : base()
        {
            FileBuffer = fileBuffer;
            TextStringOffsets = textOffsets;
            TextTalkerIndices = textTalkerIndices;
            TextStrings = textStrings;
            TalkerStrings = talkerStrings;
        }

        /// <summary>
        /// Gets or sets the file buffer.
        /// </summary>
        public byte[] FileBuffer { get; set; }

        /// <summary>
        /// Gets the text strings offsets list.
        /// </summary>
        public List<uint> TextStringOffsets { get; }

        /// <summary>
        /// Gets the text strings talker indices.
        /// </summary>
        public List<int> TextTalkerIndices { get; }

        /// <summary>
        /// Gets the text strings list.
        /// </summary>
        public List<string> TextStrings { get; }

        /// <summary>
        /// Gets the talker strings list.
        /// </summary>
        public List<string> TalkerStrings { get; }
    }
}
