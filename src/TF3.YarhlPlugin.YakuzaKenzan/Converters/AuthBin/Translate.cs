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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.AuthBin
{
    using System;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;
    using Yarhl.Media.Text;

    /// <summary>
    /// Inserts strings from Po file to a <see cref="AuthBin"/>.
    /// </summary>
    public class Translate : IConverter<AuthBin, AuthBin>, IInitializer<Po>
    {
        private Po _translation = null;

        /// <summary>
        /// Converter initializer.
        /// </summary>
        /// <remarks>
        /// Initialization is mandatory.
        /// </remarks>
        /// <param name="parameters">Po with translation.</param>
        public void Initialize(Po parameters) => _translation = parameters;

        /// <summary>
        /// Inserts the translated strings from Po file in a <see cref="AuthBin"/>.
        /// </summary>
        /// <param name="source">Original <see cref="AuthBin"/>.</param>
        /// <returns>Translated <see cref="AuthBin"/>.</returns>
        public AuthBin Convert(AuthBin source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (_translation == null)
            {
                throw new InvalidOperationException("Uninitialized");
            }

            AuthBin result = source;

            InsertStrings(result);

            return result;
        }

        private void InsertStrings(AuthBin bin)
        {
            for (int i = 0; i < bin.NodeHeaders.Count; i++)
            {
                bin.NodeHeaders[i].SubtitleNodes.Clear();
            }

            foreach (PoEntry entry in _translation.Entries)
            {
                var splits = entry.Context.Split('_', 3);

                int index = int.Parse(splits[0]);
                var startFrame = uint.Parse(splits[1]);
                var endFrame = uint.Parse(splits[2]);

                var subtitleNode = new AuthSubtitleNode(startFrame, endFrame, entry.Translated.Replace("\r\n", "\n").Replace("\n", "\\n"));
                bin.NodeHeaders[index].SubtitleNodes.Add(subtitleNode);
            }
        }
    }
}
