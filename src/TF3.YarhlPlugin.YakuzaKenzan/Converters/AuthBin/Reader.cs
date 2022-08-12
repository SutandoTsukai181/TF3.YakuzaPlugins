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
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaKenzan.Enums;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Deserializes AuthBin files.
    /// </summary>
    public class Reader : IConverter<BinaryFormat, AuthBin>
    {
        /// <summary>
        /// Converts a BinaryFormat into a <see cref="AuthBin"/>.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The <see cref="AuthBin"/> format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public AuthBin Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            source.Stream.Position = 0;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var reader = new DataReader(source.Stream)
            {
                DefaultEncoding = Encoding.UTF8,
                Endianness = EndiannessMode.BigEndian,
            };

            // Read the file header
            var header = reader.Read<FileHeader>() as FileHeader;
            CheckHeader(header);

            if (header!.Endianness == Endianness.LittleEndian)
            {
                reader.Endianness = EndiannessMode.LittleEndian;
            }

            reader.Stream.Seek(0x1C);
            var nodeStart = reader.ReadUInt32();
            reader.Stream.Seek(nodeStart);

            List<uint> nodeHeaderOffsets = new List<uint>();
            List<AuthNodeHeader> nodeHeaders = new List<AuthNodeHeader>();
            ReadNode(reader, nodeHeaderOffsets, nodeHeaders);

            reader.Stream.Seek(0);
            byte[] fileBuffer = reader.ReadBytes((int)reader.Stream.Length);

            return new AuthBin(fileBuffer, nodeHeaderOffsets, nodeHeaders);
        }

        private static void CheckHeader(FileHeader header)
        {
            if (header.Magic != "AUTH")
            {
                throw new FormatException($"AUTH: Bad magic Id ({header.Magic} != AUTH)");
            }

            if (header.Version != 0x01000000)
            {
                throw new FormatException($"AUTH: Bad version number ({header.Version} != 0x01000000)");
            }
        }

        private static void ReadNode(DataReader reader, List<uint> nodeHeaderOffsets, List<AuthNodeHeader> nodeHeaders)
        {
            reader.Stream.Seek(0x68, SeekOrigin.Current);

            var nodeHeaderOffset = reader.ReadUInt32();
            var childNodeOffset = reader.ReadUInt32();
            var nextNodeOffset = reader.ReadUInt32();

            if (nodeHeaderOffset != 0)
            {
                reader.Stream.PushToPosition(nodeHeaderOffset);
                while (ReadNodeHeader(reader, nodeHeaderOffsets, nodeHeaders));
                reader.Stream.PopPosition();
            }

            if (childNodeOffset != 0)
                reader.Stream.RunInPosition(() => ReadNode(reader, nodeHeaderOffsets, nodeHeaders), childNodeOffset);

            if (nextNodeOffset != 0)
                reader.Stream.RunInPosition(() => ReadNode(reader, nodeHeaderOffsets, nodeHeaders), nextNodeOffset);
        }

        private static bool ReadNodeHeader(DataReader reader, List<uint> nodeHeaderOffsets, List<AuthNodeHeader> nodeHeaders)
        {
            var nodeHeaderOffset = (uint)reader.Stream.Position;

            // There can be multiple references to a single node header
            if (nodeHeaderOffsets.Contains(nodeHeaderOffset))
                return false;

            var nodeHeader = reader.Read<AuthNodeHeader>() as AuthNodeHeader;

            switch (nodeHeader.NodeType)
            {
                case AuthNodeType.SubtitleNode:
                    nodeHeaderOffsets.Add(nodeHeaderOffset);
                    nodeHeaders.Add(nodeHeader);

                    for (int i = 0; i < nodeHeader.NodeCount; i++)
                    {
                        var subtitleNode = reader.Read<AuthSubtitleNode>() as AuthSubtitleNode;
                        subtitleNode.Text = subtitleNode.Text.TrimEnd('\0');
                        nodeHeader.SubtitleNodes.Add(subtitleNode);
                    }
                    break;
                default:
                    reader.Stream.Seek(nodeHeader.NodeDataSize, SeekOrigin.Current);
                    break;
            }

            return nodeHeader.NodeType != AuthNodeType.EmptyNode;
        }
    }
}
