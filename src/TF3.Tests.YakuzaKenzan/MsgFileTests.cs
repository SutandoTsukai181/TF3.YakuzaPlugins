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
namespace TF3.Tests.YakuzaKenzan
{
    using System;
    using NUnit.Framework;
    using TF3.YarhlPlugin.YakuzaKenzan.Converters.MsgFile;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using Yarhl.IO;
    using Yarhl.Media.Text;

    [TestFixture]
    public class MsgFileTests
    {
        // wdr/msg/uid00b0009d.msg
        private readonly byte[] _data =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x57, 0x08, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x01, 0x8c,
            0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x60, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x9d, 0x00, 0x77, 0x0f, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x6c, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x8c, 0x00, 0x02, 0x06, 0x64, 0x44, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0xa4, 0x00, 0x01, 0x06, 0x64, 0x44, 0x00, 0x00, 0x10, 0x00, 0x10, 0x05, 0x00,
            0x00, 0x00, 0x01, 0x40, 0x00, 0x00, 0x00, 0xb0, 0x00, 0x36, 0x03, 0x00, 0x00, 0x00, 0x01, 0x51,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x88, 0x00, 0x00, 0x01, 0x30,
            0x02, 0x0b, 0x00, 0xd2, 0xdc, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x02, 0x01, 0x00, 0x0a, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x02, 0x01, 0x00, 0x14, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x02, 0x0b, 0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xb0, 0x00, 0x9d,
            0x02, 0x01, 0x00, 0x14, 0x00, 0x00, 0x00, 0x1b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x82, 0xa8, 0x82, 0xa2, 0x81, 0x41, 0x82, 0xc7, 0x82, 0xa4, 0x82, 0xb5, 0x82, 0xbd, 0x81, 0x48,
            0x00, 0x82, 0xa8, 0x8e, 0x98, 0x82, 0xb3, 0x82, 0xf1, 0x82, 0xaa, 0x97, 0x56, 0x8f, 0x97, 0x82,
            0xc6, 0x90, 0x53, 0x92, 0x86, 0x82, 0xb7, 0x82, 0xe9, 0x82, 0xc1, 0x82, 0xc4, 0x0d, 0x0a, 0x91,
            0x9b, 0x82, 0xa2, 0x82, 0xc5, 0x82, 0xe9, 0x82, 0xdd, 0x82, 0xbd, 0x82, 0xa2, 0x82, 0xc8, 0x82,
            0xf1, 0x82, 0xc5, 0x82, 0xb7, 0x81, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x94,
            0x00, 0x00, 0x01, 0x99, 0x8b, 0xcb, 0x90, 0xb6, 0x00, 0x8c, 0x95, 0x8e, 0x6d, 0x00,
        };

        [Test]
        public void NullSourceThrowsException()
        {
            var converter = new Reader();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void ReadMsgFile()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var converter = new Reader();
            MsgFile bin = converter.Convert(binary);

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.MsgStringOffsets.Count, bin.MsgStrings.Count);

            Assert.AreEqual(3, bin.MsgStrings.Count);
            Assert.AreEqual("おい、どうした？", bin.MsgStrings[0].Text);

            Assert.AreEqual(2, bin.TalkerStrings.Count);
            Assert.AreEqual("桐生", bin.TalkerStrings[0]);
        }

        [Test]
        public void WritingNullSourceThrowsException()
        {
            var converter = new Writer();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void WriteMsgFile()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream expected = DataStreamFactory.FromArray(data, 0, data.Length);
            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var reader = new Reader();
            MsgFile bin = reader.Convert(binary);

            var writer = new Writer();
            BinaryFormat result = writer.Convert(bin);

            Assert.AreEqual(_data.Length, result.Stream.Length);
        }

        [Test]
        public void ReplaceStrings()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream expected = DataStreamFactory.FromArray(data, 0, data.Length);
            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var reader = new Reader();
            MsgFile bin = reader.Convert(binary);

            var poWriter = new ExtractStrings();
            Po po = poWriter.Convert(bin);

            po.Entries[0].Translated = "Translation test";
            var translator = new Translate();
            translator.Initialize(po);

            MsgFile binResult = translator.Convert(bin);

            Assert.AreEqual("Translation test", binResult.MsgStrings[0].Text);
        }

        [Test]
        public void ExtractStringsFromNullThrowsException()
        {
            var converter = new ExtractStrings();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void TranslateNullThrowsException()
        {
            var converter = new Translate();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void UninitializedTranslateThrowsException()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream expected = DataStreamFactory.FromArray(data, 0, data.Length);
            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var reader = new Reader();
            MsgFile bin = reader.Convert(binary);

            var translator = new Translate();
            _ = Assert.Throws<InvalidOperationException>(() => translator.Convert(bin));
        }
    }
}