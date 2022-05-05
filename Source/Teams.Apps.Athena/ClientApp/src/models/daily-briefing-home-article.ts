// <copyright file="daily-briefing-home-article.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

export default interface IDailyBriefingHomeArticle {
    resourceId: string;
    title: string;
    description: string;
    updatedOn?: Date;
    articleUrl: string;
    nodeTypeId: number;
}