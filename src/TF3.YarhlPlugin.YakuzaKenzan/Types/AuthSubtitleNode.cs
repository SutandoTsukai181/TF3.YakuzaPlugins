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
    using Yarhl.IO;

    /// <summary>
    /// Auth.bin subtitle node.
    /// </summary>
    [Serializable]
    public class AuthSubtitleNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthSubtitleNode"/> class.
        /// </summary>
        public AuthSubtitleNode()
        {
            StartFrame = 0;
            EndFrame = 0;
            Padding = 0;
            Text = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthSubtitleNode"/> class.
        /// </summary>
        /// <param name="startFrame">Subtitle start frame.</param>
        /// <param name="endFrame">Subtitle end frame.</param>
        /// <param name="text">Subtitle text.</param>
        public AuthSubtitleNode(uint startFrame, uint endFrame, string text)
        {
            StartFrame = startFrame;
            EndFrame = endFrame;
            Padding = 0;
            Text = text;
        }

        /// <summary>
        /// Gets or sets the start frame.
        /// <remarks>Framerate is 30. Divide this number by 30 to get the time in seconds.</remarks>
        /// </summary>
        [BinaryForceEndianness(EndiannessMode.LittleEndian)]
        public uint StartFrame { get; set; }

        /// <summary>
        /// Gets or sets the end frame.
        /// <remarks>Framerate is 30. Divide this number by 30 to get the time in seconds.</remarks>
        /// </summary>
        [BinaryForceEndianness(EndiannessMode.LittleEndian)]
        public uint EndFrame { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// <remarks>Can be ignored.</remarks>
        /// </summary>
        public ulong Padding { get; set; }

        /// <summary>
        /// Gets or sets the subtitle text.
        /// </summary>
        [BinaryString(CodePage = 932, FixedSize = 0x100)]
        public string Text { get; set; }
    }
}
