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
namespace TF3.YarhlPlugin.YakuzaKenzan.Enums
{
    /// <summary>
    /// MsgFile property type.
    /// </summary>
    public enum MsgPropType
    {
        /// <summary>
        /// Contains number of characters printed (string length in characters).
        /// </summary>
        LengthProp = 0x0100,

        /// <summary>
        /// Contains number of characters printed before a question with choices is displayed.
        /// <remarks>Seems to have a similar function to <see cref="LengthProp"/>.</remarks>
        /// </summary>
        LengthPromptProp = 0x010E,

        /// <summary>
        /// Contains the count of characters that should be printed.
        /// </summary>
        PrintProp = 0x0201,

        /// <summary>
        /// Contains the index of the talker name (to show above the text box).
        /// </summary>
        TalkerProp = 0x020B,
    }
}
