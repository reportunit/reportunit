using System;

namespace ReportUnit.Extensions
{
    using System.Xml.Linq;

    /// <summary>
    /// To avoid having to write the Null verification on every attribute in the report, this extension method provides
    /// reusable code.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Checks if an attribute with the specified name exists and returns it's value.
        /// </summary>
        /// <param name="element">
        /// The element to get the attribute from.
        /// </param>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <returns>
        /// The string value of the attribute if it's not null or string.Empty if it's null.
        /// </returns>
        public static string GetNullableAttribute(this XElement element, string attributeName)
        {
            var attr = element.Attribute(attributeName);
            return attr?.Value ?? string.Empty;
        }

        /// <summary>
        /// Compare an attribute value to an <paramref name="expectedValue"/> and returns true if they are equals (ignoring case)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static bool AttributeEqualsTo(this XElement element, string attributeName, string expectedValue)
        {
            var value = element.GetNullableAttribute(attributeName);
            return value.Equals(expectedValue, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Compare an attribute value to those <paramref name="expectedValues"/> and returns true if one of them is equal (ignoring case)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="expectedValues"></param>
        /// <returns></returns>
        public static bool AttributeEqualsOneOf(this XElement element, string attributeName, params string[] expectedValues)
        {
            foreach (var expectedValue in expectedValues)
            {
                if (element.AttributeEqualsTo(attributeName, expectedValue))
                    return true;
            }
            return false;
        }
    }
}
