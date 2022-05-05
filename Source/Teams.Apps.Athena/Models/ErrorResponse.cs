// <copyright file="ErrorResponse.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Custom error response model for APIs.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets detailed error message to be sent.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets list of validation errors.
        /// </summary>
#pragma warning disable CA2227 // Required to set list of error before sending error response from APIs
        public List<string> Errors { get; set; }
#pragma warning restore CA2227 // Required to set list of error before sending error response from APIs
    }
}
