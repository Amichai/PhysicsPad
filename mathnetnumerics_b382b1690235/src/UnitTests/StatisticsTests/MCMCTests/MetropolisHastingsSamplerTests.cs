﻿// <copyright file="MetropolisHastingsSamplerTests.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.StatisticsTests.McmcTests
{
    using System;
    using Distributions;
    using Numerics.Random;
    using NUnit.Framework;
    using Statistics.Mcmc;

    /// <summary>
    /// Metropolis hastings sampler tests.
    /// </summary>
    [TestFixture]
    public class MetropolisHastingsSamplerTests
    {
        /// <summary>
        /// Metropolis hastings constructor.
        /// </summary>
        [Test]
        public void MetropolisHastingsConstructor()
        {
            var normal = new Normal(0.0, 1.0);
            var rnd = new MersenneTwister();

            var ms = new MetropolisHastingsSampler<double>(0.2, normal.Density, (x, y) => (new Normal(x, 0.1)).Density(y), x => Normal.Sample(rnd, x, 0.1), 10)
                     {
                         RandomSource = rnd
                     };
            Assert.IsNotNull(ms.RandomSource);

            ms.RandomSource = new Random();
            Assert.IsNotNull(ms.RandomSource);
        }

        /// <summary>
        /// Sample test.
        /// </summary>
        [Test]
        public void SampleTest()
        {
            var normal = new Normal(0.0, 1.0);
            var rnd = new MersenneTwister();

            var ms = new MetropolisHastingsSampler<double>(0.2, normal.Density, (x, y) => (new Normal(x, 0.1)).Density(y), x => Normal.Sample(rnd, x, 0.1), 10)
                     {
                         RandomSource = rnd
                     };

            ms.Sample();
        }

        /// <summary>
        /// Sample array test.
        /// </summary>
        [Test]
        public void SampleArrayTest()
        {
            var normal = new Normal(0.0, 1.0);
            var rnd = new MersenneTwister();

            var ms = new MetropolisHastingsSampler<double>(0.2, normal.Density, (x, y) => (new Normal(x, 0.1)).Density(y), x => Normal.Sample(rnd, x, 0.1), 10)
                     {
                         RandomSource = rnd
                     };

            ms.Sample(5);
        }

        /// <summary>
        /// Set <c>null</c> RNG throws <c>ArgumentNullException</c>.
        /// </summary>
        [Test]
        public void NullRandomNumberGenerator()
        {
            var normal = new Normal(0.0, 1.0);
            var ms = new MetropolisHastingsSampler<double>(0.2, normal.Density, (x, y) => (new Normal(x, 0.1)).Density(y), x => Normal.Sample(new Random(), x, 0.1), 10);
            Assert.Throws<ArgumentNullException>(() => ms.RandomSource = null);
        }
    }
}
