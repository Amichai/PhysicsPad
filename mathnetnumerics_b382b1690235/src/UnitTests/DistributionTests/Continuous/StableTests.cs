﻿// <copyright file="StableTests.cs" company="Math.NET">
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
    /// Stable distribution tests.
    /// </summary>
    [TestFixture]
    public class StableTests
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
        /// Can create stable.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        /// <param name="beta">Beta value.</param>
        /// <param name="scale">Scale value.</param>
        /// <param name="location">Location value.</param>
        [Test, Combinatorial]
        public void CanCreateStable([Values(0.1, 2.0)] double alpha, [Values(-1.0, 1.0)] double beta, [Values(0.1, Double.PositiveInfinity)] double scale, [Values(Double.NegativeInfinity, -1.0, 0.0, 1.0, Double.PositiveInfinity)] double location)
        {
            var n = new Stable(alpha, beta, scale, location);
            Assert.AreEqual(alpha, n.Alpha);
            Assert.AreEqual(beta, n.Beta);
            Assert.AreEqual(scale, n.Scale);
            Assert.AreEqual(location, n.Location);
        }

        /// <summary>
        /// Stable create fails with bad parameters.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        /// <param name="beta">Beta value.</param>
        /// <param name="location">Location value.</param>
        /// <param name="scale">Scale value.</param>
        [Test, Sequential]
        public void StableCreateFailsWithBadParameters(
            [Values(Double.NaN, 1.0, Double.NaN, Double.NaN, Double.NaN, 1.0, 1.0, 1.0, Double.NaN, 1.0, 1.0, 1.0, Double.NaN, 1.0, 1.0, 1.0, 0.0, 2.1)] double alpha, 
            [Values(Double.NaN, Double.NaN, 1.0, Double.NaN, Double.NaN, 1.0, Double.NaN, Double.NaN, 1.0, 1.0, 1.0, Double.NaN, 1.0, 1.0, -1.1, 1.1, 1.0, 1.0)] double beta, 
            [Values(Double.NaN, Double.NaN, Double.NaN, 1.0, Double.NaN, Double.NaN, 1.0, Double.NaN, 1.0, 1.0, Double.NaN, 1.0, 1.0, 0.0, 1.0, 1.0, 1.0, 1.0)] double location, 
            [Values(Double.NaN, Double.NaN, Double.NaN, Double.NaN, 1.0, Double.NaN, Double.NaN, 1.0, Double.NaN, Double.NaN, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0)] double scale)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Stable(alpha, beta, location, scale));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var n = new Stable(1.2, 0.3, 1.0, 2.0);
            Assert.AreEqual(String.Format("Stable(Stability = {0}, Skewness = {1}, Scale = {2}, Location = {3})", n.Alpha, n.Beta, n.Scale, n.Location), n.ToString());
        }

        /// <summary>
        /// Can set alpha.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        [Test]
        public void CanSetAlpha([Values(0.1, 1.0, 2.0)] double alpha)
        {
            new Stable(1.0, 1.0, 1.0, 1.0)
            {
                Alpha = alpha
            };
        }

        /// <summary>
        /// Set alpha fails with bad values.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        [Test]
        public void SetAlphaFail([Values(Double.NaN, -0.0, 0.0, 2.1, Double.NegativeInfinity, Double.PositiveInfinity)] double alpha)
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Alpha = alpha);
        }

        /// <summary>
        /// Can set beta.
        /// </summary>
        /// <param name="beta">Beta value.</param>
        [Test]
        public void CanSetBeta([Values(-1.0, -0.1, -0.0, 0.0, 0.1, 1.0)] double beta)
        {
            new Stable(1.0, 1.0, 1.0, 1.0)
            {
                Beta = beta
            };
        }

        /// <summary>
        /// Set beta fails with bad values.
        /// </summary>
        /// <param name="beta">Beta value.</param>
        [Test]
        public void SetBetaFail([Values(Double.NaN, -1.1, 1.1, Double.NegativeInfinity, Double.PositiveInfinity)] double beta)
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Beta = beta);
        }

        /// <summary>
        /// Can set scale.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void CanSetScale([Values(0.1, 1.0, 10.0, Double.PositiveInfinity)] double scale)
        {
            new Stable(1.0, 1.0, 1.0, 1.0)
            {
                Scale = scale
            };
        }

        /// <summary>
        /// Set scale fails with bad values.
        /// </summary>
        /// <param name="scale">Scale value.</param>
        [Test]
        public void SetScaleFail([Values(Double.NaN, 0.0)] double scale)
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Scale = scale);
        }

        /// <summary>
        /// Can set location.
        /// </summary>
        /// <param name="location">Location value.</param>
        [Test]
        public void CanSetLocation([Values(Double.NegativeInfinity, -10.0, -1.0, -0.1, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double location)
        {
            new Stable(1.0, 1.0, 1.0, 1.0)
            {
                Location = location
            };
        }

        /// <summary>
        /// Set location fails with bad values.
        /// </summary>
        [Test]
        public void SetLocationFail()
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Location = Double.NaN);
        }

        /// <summary>
        /// Validate entropy throws <c>NotSupportedException</c>.
        /// </summary>
        [Test]
        public void ValidateEntropyThrowsNotSupportedException()
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.Throws<NotSupportedException>(() => { var e = n.Entropy; });
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        [Test]
        public void ValidateSkewness()
        {
            var n = new Stable(2.0, 1.0, 1.0, 1.0);
            if (n.Alpha == 2)
            {
                Assert.AreEqual(0.0, n.Skewness);
            }
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="location">Location value.</param>
        [Test]
        public void ValidateMode([Values(Double.NegativeInfinity, -10.0, -1.0, -0.1, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double location)
        {
            var n = new Stable(1.0, 0.0, 1.0, location);
            if (n.Beta == 0)
            {
                Assert.AreEqual(location, n.Mode);
            }
        }

        /// <summary>
        /// Validate mean.
        /// </summary>
        /// <param name="location">Location value.</param>
        [Test]
        public void ValidateMedian([Values(Double.NegativeInfinity, -10.0, -1.0, -0.1, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double location)
        {
            var n = new Stable(1.0, 0.0, 1.0, location);
            if (n.Beta == 0)
            {
                Assert.AreEqual(location, n.Mode);
            }
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        /// <param name="beta">Beta value.</param>
        [Test]
        public void ValidateMinimum([Values(-1.0, -0.1, -0.0, 0.0, 0.1, 1.0)] double beta)
        {
            var n = new Stable(1.0, beta, 1.0, 1.0);
            Assert.AreEqual(Math.Abs(beta) != 1 ? Double.NegativeInfinity : 0.0, n.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        [Test]
        public void ValidateMaximum()
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            Assert.AreEqual(Double.PositiveInfinity, n.Maximum);
        }

        /// <summary>
        /// Validate density.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        /// <param name="beta">Beta value.</param>
        /// <param name="scale">Scale value.</param>
        /// <param name="location">Location value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="d">Expected value.</param>
        [Test, Sequential]
        public void ValidateDensity(
            [Values(2.0, 2.0, 2.0, 1.0, 1.0, 1.0, 0.5, 0.5, 0.5)] double alpha, 
            [Values(-1.0, -1.0, -1.0, 0.0, 0.0, 0.0, 1.0, 1.0, 1.0)] double beta, 
            [Values(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0)] double scale, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)] double location, 
            [Values(1.5, 3.0, 5.0, 1.5, 3.0, 5.0, 1.5, 3.0, 5.0)] double x, 
            [Values(0.16073276729880184, 0.029732572305907354, 0.00054457105758817781, 0.097941503441166353, 0.031830988618379068, 0.012242687930145794, 0.15559955475708653, 0.064989885240913717, 0.032286845174307237)] double d)
        {
            var n = new Stable(alpha, beta, scale, location);
            AssertHelpers.AlmostEqual(d, n.Density(x), 15);
        }

        /// <summary>
        /// Validate density log.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        /// <param name="beta">Beta value.</param>
        /// <param name="scale">Scale value.</param>
        /// <param name="location">Location value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="dln">Expected value.</param>
        [Test, Sequential]
        public void ValidateDensityLn(
            [Values(2.0, 2.0, 2.0, 1.0, 1.0, 1.0, 0.5, 0.5, 0.5)] double alpha, 
            [Values(-1.0, -1.0, -1.0, 0.0, 0.0, 0.0, 1.0, 1.0, 1.0)] double beta, 
            [Values(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0)] double scale, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)] double location, 
            [Values(1.5, 3.0, 5.0, 1.5, 3.0, 5.0, 1.5, 3.0, 5.0)] double x, 
            [Values(-1.8280121234846454, -3.5155121234846449, -7.5155121234846449, -2.3233848821910463, -3.4473149788434458, -4.4028264238708825, -1.8604695287002526, -2.7335236328735038, -3.4330954018558235)] double dln)
        {
            var n = new Stable(alpha, beta, scale, location);
            AssertHelpers.AlmostEqual(dln, n.DensityLn(x), 15);
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            n.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var n = new Stable(1.0, 1.0, 1.0, 1.0);
            var ied = n.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="alpha">Alpha value.</param>
        /// <param name="beta">Beta value.</param>
        /// <param name="scale">Scale value.</param>
        /// <param name="location">Location value.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="cdf">Expected value.</param>
        [Test, Sequential]
        public void ValidateCumulativeDistribution(
            [Values(2.0, 2.0, 2.0, 1.0, 1.0, 1.0, 0.5, 0.5, 0.5)] double alpha, 
            [Values(-1.0, -1.0, -1.0, 0.0, 0.0, 0.0, 1.0, 1.0, 1.0)] double beta, 
            [Values(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0)] double scale, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)] double location, 
            [Values(1.5, 3.0, 5.0, 1.5, 3.0, 5.0, 1.5, 3.0, 5.0)] double x, 
            [Values(0.8555778168267576, 0.98305257323765538, 0.9997965239912775, 0.81283295818900125, 0.89758361765043326, 0.93716704181099886, 0.41421617824252516, 0.563702861650773, 0.65472084601857694)] double cdf)
        {
            var n = new Stable(alpha, beta, scale, location);
            AssertHelpers.AlmostEqual(cdf, n.CumulativeDistribution(x), 15);
        }
    }
}
