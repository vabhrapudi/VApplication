// <copyright file="StringExtension.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Extensions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Extension method to check whether a string value is invalid Guid.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>Returns true if the string value is invalid Guid. Else returns false.</returns>
        public static bool IsEmptyOrInvalidGuid(this string value)
        {
            return string.IsNullOrEmpty(value)
                || !Guid.TryParse(value, out Guid result)
                || result == Guid.Empty;
        }

        /// <summary>
        /// Deserialize the string into object and handles the exceptions if any.
        /// </summary>
        /// <typeparam name="T">The type of object to which value to be parsed.</typeparam>
        /// <param name="value">The value to be parsed.</param>
        /// <returns>The parsed object.</returns>
        public static T TryDeserializeObject<T>(this string value)
        {
            if (value == null)
            {
                return default;
            }

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
            };

            try
            {
                return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Escaping unsafe, reserved and special characters that requires escaping includes
        /// + - &amp; | ! ( ) { } [ ] ^ " ~ * ? : \ /
        /// </summary>
        /// <param name="value">The string value</param>
        /// <returns>Returns string escaping unsafe, reserved and special characters.</returns>
        public static string EscapeSpecialCharacters(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("*", string.Empty, StringComparison.InvariantCulture).Trim();
                value = value.Replace("'", "''", StringComparison.InvariantCulture).Trim();
                string pattern = @"(@|&|\(|\)|\.|<|>|#)";
                string substitution = "\\$&";
                value = Regex.Replace(value, pattern, substitution);
                value = value.Any(ch => !char.IsLetterOrDigit(ch)) ? value += "\\" + "*" : value += "*";
            }

            return value;
        }
    }
}
