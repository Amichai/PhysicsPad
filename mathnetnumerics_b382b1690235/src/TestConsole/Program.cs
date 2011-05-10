using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.NumberTheory;
using Examples;
using Examples.SpecialFunctions;
using MathNet.Numerics.LinearAlgebra;

namespace TestConsole {
	class Program {
		static void Main(string[] args) {
			new NumberTheory().Run();
			new Integration().Run();
			new Statistics().Run();
			new RandomNumberGeneration().Run();
			new Examples.ContinuousDistributions.BetaDistribution().Run();
			new Examples.SpecialFunctions.Factorial().Run();
			new Examples.RandomNumberGeneration().Run();
			
			Console.Read();
		}
	}
}
