using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;
using System.Numerics;

namespace Compiler {

	public interface IVariable {
	}
	

	public class PI : IVariable {
		public readonly static Complex value = new Complex(Math.PI, 0);
	}
}
