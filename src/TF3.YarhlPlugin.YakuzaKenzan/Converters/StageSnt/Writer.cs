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
namespace TF3.YarhlPlugin.YakuzaKenzan.Converters.StageSnt
{
    using System;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Kenzan StageSnt file.
    /// </summary>
    public class Writer : IConverter<StageSnt, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="StageSnt"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(StageSnt source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            DataStream stream = DataStreamFactory.FromMemory();

            var writer = new DataWriter(stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            for (int i = 0; i < source.Indices.Count; i++)
            {
                writer.Write(source.Indices[i]);
                writer.WriteOfType<sbyte>(0);

                var pos = writer.Stream.Position;
                writer.Write(source.Strings[i]);

                var size = writer.Stream.Position - pos - 1;

                writer.Stream.RunInPosition(() => writer.WriteOfType<sbyte>((sbyte)size), pos - 1);
            }

            writer.WriteOfType<sbyte>(-1);
            writer.WriteOfType<sbyte>(0);

            return new BinaryFormat(stream);
        }
    }
}
