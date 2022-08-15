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
    /// Yakuza Kenzan stage .snt format.
    /// </summary>
    public sealed class StageSnt : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StageSnt"/> class.
        /// </summary>
        public StageSnt()
        {
            Indices = new List<sbyte>();
            Strings = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StageSnt"/> class.
        /// </summary>
        /// <param name="indices">List of indices.</param>
        /// <param name="strings">List of strings.</param>
        public StageSnt(List<sbyte> indices, List<string> strings)
        {
            Indices = indices;
            Strings = strings;
        }

        /// <summary>
        /// Gets the indices list.
        /// </summary>
        public List<sbyte> Indices { get; }

        /// <summary>
        /// Gets the strings list.
        /// </summary>
        public List<string> Strings { get; }
    }
}
