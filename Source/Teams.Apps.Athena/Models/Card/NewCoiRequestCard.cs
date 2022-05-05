// <copyright file="NewCoiRequestCard.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    /// <summary>
    /// Describes the information of newly created COI request card.
    /// </summary>
    public class NewCoiRequestCard
    {
        /// <summary>
        /// Gets or sets the card title.
        /// </summary>
        public string CoiRequestAdaptiveCardTitle { get; set; }

        /// <summary>
        /// Gets or sets the title label.
        /// </summary>
        public string AdaptiveCardTitleLabel { get; set; }

        /// <summary>
        /// Gets or sets the type label.
        /// </summary>
        public string AdaptiveCardTypeLabel { get; set; }

        /// <summary>
        /// Gets or sets the date label.
        /// </summary>
        public string AdaptiveCardDateLabel { get; set; }

        /// <summary>
        /// Gets or sets the status label.
        /// </summary>
        public string AdaptiveCardStatusLabel { get; set; }

        /// <summary>
        /// Gets or sets the view details button label.
        /// </summary>
        public string AdaptiveCardViewDetailsButtonLabel { get; set; }

        /// <summary>
        /// Gets or sets the COI table Id.
        /// </summary>
        public string CoiTableId { get; set; }

        /// <summary>
        /// Gets or sets the title of COI request.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of COI.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which request was created.
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the user name who created the request.
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets the status of COI request.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the operation to be executed which will be accessed at Bot activity handler.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the admin comment for rejection.
        /// </summary>
        public string AdminComment { get; set; }
    }
}
