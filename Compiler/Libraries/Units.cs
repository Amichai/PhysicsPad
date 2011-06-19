using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Libraries {
	class Units {
		public static HashSet<string> MassUnits = new HashSet<string>() {"amu", "gram", "kilogram", "picogram", "miligram", "ton" };
		public static HashSet<string> VolumeUnits = new HashSet<string>() { "liter", "gallon", "cup", "mililiter", "nanoliter" };
	}
}
