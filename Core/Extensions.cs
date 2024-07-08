using System.Text;
using System.Xml;

namespace DBFConverter.Core;

public static class Extensions {
	public static string Populate(this string str, int n) {
		StringBuilder builder = new();
		
		for (int i = 0; i < n; ++i) {
			builder.Append(str);
		}

		return builder.ToString();
	}
}