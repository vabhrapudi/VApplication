// <copyright file="DateTimeExtension.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Extensions
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Extension method which convert the <see cref="DateTime"/> value in Zulu time format without containing milliseconds.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value.</param>
        /// <returns>The string in Zulu format.</returns>
        public static string ToZuluTimeFormatWithoutMilliseconds(this DateTime value)
        {
            return $"{value.Year}-{value.Month:00}-{value.Day:00}T{value.Hour:00}:{value.Minute:00}:{value.Second:00}Z";
        }

        /// <summary>
        /// Extension method which convert the <see cref="DateTime"/> value in Zulu time format with start of day without containing milliseconds.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value.</param>
        /// <returns>The string in Zulu format.</returns>
        public static string ToZuluTimeFormatWithStartOfDay(this DateTime value)
        {
            return $"{value.Year}-{value.Month:00}-{value.Day:00}T00:00:00Z";
        }
    }
}
