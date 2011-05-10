﻿// <copyright file="ExponentialTests.cs" company="Math.NET">
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
    /// Exponential distribution tests.
    /// </summary>
    [TestFixture]
    public class ExponentialTests
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
        /// Can create exponential.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void CanCreateExponential([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(lambda, n.Lambda);
        }

        /// <summary>
        /// Exponential create fails with bad parameter.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ExponentialCreateFailsWithBadParameters([Values(Double.NaN, -1.0, -10.0)] double lambda)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Exponential(lambda));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var n = new Exponential(2.0);
            Assert.AreEqual("Exponential(Lambda = 2)", n.ToString());
        }

        /// <summary>
        /// Can set lambda.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void CanSetLambda([Values(-0.0, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            new Exponential(1.0)
            {
                Lambda = lambda
            };
        }

        /// <summary>
        /// Set lambda fails with negative lambda.
        /// </summary>
        [Test]
        public void SetLambdaFailsWithNegativeLambda()
        {
            var n = new Exponential(1.0);
            Assert.Throws<ArgumentOutOfRangeException>(() => n.Lambda = -1.0);
        }

        /// <summary>
        /// Validate mean.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateMean([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(1.0 / lambda, n.Mean);
        }

        /// <summary>
        /// Validate variance.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateVariance([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(1.0 / (lambda * lambda), n.Variance);
        }

        /// <summary>
        /// Validate standard deviation.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateStdDev([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(1.0 / lambda, n.StdDev);
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateEntropy([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(1.0 - Math.Log(lambda), n.Entropy);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateSkewness([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(2.0, n.Skewness);
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateMode([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(0.0, n.Mode);
        }

        /// <summary>
        /// Validate median.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        [Test]
        public void ValidateMedian([Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(Math.Log(2.0) / lambda, n.Median);
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        [Test]
        public void ValidateMinimum()
        {
            var n = new Exponential(1.0);
            Assert.AreEqual(0.0, n.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        [Test]
        public void ValidateMaximum()
        {
            var n = new Exponential(1.0);
            Assert.AreEqual(Double.PositiveInfinity, n.Maximum);
        }

        /// <summary>
        /// Validate density.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Sequential]
        public void ValidateDensity(
            [Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.1, 0.1, 0.1, 0.1, 1.0, 1.0, 1.0, 1.0, 1.0, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity)] double x)
        {
            var n = new Exponential(lambda);
            if (x >= 0)
            {
                Assert.AreEqual(lambda * Math.Exp(-lambda * x), n.Density(x));
            }
            else
            {
                Assert.AreEqual(0.0, n.Density(lambda));
            }
        }

        /// <summary>
        /// Validate density log.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Sequential]
        public void ValidateDensityLn(
            [Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.1, 0.1, 0.1, 0.1, 1.0, 1.0, 1.0, 1.0, 1.0, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity)] double x)
        {
            var n = new Exponential(lambda);
            Assert.AreEqual(Math.Log(lambda) - (lambda * x), n.DensityLn(x));
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new Exponential(1.0);
            n.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var n = new Exponential(1.0);
            var ied = n.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="lambda">Lambda value.</param>
        /// <param name="x">Input X value.</param>
        [Test, Sequential]
        public void ValidateCumulativeDistribution(
            [Values(0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity, 0.0, 0.1, 1.0, 10.0, Double.PositiveInfinity)] double lambda, 
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.1, 0.1, 0.1, 0.1, 1.0, 1.0, 1.0, 1.0, 1.0, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity)] double x)
        {
            var n = new Exponential(lambda);
            if (x >= 0.0)
            {
                Assert.AreEqual(1.0 - Math.Exp(-lambda * x), n.CumulativeDistribution(x));
            }
            else
            {
                Assert.AreEqual(0.0, n.CumulativeDistribution(x));
            }
        }
    }
}
