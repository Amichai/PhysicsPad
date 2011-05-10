// <copyright file="BernoulliTests.cs" company="Math.NET">
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
    /// Bernoulli distribution tests.
    /// </summary>
    [TestFixture]
    public class BernoulliTests
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
        /// Can create Bernoulli.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void CanCreateBernoulli([Values(0.0, 0.3, 1.0)] double p)
        {
            var bernoulli = new Bernoulli(p);
            Assert.AreEqual(p, bernoulli.P);
        }

        /// <summary>
        /// Bernoulli create fails with bad parameters.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void BernoulliCreateFailsWithBadParameters([Values(Double.NaN, -1.0, 2.0)] double p)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bernoulli(p));
        }

        /// <summary>
        /// Validate ToString.
        /// </summary>
        [Test]
        public void ValidateToString()
        {
            var b = new Bernoulli(0.3);
            Assert.AreEqual(String.Format("Bernoulli(P = {0})", b.P), b.ToString());
        }

        /// <summary>
        /// Can set probability of one.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void CanSetProbabilityOfOne([Values(0.0, 0.3, 1.0)] double p)
        {
            new Bernoulli(0.3)
            {
                P = p
            };
        }

        /// <summary>
        /// Set probability of one fails with bad values.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void SetProbabilityOfOneFails([Values(Double.NaN, -1.0, 2.0)] double p)
        {
            var b = new Bernoulli(0.3);
            Assert.Throws<ArgumentOutOfRangeException>(() => b.P = p);
        }

        /// <summary>
        /// Validate entropy.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void ValidateEntropy([Values(0.0, 0.3, 1.0)] double p)
        {
            var b = new Bernoulli(p);
            AssertHelpers.AlmostEqual(-((1.0 - p) * Math.Log(1.0 - p)) - (p * Math.Log(p)), b.Entropy, 14);
        }

        /// <summary>
        /// Validate skewness.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        [Test]
        public void ValidateSkewness([Values(0.0, 0.3, 1.0)] double p)
        {
            var b = new Bernoulli(p);
            Assert.AreEqual((1.0 - (2.0 * p)) / Math.Sqrt(p * (1.0 - p)), b.Skewness);
        }

        /// <summary>
        /// Validate mode.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        /// <param name="m">Expected value.</param>
        [Test, Sequential]
        public void ValidateMode([Values(0.0, 0.3, 1.0)] double p, [Values(0.0, 0.0, 1.0)] double m)
        {
            var b = new Bernoulli(p);
            Assert.AreEqual(m, b.Mode);
        }

        /// <summary>
        /// Validate median throws <c>NotSupportedException</c>.
        /// </summary>
        [Test]
        public void ValidateMedianThrowsNotSupportedException()
        {
            var b = new Bernoulli(0.3);
            Assert.Throws<NotSupportedException>(() => { double m = b.Median; });
        }

        /// <summary>
        /// Validate minimum.
        /// </summary>
        [Test]
        public void ValidateMinimum()
        {
            var b = new Bernoulli(0.3);
            Assert.AreEqual(0.0, b.Minimum);
        }

        /// <summary>
        /// Validate maximum.
        /// </summary>
        [Test]
        public void ValidateMaximum()
        {
            var b = new Bernoulli(0.3);
            Assert.AreEqual(1.0, b.Maximum);
        }

        /// <summary>
        /// Validate probability.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="d">Expected value.</param>
        [Test, Sequential]
        public void ValidateProbability(
            [Values(0.0, 0.0, 0.0, 0.0, 0.3, 0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 1.0)] double p, 
            [Values(-1, 0, 1, 2, -1, 0, 1, 2, -1, 0, 1, 2)] int x, 
            [Values(0.0, 1.0, 0.0, 0.0, 0.0, 0.7, 0.3, 0.0, 0.0, 0.0, 1.0, 0.0)] double d)
        {
            var b = new Bernoulli(p);
            Assert.AreEqual(d, b.Probability(x));
        }

        /// <summary>
        /// Validate probability log.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="dln">Expected value.</param>
        [Test, Sequential]
        public void ValidateProbabilityLn(
            [Values(0.0, 0.0, 0.0, 0.0, 0.3, 0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 1.0)] double p, 
            [Values(-1, 0, 1, 2, -1, 0, 1, 2, -1, 0, 1, 2)] int x, 
            [Values(Double.NegativeInfinity, 0.0, Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity, -0.35667494393873244235395440410727451457180907089949815, -1.2039728043259360296301803719337238685164245381839102, Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity, 0.0, Double.NegativeInfinity)] double dln)
        {
            var b = new Bernoulli(p);
            Assert.AreEqual(dln, b.ProbabilityLn(x));
        }

        /// <summary>
        /// Can sample static.
        /// </summary>
        [Test]
        public void CanSampleStatic()
        {
            Bernoulli.Sample(new Random(), 0.3);
        }

        /// <summary>
        /// Can sample sequence static.
        /// </summary>
        [Test]
        public void CanSampleSequenceStatic()
        {
            var ied = Bernoulli.Samples(new Random(), 0.3);
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Fail sample static with bad values.
        /// </summary>
        [Test]
        public void FailSampleStatic()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Bernoulli.Sample(new Random(), -1.0));
        }

        /// <summary>
        /// Fail sample sequence static with bad values.
        /// </summary>
        [Test]
        public void FailSampleSequenceStatic()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Bernoulli.Samples(new Random(), -1.0).First());
        }

        /// <summary>
        /// Can sample.
        /// </summary>
        [Test]
        public void CanSample()
        {
            var n = new Bernoulli(0.3);
            n.Sample();
        }

        /// <summary>
        /// Can sample sequence.
        /// </summary>
        [Test]
        public void CanSampleSequence()
        {
            var n = new Bernoulli(0.3);
            var ied = n.Samples();
            ied.Take(5).ToArray();
        }

        /// <summary>
        /// Validate cumulative distribution.
        /// </summary>
        /// <param name="p">Probability of one.</param>
        /// <param name="x">Input X value.</param>
        /// <param name="cdf">Expected value.</param>
        [Test, Sequential]
        public void ValidateCumulativeDistribution(
            [Values(0.0, 0.0, 0.0, 0.0, 0.0, 0.3, 0.3, 0.3, 0.3, 0.3, 1.0, 1.0, 1.0, 1.0, 1.0)] double p, 
            [Values(-1.0, 0.0, 0.5, 1.0, 2.0, -1.0, 0.0, 0.5, 1.0, 2.0, -1.0, 0.0, 0.5, 1.0, 2.0)] double x, 
            [Values(0.0, 1.0, 1.0, 1.0, 1.0, 0.0, 0.7, 0.7, 1.0, 1.0, 0.0, 0.0, 0.0, 1.0, 1.0)] double cdf)
        {
            var b = new Bernoulli(p);
            Assert.AreEqual(cdf, b.CumulativeDistribution(x));
        }
    }
}
