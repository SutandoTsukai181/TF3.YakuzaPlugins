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
    using TF3.YarhlPlugin.YakuzaKenzan.Converters.GeneralBin;
    using TF3.YarhlPlugin.YakuzaKenzan.Formats;
    using TF3.YarhlPlugin.YakuzaKenzan.Types.GeneralBin;
    using Yarhl.IO;
    using Yarhl.Media.Text;

    [TestFixture]
    public class GeneralBinTests
    {
        // item/shop/wanted.bin
        private readonly byte[] _data =
        {
            0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x5c, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x63, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0xab, 0x00, 0x00, 0x00, 0xcc,
            0x00, 0x1f, 0x00, 0x01, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x03, 0xe8, 0x00, 0x00, 0x00, 0xeb,
            0x00, 0x20, 0x00, 0x02, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x03, 0xe8, 0x00, 0x00, 0x00, 0xf6,
            0x00, 0x21, 0x00, 0x03, 0x00, 0x00, 0x00, 0x56, 0x00, 0x00, 0x03, 0xe8, 0x00, 0x00, 0x01, 0x01,
            0x00, 0x1e, 0x00, 0x1f, 0x00, 0x01, 0x00, 0x20, 0x00, 0x01, 0x00, 0x01, 0x82, 0xe2, 0x82, 0xdf,
            0x82, 0xe9, 0x00, 0x81, 0x69, 0x8f, 0x8a, 0x8e, 0x9d, 0x8b, 0xe0, 0x82, 0xaa, 0x91, 0xab, 0x82,
            0xe8, 0x82, 0xc8, 0x82, 0xa2, 0x82, 0xe6, 0x82, 0xa4, 0x82, 0xbe, 0x82, 0xc8, 0x81, 0x63, 0x81,
            0x42, 0x81, 0x6a, 0x00, 0x81, 0x69, 0x92, 0x62, 0x82, 0xa6, 0x82, 0xe7, 0x82, 0xea, 0x82, 0xe9,
            0x95, 0x90, 0x8a, 0xed, 0x82, 0xf0, 0x8e, 0x9d, 0x82, 0xc1, 0x82, 0xc4, 0x82, 0xa2, 0x82, 0xc8,
            0x82, 0xa2, 0x82, 0xc8, 0x81, 0x63, 0x81, 0x42, 0x81, 0x6a, 0x00, 0x82, 0xbb, 0x82, 0xa2, 0x82,
            0xc2, 0x82, 0xcd, 0x82, 0xbf, 0x82, 0xe5, 0x82, 0xc1, 0x82, 0xc6, 0x92, 0x62, 0x82, 0xa6, 0x82,
            0xe7, 0x82, 0xea, 0x82, 0xc8, 0x82, 0xa2, 0x82, 0xc8, 0x81, 0x42, 0x00, 0x82, 0xcd, 0x82, 0xa2,
            0x81, 0x41, 0x82, 0xa8, 0x82, 0xa8, 0x82, 0xab, 0x82, 0xc9, 0x81, 0x42, 0x25, 0x73, 0x82, 0xc9,
            0x82, 0xc8, 0x82, 0xe8, 0x82, 0xdc, 0x82, 0xb7, 0x81, 0x42, 0x00, 0x8b, 0xca, 0x8d, 0x7c, 0x82,
            0xcc, 0x93, 0x81, 0x82, 0x51, 0x00, 0x8b, 0xca, 0x8d, 0x7c, 0x82, 0xcc, 0x93, 0x81, 0x82, 0x52,
            0x00, 0x8b, 0xca, 0x8d, 0x7c, 0x82, 0xcc, 0x93, 0x81, 0x82, 0x53, 0x00,
        };

        [Test]
        public void NullSourceThrowsException()
        {
            var converter = new Reader();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void ReadGeneralBin()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var converter = new Reader();
            var parameters = new ReaderParameters
            {
                BinTypeName = typeof(WantedBin).FullName,
            };

            converter.Initialize(parameters);
            GeneralBin bin = converter.Convert(binary);

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.StringOffsets.Count, bin.HeaderStrings.Count + bin.StructStrings.Count);

            Assert.AreEqual(5, bin.HeaderStrings.Count);
            Assert.AreEqual("やめる", bin.HeaderStrings[0]);

            Assert.AreEqual(3, bin.StructStrings.Count);
            Assert.AreEqual("玉鋼の刀２", bin.StructStrings[0]);
        }

        [Test]
        public void WritingNullSourceThrowsException()
        {
            var converter = new Writer();
            _ = Assert.Throws<ArgumentNullException>(() => converter.Convert(null));
        }

        [Test]
        public void WriteGeneralBin()
        {
            byte[] data = new byte[_data.Length];

            Array.Copy(_data, data, _data.Length);

            using DataStream expected = DataStreamFactory.FromArray(data, 0, data.Length);
            using DataStream ds = DataStreamFactory.FromArray(data, 0, data.Length);
            BinaryFormat binary = new BinaryFormat(ds);

            var reader = new Reader();
            var parameters = new ReaderParameters
            {
                BinTypeName = typeof(WantedBin).FullName,
            };

            reader.Initialize(parameters);
            GeneralBin bin = reader.Convert(binary);

            var writer = new Writer();
            BinaryFormat result = writer.Convert(bin);

            var dr = new DataReader(result.Stream)
            {
                Endianness = EndiannessMode.BigEndian,
            };

            result.Stream.Seek(0);
            var header = dr.ReadByType(typeof(WantedBin));

            Assert.AreEqual(3, header.StructCount);
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
            var parameters = new ReaderParameters
            {
                BinTypeName = typeof(WantedBin).FullName,
            };

            reader.Initialize(parameters);
            GeneralBin bin = reader.Convert(binary);

            var poWriter = new ExtractStrings();
            Po po = poWriter.Convert(bin);

            po.Entries[0].Translated = "Translation test";
            var translator = new Translate();
            translator.Initialize(po);

            GeneralBin binResult = translator.Convert(bin);

            Assert.AreEqual("Translation test", binResult.HeaderStrings[0]);
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
            var parameters = new ReaderParameters
            {
                BinTypeName = typeof(WantedBin).FullName,
            };

            reader.Initialize(parameters);
            GeneralBin bin = reader.Convert(binary);

            var translator = new Translate();
            _ = Assert.Throws<InvalidOperationException>(() => translator.Convert(bin));
        }
    }
}
