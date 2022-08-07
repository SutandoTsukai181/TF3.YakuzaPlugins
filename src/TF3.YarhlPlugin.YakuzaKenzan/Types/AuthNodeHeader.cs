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
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// Auth.bin general node header.
    /// </summary>
    [Serializable]
    public class AuthNodeHeader
    {
        /// <summary>
        /// Gets or sets the first 8 bytes of the GUID.
        /// </summary>
        public ulong Guid0 { get; set; }

        /// <summary>
        /// Gets or sets the second 8 bytes of the GUID.
        /// </summary>
        public ulong Guid1 { get; set; }

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        [BinaryEnum(ReadAs = typeof(uint))]
        public AuthNodeType NodeType { get; set; }

        /// <summary>
        /// Gets or sets the node data size.
        /// </summary>
        public uint NodeDataSize { get; set; }

        /// <summary>
        /// Gets or sets the node count.
        /// </summary>
        public uint NodeCount { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// <remarks>Can be ignored.</remarks>
        /// </summary>
        public uint Padding { get; set; }
    }
}
