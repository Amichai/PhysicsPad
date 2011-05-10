﻿// <copyright file="ComplexTest.TextHandling.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.ComplexTests
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using NUnit.Framework;

    /// <summary>
    /// Complex text handling tests
    /// </summary>
    [TestFixture]
    public class ComplexTextHandlingTest
    {
        /// <summary>
        /// Can parse string to complex with a culture.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <param name="expectedReal">Expected real part.</param>
        /// <param name="expectedImaginary">Expected imaginary part.</param>
        /// <param name="cultureName">Culture ID.</param>
        [Test, Sequential]
        public void CanParseStringToComplexWithCulture(
            [Values("-1 -2i", "-1 - 2i ")] string text, 
            [Values(-1, -1)] double expectedReal, 
            [Values(-2, -2)] double expectedImaginary, 
            [Values("en-US", "de-CH")] string cultureName)
        {
            var parsed = text.ToComplex(CultureInfo.GetCultureInfo(cultureName));
            Assert.AreEqual(expectedReal, parsed.Real);
            Assert.AreEqual(expectedImaginary, parsed.Imaginary);
        }

        /// <summary>
        /// Can try parse string to complex with invariant.
        /// </summary>
        /// <param name="str">String to parse.</param>
        /// <param name="expectedReal">Expected real part.</param>
        /// <param name="expectedImaginary">Expected imaginary part.</param>
        [Test, Sequential]
        public void CanTryParseStringToComplexWithInvariant(
            [Values("1", "-1", "-i", "i", "2i", "1 + 2i", "1+2i", "1 - 2i", "1-2i", "1,2 ", "1 , 2", "1,2i", "-1, -2i", " - 1 , - 2 i ", "(+1,2i)", "(-1 , -2)", "(-1 , -2i)", "(+1e1 , -2e-2i)", "(-1E1 -2e2i)", "(-1e+1 -2e2i)", "(-1e1 -2e+2i)", "(-1e-1  -2E2i)", "(-1e1  -2e-2i)", "(-1E+1 -2e+2i)", "(-1e-1,-2e-2i)", "(+1 +2i)", "(-1E+1 -2e+2i)", "(-1e-1,-2e-2i)")] string str, 
            [Values(1, -1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, -1, -1, 10, -10, -10, -10, -0.1, -10, -10, -0.1, 1, -10, -0.1)] double expectedReal, 
            [Values(0, 0, -1, 1, 2, 2, 2, -2, -2, 2, 2, 2, -2, -2, 2, -2, -2, -0.02, -200, -200, -200, -200, -0.02, -200, -0.02, 2, -200, -0.02)] double expectedImaginary)
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            Complex z;
            var ret = str.TryToComplex(invariantCulture, out z);
            Assert.IsTrue(ret);
            Assert.AreEqual(expectedReal, z.Real);
            Assert.AreEqual(expectedImaginary, z.Imaginary);
        }

        /// <summary>
        /// If missing closing paren parse throws <c>FormatException</c>.
        /// </summary>
        [Test]
        public void IfMissingClosingParenParseThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => "(1,2".ToComplex());
        }

        /// <summary>
        /// Try parse can handle symbols.
        /// </summary>
        [Test]
        public void TryParseCanHandleSymbols()
        {
            Complex z;
            var ni = NumberFormatInfo.CurrentInfo;
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            var symbol = ni.NegativeInfinitySymbol + separator + ni.PositiveInfinitySymbol;
            var ret = symbol.TryToComplex(out z);
            Assert.IsTrue(ret, "A1");
            Assert.AreEqual(double.NegativeInfinity, z.Real, "A2");
            Assert.AreEqual(double.PositiveInfinity, z.Imaginary, "A3");

            symbol = ni.NaNSymbol + separator + ni.NaNSymbol;
            ret = symbol.TryToComplex(out z);
            Assert.IsTrue(ret, "B1");
            Assert.AreEqual(double.NaN, z.Real, "B2");
            Assert.AreEqual(double.NaN, z.Imaginary, "B3");

            symbol = ni.NegativeInfinitySymbol + "+" + ni.PositiveInfinitySymbol + "i";
            ret = symbol.TryToComplex(out z);
            Assert.IsTrue(ret, "C1");
            Assert.AreEqual(double.NegativeInfinity, z.Real, "C2");
            Assert.AreEqual(double.PositiveInfinity, z.Imaginary, "C3");

            symbol = ni.NaNSymbol + "+" + ni.NaNSymbol + "i";
            ret = symbol.TryToComplex(out z);
            Assert.IsTrue(ret, "D1");
            Assert.AreEqual(double.NaN, z.Real, "D2");
            Assert.AreEqual(double.NaN, z.Imaginary, "D3");

            symbol = double.MaxValue.ToString("R") + " " + double.MinValue.ToString("R") + "i";
            ret = symbol.TryToComplex(out z);
            Assert.IsTrue(ret, "E1");
            Assert.AreEqual(double.MaxValue, z.Real, "E2");
            Assert.AreEqual(double.MinValue, z.Imaginary, "E3");
        }

        /// <summary>
        /// Try parse can handle symbols with a culture.
        /// </summary>
        /// <param name="cultureName">Culture ID.</param>
        [Test]
        public void TryParseCanHandleSymbolsWithCulture([Values("en-US", "tr-TR", "de-DE", "de-CH", "he-IL")] string cultureName)
        {
            Complex z;
            var culture = CultureInfo.GetCultureInfo(cultureName);
            var ni = culture.NumberFormat;
            var separator = culture.TextInfo.ListSeparator;

            var symbol = ni.NegativeInfinitySymbol + separator + ni.PositiveInfinitySymbol;
            var ret = symbol.TryToComplex(culture, out z);
            Assert.IsTrue(ret, "A1");
            Assert.AreEqual(double.NegativeInfinity, z.Real, "A2");
            Assert.AreEqual(double.PositiveInfinity, z.Imaginary, "A3");

            symbol = ni.NaNSymbol + separator + ni.NaNSymbol;
            ret = symbol.TryToComplex(culture, out z);
            Assert.IsTrue(ret, "B1");
            Assert.AreEqual(double.NaN, z.Real, "B2");
            Assert.AreEqual(double.NaN, z.Imaginary, "B3");

            symbol = ni.NegativeInfinitySymbol + "+" + ni.PositiveInfinitySymbol + "i";
            ret = symbol.TryToComplex(culture, out z);
            Assert.IsTrue(ret, "C1");
            Assert.AreEqual(double.NegativeInfinity, z.Real, "C2");
            Assert.AreEqual(double.PositiveInfinity, z.Imaginary, "C3");

            symbol = ni.NaNSymbol + "+" + ni.NaNSymbol + "i";
            ret = symbol.TryToComplex(culture, out z);
            Assert.IsTrue(ret, "D1");
            Assert.AreEqual(double.NaN, z.Real, "D2");
            Assert.AreEqual(double.NaN, z.Imaginary, "D3");

            symbol = double.MaxValue.ToString("R", culture) + " " + double.MinValue.ToString("R", culture) + "i";
            ret = symbol.TryToComplex(culture, out z);
            Assert.IsTrue(ret, "E1");
            Assert.AreEqual(double.MaxValue, z.Real, "E2");
            Assert.AreEqual(double.MinValue, z.Imaginary, "E3");
        }

        /// <summary>
        /// Try parse returns <c>false</c> when given bad value with invariant.
        /// </summary>
        /// <param name="str">String to parse.</param>
        [Test]
        public void TryParseReturnsFalseWhenGivenBadValueWithInvariant([Values("", "+", "1-", "i+", "1/2i", "1i+2i", "i1i", "(1i,2)", "1e+", "1e", "1,", ",1", null, "()", "(  )")] string str)
        {
            Complex z;
            var ret = str.TryToComplex(CultureInfo.InvariantCulture, out z);
            Assert.IsFalse(ret);
            Assert.AreEqual(0, z.Real);
            Assert.AreEqual(0, z.Imaginary);
        }
    }
}
