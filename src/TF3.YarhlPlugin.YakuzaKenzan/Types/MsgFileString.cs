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
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// MsgFile string.
    /// </summary>
    [Serializable]
    public class MsgFileString
    {
        /// <summary>
        /// Gets or sets the string size (in bytes).
        /// </summary>
        public ushort StringSize { get; set; }

        /// <summary>
        /// Gets or sets the property count.
        /// </summary>
        public byte PropCount { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// <remarks>Can be ignored.</remarks>
        /// </summary>
        public byte Padding { get; set; }

        /// <summary>
        /// Gets or sets the string offset.
        /// </summary>
        public uint StringOffset { get; set; }

        /// <summary>
        /// Gets or sets the properties offset.
        /// </summary>
        public uint PropOffset { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [BinaryIgnore]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the talker index for the string.
        /// </summary>
        [BinaryIgnore]
        public short TalkerIndex { get; set; }
    }
}
