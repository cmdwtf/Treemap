using System;

namespace Example
{
	internal static class Extensions
	{
		public static T Next<T>(this T src) where T : Enum
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum.");
			}

			var Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) + 1;
			return (Arr.Length == j) ? Arr[0] : Arr[j];
		}
	}
}
