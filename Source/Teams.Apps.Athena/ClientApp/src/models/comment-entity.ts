// <copyright file="comment-entity.ts">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

export interface ICommentEntity {
    commentId: string;
    researchProjectTableId: string;
    comment: string;
    userId: string;
    userName: string;
}