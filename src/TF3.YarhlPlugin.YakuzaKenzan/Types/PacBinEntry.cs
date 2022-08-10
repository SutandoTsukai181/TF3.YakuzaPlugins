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
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// Pac file entry (wdr/pac/pac_*.bin).
    /// </summary>
    [Serializable]
    public class PacBinEntry
    {
        /// <summary>
        /// Gets or sets the entry flags.
        /// </summary>
        public ushort Flags { get; set; }

        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the MsgFile size.
        /// </summary>
        public uint MsgFileSize { get; set; }

        /// <summary>
        /// Gets or sets the pac struct size.
        /// </summary>
        public uint PacStructSize { get; set; }

        /// <summary>
        /// Gets or sets the MsgFile offset.
        /// </summary>
        public uint MsgFileOffset { get; set; }

        /// <summary>
        /// Gets or sets the pac struct offset.
        /// </summary>
        public uint PacStructOffset { get; set; }

        /// <summary>
        /// Gets or sets the MsgFile.
        /// </summary>
        [BinaryIgnore]
        public MsgFile MsgFile { get; set; }

        /// <summary>
        /// Gets or sets the pac struct buffer.
        /// </summary>
        [BinaryIgnore]
        public byte[] PacStructBuffer { get; set; }
    }
}
