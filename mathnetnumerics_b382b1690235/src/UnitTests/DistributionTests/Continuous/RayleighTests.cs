﻿// <copyright file="RayleighTests.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.DistributionTests.Continuous
{
    using System;
    using System.Linq;
    using Distributions;
    using NUnit.Framework;

    /// <summary>
    /// Rayleigh distribution tests.
    /// </summary>
    [TestFixture]
    public class RayleighTests
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
        /// Can create Rayleigh
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void CanCreateRayleigh([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(scale, n.Scale);
        }

        /// <summary>
        /// Rayleigh create fails with bad parameters.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void RayleighCreateFailsWithBadParameters([Values(Double.NaN, Double.NegativeInfinity, -1.0, 0.0)] double scale)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rayleigh(scale));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var n = new Rayleigh(2.0);
            Assert.AreEqual("Rayleigh(Scale = 2)", n.ToString());
        }

        /// <summary>
        /// Can set scale.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void CanSetScale([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            new Rayleigh(1.0)
            {
                Scale = scale
            };
        }

        /// <summary>
        /// Set scale fails with negative scale.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void SetScaleFailsWithNegativeScale([Values(-0.0, 0.0, -1.0, Double.NegativeInfinity, Double.NaN)] double scale)
        {
            var n = new Rayleigh(1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Scale = scale);
        }

        /// <summary>
        /// Validate mean.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateMean([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(scale * Math.Sqrt(Constants.PiOver2), n.Mean);
        }

        /// <summary>
        /// Validate variance.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateVariance([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual((2.0 - Constants.PiOver2) * scale * scale, n.Variance);
        }

        /// <summary>
        /// Validate standard deviation.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateStdDev([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(Math.Sqrt(2.0 - Constants.PiOver2) * scale, n.StdDev);
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateEntropy([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(1.0 + Math.Log(scale / Math.Sqrt(2)) + (Constants.EulerMascheroni / 2.0), n.Entropy);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <param name="skn">Expected value.</param>
        [Test, Combinatorial]
        public void ValidateSkewness([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale, [Values(0.63111065781893638)] double skn)
        {
            var n = new Rayleigh(scale);
            AssertHelpers.AlmostEqual(skn, n.Skewness, 17);
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateMode([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(scale, n.Mode);
        }

        /// <summary>
        /// Validate median.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateMedian([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(scale * Math.Sqrt(Math.Log(4.0)), n.Median);
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateMinimum([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(0.0, n.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void ValidateMaximum([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            var n = new Rayleigh(1.0);
            Assert.AreEqual(Double.PositiveInfinity, n.Maximum);
        }

        /// <summary>
        /// Validate density.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateDensity([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale, [Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double x)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual((x / (scale * scale)) * Math.Exp(-x * x / (2.0 * scale * scale)), n.Density(x));
        }

        /// <summary>
        /// Validate density log.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateDensityLn([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale, [Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double x)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(Math.Log(x / (scale * scale)) - (x * (x / (2.0 * (scale * scale)))), n.DensityLn(x));
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new Rayleigh(1.0);
            n.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var n = new Rayleigh(1.0);
            var ied = n.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateCumulativeDistribution([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale, [Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double x)
        {
            var n = new Rayleigh(scale);
            Assert.AreEqual(1.0 - Math.Exp(-x * x / (2.0 * scale * scale)), n.CumulativeDistribution(x));
        }
    }
}
