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
    /// Yakuza 2007 bin column type.
    /// </summary>
    public enum Y2007BinColumnType
    {
        /// <summary>
        /// Each entry is a null terminated string.
        /// </summary>
        String = 0,

        /// <summary>
        /// Each entry is a null terminated string, and after all strings is 1 byte per row (index).
        /// </summary>
        StringTable = 1,

        /// <summary>
        /// Each entry is a null terminated string, preceded by a short value (index).
        /// </summary>
        StringIndex = 2,

        /// <summary>
        /// Each entry is a null terminated string. The string represents a number.
        /// </summary>
        Value = 3,

        /// <summary>
        /// Each entry is a null terminated string, preceded by a short value (index). The string represents a number.
        /// </summary>
        ValueIndex = 5,

        /// <summary>
        /// Each entry is 4 bytes.
        /// </summary>
        ScenarioCategory = 6,

        /// <summary>
        /// Each entry is 8 bytes.
        /// </summary>
        ScenarioStatus = 7,
    }
}
