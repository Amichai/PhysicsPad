// <copyright file="NormalGammaTests.cs" company="Math.NET">
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

    /// <summary>
    /// <c>NormalGamma</c> distribution tests.
    /// </summary>
    [TestFixture]
    public class NormalGammaTests
    {
        /// <summary>
        /// Can create <c>NormalGamma</c>.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanCreateNormalGamma([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);

            Assert.AreEqual(meanLocation, ng.MeanLocation);
            Assert.AreEqual(meanScale, ng.MeanScale);
            Assert.AreEqual(precShape, ng.PrecisionShape);
            Assert.AreEqual(precInvScale, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can get density and density log.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Sequential]
        public void CanGetDensityAndDensityLn([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 1.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(ng.DensityLn(meanLocation, precShape), Math.Log(ng.Density(meanLocation, precShape)), 1e-14);
        }

        /// <summary>
        /// <c>NormalGamma</c> constructor fails with invalid params.
        /// </summary>
        [Test]
        public void NormalGammaConstructorFailsWithInvalidParams()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NormalGamma(1.0, -1.3, 2.0, 2.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NormalGamma(1.0, 1.0, -1.0, 1.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new NormalGamma(1.0, 1.0, 1.0, -1.0));
        }

        /// <summary>
        /// Can get mean location.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanGetMeanLocation([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(meanLocation, ng.MeanLocation);
        }

        /// <summary>
        /// Can set mean location.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanSetMeanLocation([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale)
                     {
                         MeanLocation = -5.0
                     };

            Assert.AreEqual(-5.0, ng.MeanLocation);
            Assert.AreEqual(meanScale, ng.MeanScale);
            Assert.AreEqual(precShape, ng.PrecisionShape);
            Assert.AreEqual(precInvScale, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can get mean scale.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanGetMeanScale([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(meanScale, ng.MeanScale);
        }

        /// <summary>
        /// Can set mean scale.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanSetMeanScale([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale)
                     {
                         MeanScale = 5.0
                     };
            Assert.AreEqual(meanLocation, ng.MeanLocation);
            Assert.AreEqual(5.0, ng.MeanScale);
            Assert.AreEqual(precShape, ng.PrecisionShape);
            Assert.AreEqual(precInvScale, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can get precision shape.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanGetPrecisionShape([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(precShape, ng.PrecisionShape);
        }

        /// <summary>
        /// Can set precision shape.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanSetPrecisionShape([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale)
                     {
                         PrecisionShape = 5.0
                     };
            Assert.AreEqual(meanLocation, ng.MeanLocation);
            Assert.AreEqual(meanScale, ng.MeanScale);
            Assert.AreEqual(5.0, ng.PrecisionShape);
            Assert.AreEqual(precInvScale, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can get precision inverse scale.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanGetPrecisionInverseScale([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(precInvScale, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can set precision inverse scale.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanSetPrecisionPrecisionInverseScale([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale)
                     {
                         PrecisionInverseScale = 5.0
                     };
            Assert.AreEqual(meanLocation, ng.MeanLocation);
            Assert.AreEqual(meanScale, ng.MeanScale);
            Assert.AreEqual(precShape, ng.PrecisionShape);
            Assert.AreEqual(5.0, ng.PrecisionInverseScale);
        }

        /// <summary>
        /// Can get mean marginals.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        /// <param name="meanMarginalMean">Mean marginal mean.</param>
        /// <param name="meanMarginalScale">Mean marginal scale.</param>
        /// <param name="meanMarginalDoF">Mean marginal degrees of freedom.</param>
        [Test, Sequential]
        public void CanGetMeanMarginal([Values(0.0, 10.0, 10.0)] double meanLocation, [Values(1.0, 1.0, 1.0)] double meanScale, [Values(1.0, 2.0, 2.0)] double precShape, [Values(1.0, 2.0, Double.PositiveInfinity)] double precInvScale, [Values(0.0, 10.0, 10.0)] double meanMarginalMean, [Values(1.0, 1.0, 0.5)] double meanMarginalScale, [Values(2.0, 4.0, Double.PositiveInfinity)] double meanMarginalDoF)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            var mm = ng.MeanMarginal();
            Assert.AreEqual(meanMarginalMean, mm.Location);
            Assert.AreEqual(meanMarginalScale, mm.Scale);
            Assert.AreEqual(meanMarginalDoF, mm.DegreesOfFreedom);
        }

        /// <summary>
        /// Can get precision marginal.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void CanGetPrecisionMarginal([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0, Double.PositiveInfinity)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            var pm = ng.PrecisionMarginal();
            Assert.AreEqual(precShape, pm.Shape);
            Assert.AreEqual(precInvScale, pm.InvScale);
        }

        /// <summary>
        /// Can get mean.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        /// <param name="meanMean">Mean value.</param>
        /// <param name="meanPrecision">Mean precision.</param>
        [Test, Sequential]
        public void CanGetMean([Values(0.0, 10.0, 10.0)] double meanLocation, [Values(1.0, 1.0, 1.0)] double meanScale, [Values(1.0, 2.0, 2.0)] double precShape, [Values(1.0, 2.0, Double.PositiveInfinity)] double precInvScale, [Values(0.0, 10.0, 10.0)] double meanMean, [Values(1.0, 1.0, 2.0)] double meanPrecision)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            Assert.AreEqual(meanMean, ng.Mean.Mean);
            Assert.AreEqual(meanPrecision, ng.Mean.Precision);
        }

        /// <summary>
        /// Has random source.
        /// </summary>
        [Test]
        public void HasRandomSource()
        {
            var ng = new NormalGamma(0.0, 1.0, 1.0, 1.0);
            Assert.IsNotNull(ng.RandomSource);
        }

        /// <summary>
        /// Can set random source.
        /// </summary>
        [Test]
        public void CanSetRandomSource()
        {
            new NormalGamma(0.0, 1.0, 1.0, 1.0)
            {
                RandomSource = new Random()
            };
        }

        /// <summary>
        /// Validate variance.
        /// </summary>
        /// <param name="meanLocation">Mean location.</param>
        /// <param name="meanScale">Mean scale.</param>
        /// <param name="precShape">Precision shape.</param>
        /// <param name="precInvScale">Precision inverse scale.</param>
        [Test, Combinatorial]
        public void ValidateVariance([Values(0.0, 10.0)] double meanLocation, [Values(1.0, 2.0)] double meanScale, [Values(1.0, 2.0)] double precShape, [Values(1.0, 2.0, Double.PositiveInfinity)] double precInvScale)
        {
            var ng = new NormalGamma(meanLocation, meanScale, precShape, precInvScale);
            var x = precInvScale / (meanScale * (precShape - 1));
            var t = precShape / Math.Sqrt(precInvScale);
            Assert.AreEqual(x, ng.Variance.Mean);
            Assert.AreEqual(t, ng.Variance.Precision);
        }

        /// <summary>
        /// Test the method which samples one variable at a time.
        /// </summary>
        [Test]
        public void SampleFollowsCorrectDistribution()
        {
            var cd = new NormalGamma(1.0, 4.0, 7.0, 3.5);

            // Sample from the distribution.
            var samples = new MeanPrecisionPair[CommonDistributionTests.NumberOfTestSamples];
            for (var i = 0; i < CommonDistributionTests.NumberOfTestSamples; i++)
            {
                samples[i] = cd.Sample();
            }

            // Extract the mean and precisions.
            var means = samples.Select(mp => mp.Mean);
            var precs = samples.Select(mp => mp.Precision);
            var meanMarginal = cd.MeanMarginal();
            var precMarginal = cd.PrecisionMarginal();

            // Check the precision distribution.
            CommonDistributionTests.VapnikChervonenkisTest(
                CommonDistributionTests.Error, 
                CommonDistributionTests.ErrorProbability, 
                precs, 
                precMarginal);

            // Check the mean distribution.
            CommonDistributionTests.VapnikChervonenkisTest(
                CommonDistributionTests.Error, 
                CommonDistributionTests.ErrorProbability, 
                means, 
                meanMarginal);
        }

        /// <summary>
        /// Test the method which samples a sequence of variables.
        /// </summary>
        [Test]
        public void SamplesFollowsCorrectDistribution()
        {
            var cd = new NormalGamma(1.0, 4.0, 3.0, 3.5);

            // Sample from the distribution.
            var samples = cd.Samples().Take(CommonDistributionTests.NumberOfTestSamples).ToArray();

            // Extract the mean and precisions.
            var means = samples.Select(mp => mp.Mean);
            var precs = samples.Select(mp => mp.Precision);
            var meanMarginal = cd.MeanMarginal();
            var precMarginal = cd.PrecisionMarginal();

            // Check the precision distribution.
            CommonDistributionTests.VapnikChervonenkisTest(
                CommonDistributionTests.Error, 
                CommonDistributionTests.ErrorProbability, 
                precs, 
                precMarginal);
            
            // Check the mean distribution.
            CommonDistributionTests.VapnikChervonenkisTest(
                CommonDistributionTests.Error, 
                CommonDistributionTests.ErrorProbability, 
                means, 
                meanMarginal);
        }
    }
}
