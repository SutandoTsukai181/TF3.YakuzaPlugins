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
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Serializes Yakuza Kenzan AuthBin files.
    /// </summary>
    public class Writer : IConverter<AuthBin, BinaryFormat>
    {
        /// <summary>
        /// Converts a <see cref="AuthBin"/> into a Binary Format.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The binary format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public BinaryFormat Convert(AuthBin source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            DataStream stream = DataStreamFactory.FromMemory();

            var reader = new DataReader(stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            var writer = new DataWriter(stream)
            {
                DefaultEncoding = Encoding.GetEncoding(932),
                Endianness = EndiannessMode.BigEndian,
            };

            // Add file buffer length as the last offset to be used when splitting the buffer
            source.NodeHeaderOffsets.Add((uint)source.FileBuffer.Length);

            byte[] section = new byte[source.NodeHeaderOffsets[0]];
            Array.Copy(source.FileBuffer, section, source.NodeHeaderOffsets[0]);
            writer.Write(section);

            for (int i = 0; i < source.NodeHeaders.Count; i++)
            {
                var nodeHeader = source.NodeHeaders[i];
                var oldSize = nodeHeader.NodeDataSize + 0x20;   // Size of node header

                nodeHeader.NodeCount = (uint)nodeHeader.SubtitleNodes.Count;
                nodeHeader.NodeDataSize = nodeHeader.NodeCount * 0x110; // Size of AuthSubtitleNode

                // Always seek to end of stream before writing node header
                writer.Stream.Seek(writer.Stream.Length);
                writer.WriteOfType(nodeHeader);
                foreach (var subtitleNode in nodeHeader.SubtitleNodes)
                {
                    writer.WriteOfType(subtitleNode);
                }

                var nextSectionStart = source.NodeHeaderOffsets[i] + oldSize;
                var nextSectionSize = source.NodeHeaderOffsets[i + 1] - nextSectionStart;

                section = new byte[nextSectionSize];
                Array.Copy(source.FileBuffer, nextSectionStart, section, 0, nextSectionSize);
                writer.Write(section);

                // Update all file offsets after each node header written
                int sizeDifference = (int)nodeHeader.NodeDataSize + 0x20 - (int)oldSize;
                UpdateFileOffsets(reader, writer, source.NodeHeaderOffsets[i], sizeDifference);
            }

            return new BinaryFormat(stream);
        }

        private static void UpdateFileOffsets(DataReader reader, DataWriter writer, uint nodeOffset, int sizeDifference)
        {
            // Update file size
            reader.Stream.Seek(0x10);
            var fileSize = reader.ReadUInt32();
            writer.Stream.RunInPosition(() => writer.Write((uint)(fileSize + sizeDifference)), 0x10);

            // Update offsets in file header
            var authHeaderOffsets = new uint[] { 0x14, 0x1C, 0x20, 0x30 };
            foreach (var offset in authHeaderOffsets)
            {
                reader.Stream.Seek(offset);
                var offsetVal = reader.ReadUInt32();
                if (offsetVal > nodeOffset)
                    writer.Stream.RunInPosition(() => writer.Write((uint)(offsetVal + sizeDifference)), offset);
            }

            reader.Stream.Seek(0x1C);
            var nodeStart = reader.ReadUInt32();

            reader.Stream.Seek(nodeStart);
            UpdateNodeOffsets(reader, writer, nodeOffset, sizeDifference);
        }

        private static void UpdateNodeOffsets(DataReader reader, DataWriter writer, uint nodeOffset, int sizeDifference)
        {
            var nodeStart = reader.Stream.Position;

            // Update offsets in node
            var authNodeOffsets = new uint[] { 0x60, 0x68, 0x6C, 0x70 };
            foreach (var offset in authNodeOffsets)
            {
                reader.Stream.Seek(nodeStart + offset);
                var offsetVal = reader.ReadUInt32();
                if (offsetVal > nodeOffset)
                    writer.Stream.RunInPosition(() => writer.Write((uint)(offsetVal + sizeDifference)), nodeStart + offset);
            }

            // Go to all subsequent nodes and update their offsets
            reader.Stream.Seek(nodeStart + 0x6C);
            var childNodeOffset = reader.ReadUInt32();
            var nextNodeOffset = reader.ReadUInt32();

            if (childNodeOffset != 0)
                reader.Stream.RunInPosition(() => UpdateNodeOffsets(reader, writer, nodeOffset, sizeDifference), childNodeOffset);

            if (nextNodeOffset != 0)
                reader.Stream.RunInPosition(() => UpdateNodeOffsets(reader, writer, nodeOffset, sizeDifference), nextNodeOffset);
        }
    }
}
