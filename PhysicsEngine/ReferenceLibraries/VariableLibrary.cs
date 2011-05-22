using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using System.Numerics;

namespace PhysicsEngine.ReferenceLibraries {
	static class Variable{
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"ANS", "PI" };
		public static Complex PI = new Complex(Math.PI, 0);
	}
}
