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
    /// Extracts Yakuza Kenzan MsgFile translatable strings to a Po file.
    /// </summary>
    public class ExtractStrings : IConverter<MsgFile, Po>, IInitializer<PoHeader>
    {
        private PoHeader _poHeader = new ("NoName", "dummy@dummy.com", "en");

        /// <summary>
        /// Converter initializer.
        /// </summary>
        /// <param name="parameters">Header to use in created Po elements.</param>
        public void Initialize(PoHeader parameters) => _poHeader = parameters;

        /// <summary>
        /// Extracts strings to a Po file.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The po file.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public Po Convert(MsgFile source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var po = new Po(_poHeader);

            Extract(source, po);

            return po;
        }

        private void Extract(MsgFile msg, Po po)
        {
            for (int i = 0; i < msg.MsgStrings.Count; i++)
            {
                var msgString = msg.MsgStrings[i];

                if (msgString.Text == string.Empty)
                    continue;

                var context = $"speech_{i}";
                if (msgString.TalkerIndex >= 0)
                    context += $"_{msg.TalkerStrings[msgString.TalkerIndex]}";

                var entry = new PoEntry()
                {
                    Original = msgString.Text.Replace("\\", "\\\\").Replace("\r\n", "\n"),
                    Translated = msgString.Text.Replace("\\", "\\\\").Replace("\r\n", "\n"),
                    Context = context,
                };
                po.Add(entry);
            }

            for (int i = 0; i < msg.TalkerStrings.Count; i++)
            {
                var entry = new PoEntry()
                {
                    Original = msg.TalkerStrings[i],
                    Translated = msg.TalkerStrings[i],
                    Context = $"talker_{i}",
                };
                po.Add(entry);
            }
        }
    }
}