using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides a type converter to convert <see cref="TreemapNode"/> objects to
	/// and from various other representations.
	/// </summary>
	public class TreemapNodeConverter : TypeConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNodeConverter"/> class.
		/// </summary>
		public TreemapNodeConverter()
		{

		}

		/// <summary>
		/// Gets a value indicating whether this converter can convert an object to the given
		/// destination type using the context.
		/// </summary>
		/// <param name="context"> An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="Type"/> that represents the type you wish to convert to.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}

			if (destinationType == typeof(InstanceDescriptor))
			{
				return false;
			}

			return false;
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context
		/// and culture information.
		/// </summary>
		/// <param name="context"> An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="CultureInfo"/>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="object"/> to convert</param>
		/// <param name="destinationType">A <see cref="Type"/> to convert the value parameter to.</param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo? culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return value.ToString() ?? string.Empty;
			}

			throw new NotSupportedException($"The destination type {destinationType.Name} is not supported.");
		}
	}
}