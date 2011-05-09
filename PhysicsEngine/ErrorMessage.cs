using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsEngine {
	public class ErrorMessage {
		string message = string.Empty;
		public ErrorMessage(string message) {
			this.message = message;
			//Log where in the code base the error happened
		}
	}
}
