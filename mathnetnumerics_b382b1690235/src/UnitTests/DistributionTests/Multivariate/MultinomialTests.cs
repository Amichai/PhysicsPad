// <copyright file="MultinomialTests.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.DistributionTests.Multivariate
{
    using System;
    using System.Linq;
    using Distributions;
    using NUnit.Framework;
    using Statistics;

    /// <summary>
    /// Multinomial distribution tests.
    /// </summary>
    [TestFixture]
    public class MultinomialTests
    {
        /// <summary>
        /// Bad proportion of ratios.
        /// </summary>
        private double[] _badP;

        /// <summary>
        /// Another bad proportion of ratios.
        /// </summary>
        private double[] _badP2;

        /// <summary>
        /// Small array with proportion of ratios
        /// </summary>
        private double[] _smallP;

        /// <summary>
        /// Large array with proportion of ratios
        /// </summary>
        private double[] _largeP;

        /// <summary>
        /// Set-up tests parameters.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Control.CheckDistributionParameters = true;
            _badP = new[] { -1.0, 1.0 };
            _badP2 = new[] { 0.0, 0.0 };
            _smallP = new[] { 1.0, 1.0, 1.0 };
            _largeP = new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 };
        }

        /// <summary>
        /// Can create Multinomial
        /// </summary>
        [Test]
        public void CanCreateMultinomial()
        {
            var m = new Multinomial(_largeP, 4);
            CollectionAssert.AreEqual(_largeP, m.P);
        }

        /// <summary>
        /// Can create Multinomial from a histogram.
        /// </summary>
        [Test]
        public void CanCreateMultinomialFromHistogram()
        {
            double[] smallDataset = { 0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5, 9.5 };
            var hist = new Histogram(smallDataset, 10, 0.0, 10.0);
            var m = new Multinomial(hist, 7);

            foreach (var t in m.P)
            {
                Assert.AreEqual(1.0, t);
            }
        }

        /// <summary>
        /// Multinomial create fails with <c>null</c> histogram.
        /// </summary>
        [Test]
        public void MultinomialCreateFailsWithNullHistogram()
        {
            Histogram h = null;
            Assert.Throws<ArgumentNullException>(() => new Categorical(h));
        }

        /// <summary>
        /// Multinomial create fails with negative ratios.
        /// </summary>
        [Test]
        public void MultinomialCreateFailsWithNegativeRatios()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Multinomial(_badP, 4));
        }

        /// <summary>
        /// Multinomial create fails with all zero ratios.
        /// </summary>
        [Test]
        public void MultinomialCreateFailsWithAllZeroRatios()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Multinomial(_badP2, 4));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var b = new Multinomial(_smallP, 4);
            Assert.AreEqual("Multinomial(Dimension = 3, Number of Trails = 4)", b.ToString());
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="p">Proportion of ratios.</param>
        /// <param name="n">Number of trials.</param>
        /// <param name="res">Expected value.</param>
        [Test, Sequential]
        public void ValidateSkewness(
            [Values(new[] { 0.3, 0.7 }, new[] { 0.1, 0.3, 0.6 }, new[] { 0.15, 0.35, 0.3, 0.2 })] double[] p, 
            [Values(5, 10, 20)] int n, 
            [Values(new[] { 0.390360029179413, -0.390360029179413 }, new[] { 0.843274042711568, 0.276026223736942, -0.129099444873581 }, new[] { 0.438357003759605, 0.140642169281549, 0.195180014589707, 0.335410196624968 })] double[] res)
        {
            var b = new Multinomial(p, n);
            for (var i = 0; i < b.P.Length; i++)
            {
                AssertHelpers.AlmostEqual(res[i], b.Skewness[i], 12);
            }
        }

        /// <summary>
        /// Validate variance.
        /// </summary>
        /// <param name="p">Proportion of ratios.</param>
        /// <param name="n">Number of trials.</param>
        /// <param name="res">Expected value.</param>
        [Test, Sequential]
        public void ValidateVariance(
            [Values(new[] { 0.3, 0.7 }, new[] { 0.1, 0.3, 0.6 }, new[] { 0.15, 0.35, 0.3, 0.2 })] double[] p, 
            [Values(5, 10, 20)] int n, 
            [Values(new[] { 1.05, 1.05 }, new[] { 0.9, 2.1, 2.4 }, new[] { 2.55, 4.55, 4.2, 3.2 })] double[] res)
        {
            var b = new Multinomial(p, n);
            for (var i = 0; i < b.P.Length; i++)
            {
                AssertHelpers.AlmostEqual(res[i], b.Variance[i], 12);
            }
        }

        /// <summary>
        /// Validate mean.
        /// </summary>
        /// <param name="p">Proportion of ratios.</param>
        /// <param name="n">Number of trials.</param>
        /// <param name="res">Expected value.</param>
        [Test, Sequential]
        public void ValidateMean(
            [Values(new[] { 0.3, 0.7 }, new[] { 0.1, 0.3, 0.6 }, new[] { 0.15, 0.35, 0.3, 0.2 })] double[] p, 
            [Values(5, 10, 20)] int n, 
            [Values(new[] { 1.5, 3.5 }, new[] { 1.0, 3.0, 6.0 }, new[] { 3.0, 7.0, 6.0, 4.0 })] double[] res)
        {
            var b = new Multinomial(p, n);
            for (var i = 0; i < b.P.Length; i++)
            {
                AssertHelpers.AlmostEqual(res[i], b.Mean[i], 12);
            }
        }

        /// <summary>
        /// Validate probability.
        /// </summary>
        /// <param name="p">Proportion of ratios.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="res">Expected value.</param>
        [Test, Sequential]
        public void ValidateProbability(
            [Values(new[] { 0.3, 0.7 }, new[] { 0.1, 0.3, 0.6 }, new[] { 0.15, 0.35, 0.3, 0.2 })] double[] p, 
            [Values(new[] { 1, 9 }, new[] { 1, 3, 6 }, new[] { 1, 1, 1, 7 })] int[] x, 
            [Values(0.121060821, 0.105815808, 0.000145152)] double res)
        {
            var b = new Multinomial(p, x.Sum());
            AssertHelpers.AlmostEqual(b.Probability(x), res, 12);
        }

        /// <summary>
        /// Validate probability log.
        /// </summary>
        /// <param name="x">Input X value.</param>
        [Test, Sequential]
        public void ValidateProbabilityLn([Values(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 }, new[] { 5, 6, 7, 8, 7, 6, 5, 4, 3 })] int[] x)
        {
            var b = new Multinomial(_largeP, x.Sum());
            AssertHelpers.AlmostEqual(b.ProbabilityLn(x), Math.Log(b.Probability(x)), 12);
        }

        /// <summary>
        /// Can set probability.
        /// </summary>
        [Test]
        public void CanSetProbability()
        {
            new Multinomial(_largeP, 4)
            {
                P = _smallP
            };
        }

        /// <summary>
        /// Set bad values of probability fails.
        /// </summary>
        [Test]
        public void SetProbabilityFails()
        {
            var b = new Multinomial(_largeP, 4);
            Assert.Throws<ArgumentOutOfRangeException>(() => b.P = _badP);
        }

        /// <summary>
        /// Can sample static.
        /// </summary>
        [Test]
        public void CanSampleStatic()
        {
            Multinomial.Sample(new Random(), _largeP, 4);
        }

        /// <summary>
        /// Sample static fails with bad parameters.
        /// </summary>
        [Test]
        public void FailSampleStatic()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Multinomial.Sample(new Random(), _badP, 4));
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new Multinomial(_largeP, 4);
            n.Sample();
        }
    }
}
