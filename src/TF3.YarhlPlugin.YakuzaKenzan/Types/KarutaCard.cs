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
    /// Archive header.
    /// </summary>
    [Serializable]
    public class KarutaCard
    {
        /// <summary>
        /// Gets or sets the index.
        /// <remarks>Always the index of the card in the file.</remarks>
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Gets or sets the other index.
        /// <remarks>Seems to map to a different list.</remarks>
        /// </summary>
        public uint IndexOther { get; set; }

        /// <summary>
        /// Gets or sets the card text.
        /// </summary>
        [BinaryString(CodePage = 932, FixedSize = 0xF8)]
        public string Text { get; set; }
    }
}
