using System;
using System.Reflection;
using System.Windows.Forms;

using MethodDecorator.Fody.Interfaces;

[module: cmdwtf.Treemap.InvalidatesControl]

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A decorator that causes the targets it is applied to to call their <see cref="Control.Invalidate()"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false, Inherited = true)]
	public class InvalidatesControlAttribute : Attribute, IMethodDecorator
	{
		/// <summary>
		/// The target control.
		/// </summary>
		private Control? _control = null;

		/// <summary>
		/// Initializes the attribute with the given values.
		/// </summary>
		/// <param name="instance">The object this attribute is on's instance.</param>
		/// <param name="method">The method this attribute is applied to.</param>
		/// <param name="args">Arguments to the method, if any.</param>
		public void Init(object instance, MethodBase method, object[] args)
			=> _control = instance as Control ??
				throw new ArgumentException($"{nameof(InvalidatesControlAttribute)} can only decorate targets in classes that derive from {nameof(Control)}");

		/// <summary>
		/// Called after the decorated method exits.
		/// </summary>
		public void OnExit()
			=> _control?.Invalidate();

		/// <summary>
		/// Called when the decorated method is about to run.
		/// </summary>
		public void OnEntry() { }

		/// <summary>
		/// Called if the decorated method throws an exception.
		/// </summary>
		/// <param name="exception"></param>
		public void OnException(Exception exception) { }
	}
}
