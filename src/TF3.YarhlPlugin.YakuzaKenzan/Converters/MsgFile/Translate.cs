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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.MsgFile
{
    using System;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.Media.Text;

    /// <summary>
    /// Inserts strings from Po file to a <see cref="MsgFile"/>.
    /// </summary>
    public class Translate : IConverter<MsgFile, MsgFile>, IInitializer<Po>
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
        /// Inserts the translated strings from Po file in a <see cref="MsgFile"/>.
        /// </summary>
        /// <param name="source">Original <see cref="MsgFile"/>.</param>
        /// <returns>Translated <see cref="MsgFile"/>.</returns>
        public MsgFile Convert(MsgFile source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (_translation == null)
            {
                throw new InvalidOperationException("Uninitialized");
            }

            MsgFile result = source;

            InsertStrings(result);

            return result;
        }

        private void InsertStrings(MsgFile msg)
        {
            foreach (PoEntry entry in _translation.Entries)
            {
                // Replacing the entries by the index is important, since some subtitles might have empty text
                var listName = entry.Context.Split('_')[0];
                int index = int.Parse(entry.Context.Split('_')[1]);

                if (listName == "speech")
                {
                    msg.TextStrings[index] = entry.Translated.Replace("\\\\", "\\").Replace("\n", "\r\n");
                }
                else if (listName == "talker")
                {
                    msg.TalkerStrings[index] = entry.Translated;
                }
            }
        }
    }
}
