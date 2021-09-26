#if !NET5_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// A small fix for using init autoprops pre .NET 5.0.
	/// The powers that be <see href="https://developercommunity.visualstudio.com/t/error-cs0518-predefined-type-systemruntimecompiler/1244809">
	/// decided this is not a bug</see>.
	/// </summary>
	internal static class IsExternalInit { }
}

#endif // !NET5_0_OR_GREATER
