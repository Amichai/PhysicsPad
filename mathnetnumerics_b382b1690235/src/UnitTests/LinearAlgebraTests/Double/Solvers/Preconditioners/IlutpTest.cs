// <copyright file="IlutpTest.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Double.Solvers.Preconditioners
{
    using System;
    using System.Reflection;
    using LinearAlgebra.Double;
    using LinearAlgebra.Double.Solvers.Preconditioners;
    using NUnit.Framework;

    /// <summary>
    /// Incomplete LU with tpPreconditioner test with drop tolerance and partial pivoting.
    /// </summary>
    [TestFixture]
    public sealed class IlutpPreconditionerTest : PreconditionerTest
    {
        /// <summary>
        /// The drop tolerance.
        /// </summary>
        private double _dropTolerance = 0.1;

        /// <summary>
        /// The fill level.
        /// </summary>
        private double _fillLevel = 1.0;

        /// <summary>
        /// The pivot tolerance.
        /// </summary>
        private double _pivotTolerance = 1.0;

        /// <summary>
        /// Setup default parameters.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _dropTolerance = 0.1;
            _fillLevel = 1.0;
            _pivotTolerance = 1.0;
        }

        /// <summary>
        /// Invoke method from Ilutp class.
        /// </summary>
        /// <typeparam name="T">Type of the return value.</typeparam>
        /// <param name="ilutp">Ilutp instance.</param>
        /// <param name="methodName">Method name.</param>
        /// <returns>Result of the method invocation.</returns>
        private static T GetMethod<T>(Ilutp ilutp, string methodName)
        {
            var type = ilutp.GetType();
            var methodInfo = type.GetMethod(
                methodName, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, 
                null, 
                CallingConventions.Standard, 
                new Type[0], 
                null);
            var obj = methodInfo.Invoke(ilutp, null);
            return (T)obj;
        }

        /// <summary>
        /// Get upper triangle.
        /// </summary>
        /// <param name="ilutp">Ilutp instance.</param>
        /// <returns>Upper triangle.</returns>
        private static SparseMatrix GetUpperTriangle(Ilutp ilutp)
        {
            return GetMethod<SparseMatrix>(ilutp, "UpperTriangle");
        }

        /// <summary>
        /// Get lower triangle.
        /// </summary>
        /// <param name="ilutp">Ilutp instance.</param>
        /// <returns>Lower triangle.</returns>
        private static SparseMatrix GetLowerTriangle(Ilutp ilutp)
        {
            return GetMethod<SparseMatrix>(ilutp, "LowerTriangle");
        }

        /// <summary>
        /// Get pivots.
        /// </summary>
        /// <param name="ilutp">Ilutp instance.</param>
        /// <returns>Pivots array.</returns>
        private static int[] GetPivots(Ilutp ilutp)
        {
            return GetMethod<int[]>(ilutp, "Pivots");
        }

        /// <summary>
        /// Create reverse unit matrix.
        /// </summary>
        /// <param name="size">Matrix order.</param>
        /// <returns>Reverse Unit matrix.</returns>
        private static SparseMatrix CreateReverseUnitMatrix(int size)
        {
            var matrix = new SparseMatrix(size);
            for (var i = 0; i < size; i++)
            {
                matrix[i, size - 1 - i] = 2;
            }

            return matrix;
        }

        /// <summary>
        /// Create preconditioner (internal)
        /// </summary>
        /// <returns>Ilutp instance.</returns>
        private Ilutp InternalCreatePreconditioner()
        {
            var result = new Ilutp
                         {
                             DropTolerance = _dropTolerance, 
                             FillLevel = _fillLevel, 
                             PivotTolerance = _pivotTolerance
                         };
            return result;
        }

        /// <summary>
        /// Create preconditioner.
        /// </summary>
        /// <returns>New preconditioner instance.</returns>
        internal override IPreConditioner CreatePreconditioner()
        {
            _pivotTolerance = 0;
            _dropTolerance = 0.0;
            _fillLevel = 100;
            return InternalCreatePreconditioner();
        }

        /// <summary>
        /// Check the result.
        /// </summary>
        /// <param name="preconditioner">Specific preconditioner.</param>
        /// <param name="matrix">Source matrix.</param>
        /// <param name="vector">Initial vector.</param>
        /// <param name="result">Result vector.</param>
        protected override void CheckResult(IPreConditioner preconditioner, SparseMatrix matrix, Vector vector, Vector result)
        {
            Assert.AreEqual(typeof(Ilutp), preconditioner.GetType(), "#01");

            // Compute M * result = product
            // compare vector and product. Should be equal
            Vector product = new DenseVector(result.Count);
            matrix.Multiply(result, product);
            for (var i = 0; i < product.Count; i++)
            {
                Assert.IsTrue(vector[i].AlmostEqual(product[i], -Epsilon.Magnitude()), "#02-" + i);
            }
        }

        /// <summary>
        /// Solve returning old vector without pivoting.
        /// </summary>
        [Test]
        public void SolveReturningOldVectorWithoutPivoting()
        {
            const int Size = 10;

            var newMatrix = CreateUnitMatrix(Size);
            var vector = CreateStandardBcVector(Size);

            // set the pivot tolerance to zero so we don't pivot
            _pivotTolerance = 0.0;
            _dropTolerance = 0.0;
            _fillLevel = 100;
            var preconditioner = CreatePreconditioner();
            preconditioner.Initialize(newMatrix);
            Vector result = new DenseVector(vector.Count);
            preconditioner.Approximate(vector, result);
            CheckResult(preconditioner, newMatrix, vector, result);
        }

        /// <summary>
        /// Solve returning old vector with pivoting.
        /// </summary>
        [Test]
        public void SolveReturningOldVectorWithPivoting()
        {
            const int Size = 10;
            var newMatrix = CreateUnitMatrix(Size);
            var vector = CreateStandardBcVector(Size);

            // Set the pivot tolerance to 1 so we always pivot (if necessary)
            _pivotTolerance = 1.0;
            _dropTolerance = 0.0;
            _fillLevel = 100;
            var preconditioner = CreatePreconditioner();
            preconditioner.Initialize(newMatrix);
            Vector result = new DenseVector(vector.Count);
            preconditioner.Approximate(vector, result);
            CheckResult(preconditioner, newMatrix, vector, result);
        }

        /// <summary>
        /// Compare with original dense matrix without pivoting.
        /// </summary>
        [Test]
        public void CompareWithOriginalDenseMatrixWithoutPivoting()
        {
            var sparseMatrix = new SparseMatrix(3);
            sparseMatrix[0, 0] = -1;
            sparseMatrix[0, 1] = 5;
            sparseMatrix[0, 2] = 6;
            sparseMatrix[1, 0] = 3;
            sparseMatrix[1, 1] = -6;
            sparseMatrix[1, 2] = 1;
            sparseMatrix[2, 0] = 6;
            sparseMatrix[2, 1] = 8;
            sparseMatrix[2, 2] = 9;
            var ilu = new Ilutp
                      {
                          PivotTolerance = 0.0, 
                          DropTolerance = 0, 
                          FillLevel = 10
                      };
            ilu.Initialize(sparseMatrix);
            var l = GetLowerTriangle(ilu);

            // Assert l is lower triagonal
            for (var i = 0; i < l.RowCount; i++)
            {
                for (var j = i + 1; j < l.RowCount; j++)
                {
                    Assert.IsTrue(0.0.AlmostEqual(l[i, j], -Epsilon.Magnitude()), "#01-" + i + "-" + j);
                }
            }

            var u = GetUpperTriangle(ilu);

            // Assert u is upper triagonal
            for (var i = 0; i < u.RowCount; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    Assert.IsTrue(0.0.AlmostEqual(u[i, j], -Epsilon.Magnitude()), "#02-" + i + "-" + j);
                }
            }

            var original = l.Multiply(u);
            for (var i = 0; i < sparseMatrix.RowCount; i++)
            {
                for (var j = 0; j < sparseMatrix.ColumnCount; j++)
                {
                    Assert.IsTrue(sparseMatrix[i, j].AlmostEqual(original[i, j], -Epsilon.Magnitude()), "#03-" + i + "-" + j);
                }
            }
        }

        /// <summary>
        /// Compare with original dense matrix with pivoting.
        /// </summary>
        [Test]
        public void CompareWithOriginalDenseMatrixWithPivoting()
        {
            var sparseMatrix = new SparseMatrix(3);
            sparseMatrix[0, 0] = -1;
            sparseMatrix[0, 1] = 5;
            sparseMatrix[0, 2] = 6;
            sparseMatrix[1, 0] = 3;
            sparseMatrix[1, 1] = -6;
            sparseMatrix[1, 2] = 1;
            sparseMatrix[2, 0] = 6;
            sparseMatrix[2, 1] = 8;
            sparseMatrix[2, 2] = 9;
            var ilu = new Ilutp
                      {
                          PivotTolerance = 1.0, 
                          DropTolerance = 0, 
                          FillLevel = 10
                      };
            ilu.Initialize(sparseMatrix);
            var l = GetLowerTriangle(ilu);
            var u = GetUpperTriangle(ilu);
            var pivots = GetPivots(ilu);
            var p = new SparseMatrix(l.RowCount);
            for (var i = 0; i < p.RowCount; i++)
            {
                p[i, pivots[i]] = 1.0;
            }

            var temp = l.Multiply(u);
            var original = temp.Multiply(p);
            for (var i = 0; i < sparseMatrix.RowCount; i++)
            {
                for (var j = 0; j < sparseMatrix.ColumnCount; j++)
                {
                    Assert.IsTrue(sparseMatrix[i, j].AlmostEqual(original[i, j], -Epsilon.Magnitude()), "#01-" + i + "-" + j);
                }
            }
        }

        /// <summary>
        /// Solve with pivoting.
        /// </summary>
        [Test]
        public void SolveWithPivoting()
        {
            const int Size = 10;
            var newMatrix = CreateReverseUnitMatrix(Size);
            var vector = CreateStandardBcVector(Size);
            var preconditioner = new Ilutp
                                 {
                                     PivotTolerance = 1.0, 
                                     DropTolerance = 0, 
                                     FillLevel = 10
                                 };
            preconditioner.Initialize(newMatrix);
            Vector result = new DenseVector(vector.Count);
            preconditioner.Approximate(vector, result);
            CheckResult(preconditioner, newMatrix, vector, result);
        }
    }
}
