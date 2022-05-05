// <copyright file="discovery-tree-item-type.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

// Contains the values which shows type of discovery tree item
export enum ItemType {
    // Indicates invalid item.
    None,

    // Indicates news item.
    News,

    // Indicates COI item.
    COI,
    
    // Indicates research request item.
    ResearchRequest,

    // Indicates research project item.
    ResearchProject,

    // Indicates research paper item.
    ResearchPaper,

    // Indicates user.
    User,

    // Indicates research proposal item.
    ResearchProposal,

    // Indicates event item.
    Event,

    // Indicates partner item.
    Partner,

    // Indicates sponsor item.
    Sponsor,

    // Indicates source item.
    Source,

    // Indicates information item.
    Information,

    // Indicates tool item.
    Tool
}