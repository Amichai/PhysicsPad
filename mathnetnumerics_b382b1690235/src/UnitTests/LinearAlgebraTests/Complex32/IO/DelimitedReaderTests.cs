﻿// <copyright file="DelimitedReaderTests.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Complex32.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using LinearAlgebra.Complex32;
    using LinearAlgebra.Complex32.IO;
    using NUnit.Framework;
    using Complex32 = Numerics.Complex32;

    /// <summary>
    /// Delimited reader tests.
    /// </summary>
    [TestFixture]
    public class DelimitedReaderTests
    {
        /// <summary>
        /// Can parse comma delimited data.
        /// </summary>
        [Test]
        public void CanParseCommaDelimitedData()
        {
            var data = "a,b,c" + Environment.NewLine
                       + "(1,2)" + Environment.NewLine
                       + "\"2.2\",0.3e1" + Environment.NewLine
                       + "'(4,-5)',5,6" + Environment.NewLine;

            var reader = new DelimitedReader<DenseMatrix>(',')
                         {
                             HasHeaderRow = true, 
                             CultureInfo = CultureInfo.InvariantCulture
                         };

            var matrix = reader.ReadMatrix(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            Assert.AreEqual(3, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
            Assert.AreEqual(1.0f, matrix[0, 0].Real);
            Assert.AreEqual(2.0f, matrix[0, 0].Imaginary);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 2]);
            Assert.AreEqual((Complex32)2.2f, matrix[1, 0]);
            Assert.AreEqual((Complex32)3.0f, matrix[1, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[1, 2]);
            Assert.AreEqual(4.0f, matrix[2, 0].Real);
            Assert.AreEqual(-5.0f, matrix[2, 0].Imaginary);
            Assert.AreEqual((Complex32)5.0f, matrix[2, 1]);
            Assert.AreEqual((Complex32)6.0f, matrix[2, 2]);
        }

        /// <summary>
        /// Can parse tab delimited data.
        /// </summary>
        [Test]
        public void CanParseTabDelimitedData()
        {
            var data = "1" + Environment.NewLine
                       + "\"2.2\"\t\t0.3e1" + Environment.NewLine
                       + "'4'\t5\t6";

            var reader = new DelimitedReader<SparseMatrix>('\t')
                         {
                             CultureInfo = CultureInfo.InvariantCulture
                         };

            var matrix = reader.ReadMatrix(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            Assert.AreEqual(3, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
            Assert.AreEqual((Complex32)1.0f, matrix[0, 0]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 2]);
            Assert.AreEqual((Complex32)2.2f, matrix[1, 0]);
            Assert.AreEqual((Complex32)3.0f, matrix[1, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[1, 2]);
            Assert.AreEqual((Complex32)4.0f, matrix[2, 0]);
            Assert.AreEqual((Complex32)5.0f, matrix[2, 1]);
            Assert.AreEqual((Complex32)6.0f, matrix[2, 2]);
        }

        /// <summary>
        /// Can parse white space delimited data.
        /// </summary>
        [Test]
        public void CanParseWhiteSpaceDelimitedData()
        {
            var data = "1" + Environment.NewLine
                       + "\"(2.2,3.3)\" 0.3e1" + Environment.NewLine
                       + "'4'   5      6" + Environment.NewLine;

            var reader = new DelimitedReader<UserDefinedMatrix>
                         {
                             CultureInfo = CultureInfo.InvariantCulture
                         };

            var matrix = reader.ReadMatrix(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            Assert.AreEqual(3, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
            Assert.AreEqual((Complex32)1.0f, matrix[0, 0]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 2]);
            Assert.AreEqual(2.2f, matrix[1, 0].Real);
            Assert.AreEqual(3.3f, matrix[1, 0].Imaginary);
            Assert.AreEqual((Complex32)3.0f, matrix[1, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[1, 2]);
            Assert.AreEqual((Complex32)4.0f, matrix[2, 0]);
            Assert.AreEqual((Complex32)5.0f, matrix[2, 1]);
            Assert.AreEqual((Complex32)6.0f, matrix[2, 2]);
        }

        /// <summary>
        /// Can parse period delimited data.
        /// </summary>
        [Test]
        public void CanParsePeriodDelimitedData()
        {
            var data = "a.b.c" + Environment.NewLine
                       + "1" + Environment.NewLine
                       + "\"2,2\".0,3e1+0,2e1i" + Environment.NewLine
                       + "'4,0'.5,0.6,0" + Environment.NewLine;

            var reader = new DelimitedReader<DenseMatrix>('.')
                         {
                             HasHeaderRow = true, 
                             CultureInfo = new CultureInfo("tr-TR")
                         };

            var matrix = reader.ReadMatrix(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            Assert.AreEqual(3, matrix.RowCount);
            Assert.AreEqual(3, matrix.ColumnCount);
            Assert.AreEqual((Complex32)1.0f, matrix[0, 0]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 1]);
            Assert.AreEqual((Complex32)0.0f, matrix[0, 2]);
            Assert.AreEqual((Complex32)2.2f, matrix[1, 0]);
            Assert.AreEqual(3.0f, matrix[1, 1].Real);
            Assert.AreEqual(2.0f, matrix[1, 1].Imaginary);
            Assert.AreEqual((Complex32)0.0f, matrix[1, 2]);
            Assert.AreEqual((Complex32)4.0f, matrix[2, 0]);
            Assert.AreEqual((Complex32)5.0f, matrix[2, 1]);
            Assert.AreEqual((Complex32)6.0f, matrix[2, 2]);
        }
    }
}
