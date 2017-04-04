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
            return attr != null ? attr.Value : string.Empty;
        }
    }
}
