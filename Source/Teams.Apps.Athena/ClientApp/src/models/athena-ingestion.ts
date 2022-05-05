// <copyright file="athena-ingestion.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

export interface IAthenaIngestionItems {
    dbEntity?: string | undefined,
    frequency?: number | undefined,
    updatedAt?: Date | undefined,
    filePath?: string | undefined
}