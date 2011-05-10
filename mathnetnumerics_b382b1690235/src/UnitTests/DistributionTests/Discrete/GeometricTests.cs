﻿// <copyright file="GeometricTests.cs" company="Math.NET">
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
    /// Geometric distribution tests.
    /// </summary>
    [TestFixture]
    public class GeometricTests
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
        /// Can create Geometric.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void CanCreateGeometric([Values(0.0, 0.3, 1.0)] double p)
        {
            var d = new Geometric(p);
            Assert.AreEqual(p, d.P);
        }

        /// <summary>
        /// Geometric create fails with bad parameters.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void GeometricCreateFailsWithBadParameters([Values(Double.NaN, -1.0, 2.0)] double p)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Geometric(p));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var d = new Geometric(0.3);
            Assert.AreEqual(String.Format("Geometric(P = {0})", d.P), d.ToString());
        }

        /// <summary>
        /// Can set probability of one.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void CanSetProbabilityOfOne([Values(0.0, 0.3, 1.0)] double p)
        {
            new Geometric(0.3)
            {
                P = p
            };
        }

        /// <summary>
        /// Set probability of one with a bad value fails.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void SetProbabilityOfOneFails([Values(Double.NaN, -1.0, 2.0)] double p)
        {
            var d = new Geometric(0.3);
            Assert.Throws<ArgumentOutOfRangeException>(() => d.P = p);
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void ValidateEntropy([Values(0.0, 0.3, 1.0)] double p)
        {
            var d = new Geometric(p);
            Assert.AreEqual(((-p * Math.Log(p, 2.0)) - ((1.0 - p) * Math.Log(1.0 - p, 2.0))) / p, d.Entropy);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void ValidateSkewness([Values(0.0, 0.3, 1.0)] double p)
        {
            var d = new Geometric(p);
            Assert.AreEqual((2.0 - p) / Math.Sqrt(1.0 - p), d.Skewness);
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void ValidateMode([Values(0.0, 0.3, 1.0)] double p)
        {
            var d = new Geometric(p);
            Assert.AreEqual(1, d.Mode);
        }

        /// <summary>
        /// Validate median.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        [Test]
        public void ValidateMedian([Values(0.0, 0.3, 1.0)] double p)
        {
            var d = new Geometric(p);
            Assert.AreEqual((int)Math.Ceiling(-Math.Log(2.0) / Math.Log(1 - p)), d.Median);
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        [Test]
        public void ValidateMinimum()
        {
            var d = new Geometric(0.3);
            Assert.AreEqual(1.0, d.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        [Test]
        public void ValidateMaximum()
        {
            var d = new Geometric(0.3);
            Assert.AreEqual(int.MaxValue, d.Maximum);
        }

        /// <summary>
        /// Validate probability.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateProbability([Values(0.0, 0.3, 1.0)] double p, [Values(-1, 0, 1, 2)] int x)
        {
            var d = new Geometric(p);
            if (x > 0)
            {
                Assert.AreEqual(Math.Pow(1.0 - p, x - 1) * p, d.Probability(x));
            }
            else
            {
                Assert.AreEqual(0.0, d.Probability(x));
            }
        }

        /// <summary>
        /// Validate probability log.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="pln">Expected value.</param>
        [Test, Sequential]
        public void ValidateProbabilityLn(
            [Values(0.0, 0.0, 0.0, 0.0, 0.3, 0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 1.0)] double p, 
            [Values(-1, 0, 1, 2, -1, 0, 1, 2, -1, 0, 1, 2)] int x, 
            [Values(Double.NegativeInfinity, 0.0, Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity, -0.35667494393873244235395440410727451457180907089949815, -1.2039728043259360296301803719337238685164245381839102, Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity, 0.0, Double.NegativeInfinity)] double pln)
        {
            var d = new Geometric(p);
            if (x > 0)
            {
                Assert.AreEqual(((x - 1) * Math.Log(1.0 - p)) + Math.Log(p), d.ProbabilityLn(x));
            }
            else
            {
                Assert.AreEqual(Double.NegativeInfinity, d.ProbabilityLn(x));
            }
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var d = new Geometric(0.3);
            d.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var d = new Geometric(0.3);
            var ied = d.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="p">Probability of generating a one.</param>
        /// <param name="x">Input X value.</param>
        [Test, Combinatorial]
        public void ValidateCumulativeDistribution([Values(0.0, 0.3, 1.0)] double p, [Values(-1, 0, 1, 2)] int x)
        {
            var d = new Geometric(p);
            Assert.AreEqual(1.0 - Math.Pow(1.0 - p, x), d.CumulativeDistribution(x));
        }
    }
}
