﻿// <copyright file="ZipfTests.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.DistributionTests.Discrete
{
    using System;
    using System.Linq;
    using Distributions;
    using NUnit.Framework;

    /// <summary>
    /// Zipf law tests.
    /// </summary>
    [TestFixture]
    public class ZipfTests
    {
        /// <summary>
        /// Set-up parameters.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Control.CheckDistributionParameters = true;
        }

        /// <summary>
        /// Can create zipf.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        [Test, Combinatorial]
        public void CanCreateZipf([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n)
        {
            var d = new Zipf(s, n);
            Assert.AreEqual(s, d.S);
            Assert.AreEqual(n, d.N);
        }

        /// <summary>
        /// Zipf create fails with bad parameters.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        [Test, Combinatorial]
        public void ZipfCreateFailsWithBadParameters([Values(0.0)] double s, [Values(-10, 0)] int n)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Zipf(s, n));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var d = new Zipf(1.0, 5);
            Assert.AreEqual("Zipf(S = 1, N = 5)", d.ToString());
        }

        /// <summary>
        /// Can set S.
        /// </summary>
        /// <param name="s">S parameter.</param>
        [Test]
        public void CanSetS([Values(0.1, 1.0, 5.0)] double s)
        {
            new Zipf(1.0, 5)
            {
                S = s
            };
        }

        /// <summary>
        /// Set S fails with bad values.
        /// </summary>
        /// <param name="s">S parameter.</param>
        [Test]
        public void SetSFails([Values(Double.NaN, -1.0, Double.NegativeInfinity)] double s)
        {
            var d = new Zipf(1.0, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => d.S = s);
        }

        /// <summary>
        /// Can set N.
        /// </summary>
        /// <param name="n">N parameter.</param>
        [Test]
        public void CanSetN([Values(1, 20, 50)] int n)
        {
            new Zipf(1.0, 5)
            {
                N = n
            };
        }

        /// <summary>
        /// Set N fails with bad values.
        /// </summary>
        /// <param name="n">N parameter.</param>
        [Test]
        public void SetNFails([Values(-1, 0)] int n)
        {
            var d = new Zipf(1.0, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => d.N = n);
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        /// <param name="e">Expected value.</param>
        [Test, Sequential]
        public void ValidateEntropy(
            [Values(0.1, 0.1, 0.1, 1.0, 1.0, 1.0)] double s, 
            [Values(1, 20, 50, 1, 20, 50)] int n, 
            [Values(0.0, 2.9924075515295949, 3.9078245132371388, 0.0, 2.5279968533953743, 3.1971263138845916)] double e)
        {
            var d = new Zipf(s, n);
            AssertHelpers.AlmostEqual(e, d.Entropy, 15);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        [Test, Combinatorial]
        public void ValidateSkewness(
            [Values(5.0, 10.0)] double s, 
            [Values(1, 20, 50)] int n)
        {
            var d = new Zipf(s, n);
            if (s > 4)
            {
                Assert.AreEqual(((SpecialFunctions.GeneralHarmonic(n, s - 3) * Math.Pow(SpecialFunctions.GeneralHarmonic(n, s), 2)) - (SpecialFunctions.GeneralHarmonic(n, s - 1) * ((3 * SpecialFunctions.GeneralHarmonic(n, s - 2) * SpecialFunctions.GeneralHarmonic(n, s)) - Math.Pow(SpecialFunctions.GeneralHarmonic(n, s - 1), 2)))) / Math.Pow((SpecialFunctions.GeneralHarmonic(n, s - 2) * SpecialFunctions.GeneralHarmonic(n, s)) - Math.Pow(SpecialFunctions.GeneralHarmonic(n, s - 1), 2), 1.5), d.Skewness);
            }
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        [Test, Combinatorial]
        public void ValidateMode([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n)
        {
            var d = new Zipf(s, n);
            Assert.AreEqual(1, d.Mode);
        }

        /// <summary>
        /// Validate median throws <c>NotSupportedException</c>.
        /// </summary>
        [Test]
        public void ValidateMedianThrowsNotSupportedException()
        {
            var d = new Zipf(1.0, 5);
            Assert.Throws<NotSupportedException>(() => { var m = d.Median; });
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        [Test]
        public void ValidateMinimum()
        {
            var d = new Zipf(1.0, 5);
            Assert.AreEqual(1, d.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        [Test, Combinatorial]
        public void ValidateMaximum([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n)
        {
            var d = new Zipf(s, n);
            Assert.AreEqual(n, d.Maximum);
        }

        /// <summary>
        /// Validate probability.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateProbability([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n, [Values(1, 15)] int x)
        {
            var d = new Zipf(s, n);
            Assert.AreEqual((1.0 / Math.Pow(x, s)) / SpecialFunctions.GeneralHarmonic(n, s), d.Probability(x));
        }

        /// <summary>
        /// Validate probability log.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateProbabilityLn([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n, [Values(1, 15)] int x)
        {
            var d = new Zipf(s, n);
            Assert.AreEqual(Math.Log(d.Probability(x)), d.ProbabilityLn(x));
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var d = new Zipf(0.7, 5);
            var s = d.Sample();
            Assert.LessOrEqual(s, 5);
            Assert.GreaterOrEqual(s, 0);
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var d = new Zipf(0.7, 5);
            var ied = d.Samples();
            var e = ied.Take(1000).ToArray();
            foreach (var i in e)
            {
                Assert.LessOrEqual(i, 5);
                Assert.GreaterOrEqual(i, 0);
            }
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="s">S parameter.</param>
        /// <param name="n">N parameter.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateCumulativeDistribution([Values(0.1, 1)] double s, [Values(1, 20, 50)] int n, [Values(2, 15)] int x)
        {
            var d = new Zipf(s, n);
            var cdf = SpecialFunctions.GeneralHarmonic(x, s) / SpecialFunctions.GeneralHarmonic(n, s);
            AssertHelpers.AlmostEqual(cdf, d.CumulativeDistribution(x), 14);
        }
    }
}
