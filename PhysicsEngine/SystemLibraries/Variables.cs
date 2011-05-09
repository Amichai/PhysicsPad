using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEngine.Numbers;

namespace PhysicsEngine.SystemLibraries {
	class Variable {
		public Variable(string name, Value value){
			this.name = name;
			this.value = value;
		}
		public Variable(string name, Value value, Expression.Expression function) {
			this.name = name;
			this.value = value;
			this.function = function;
		}

		string name;
		Value value;
		Expression.Expression function; 
	}

	class Variables {
		HashSet<Variable> variables = new HashSet<Variable>() { 
			//new Variable("e", new Value().InitDouble(2.7181, Restrictions.none), new Expression.Expression("lim((1+1/x)^x,x,infinity")),
			//new Variable("pi", new Value(3.1415926, Restrictions.none)),
			//new Variable("i", new Value(0,1))
		};
	}
}
