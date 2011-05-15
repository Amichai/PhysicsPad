using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;

namespace PhysicsEngine.ReferenceLibraries {
	static class Variable{
		public static readonly HashSet<string> Library = new HashSet<string>() { 
			"ANS", "PI" };
		public static Value PI = new Value(new BigRationalNumerics.BigRational(Math.PI), Restrictions.none);
	}
}
