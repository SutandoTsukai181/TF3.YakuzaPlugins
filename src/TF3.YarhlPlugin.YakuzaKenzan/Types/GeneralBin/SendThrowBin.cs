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
namespace TF3.YarhlPlugin.YakuzaKenzan.Types.GeneralBin
{
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// item/shop/send.bin and item/shop/throw.bin.
    /// </summary>
    [Serializable]
    public class SendThrowBin
    {
        /// <summary>
        /// Gets or sets the struct count.
        /// </summary>
        public ushort StructCount { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// <remarks>Can be ignored.</remarks>
        /// </summary>
        public ushort Padding { get; set; }

        /// <summary>
        /// Gets or sets the structs start offset.
        /// </summary>
        public uint StructStart { get; set; }

        /// <summary>
        /// Gets the size of each struct.
        /// </summary>
        public readonly uint StructStride = 0x00;

        /// <summary>
        /// Gets the offsets for string offsets in each struct.
        /// </summary>
        public readonly uint[] StructStringOffsets = null;
    }
}
