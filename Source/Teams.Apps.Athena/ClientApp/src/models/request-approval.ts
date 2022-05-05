// <copyright file="coi-request-approval.ts">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

export default interface IRequestApproval {
    RequestIds: string[],
    ItemType: number,
    Comment: string,
}