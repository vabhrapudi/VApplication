// <copyright file="IGraphServiceFactory.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    /// <summary>
    /// Interface for Graph service factory.
    /// </summary>
    public interface IGraphServiceFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IUserService"/> implementation.
        /// </summary>
        /// <returns>Returns an implementation of <see cref="IUserService"/>.</returns>
        public IUserService GetUserService();
    }
}