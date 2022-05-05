// <copyright file="user-settings.ts">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import IKeyword from "./keyword";

export default interface IUserSettings {
    tableId: string | undefined;
    userId: string | undefined
    firstName: string | undefined;
    middleName: string | undefined;
    lastName: string | undefined;
    jobTitle: string[] | undefined;
    otherContact: number | undefined;
    secondaryOtherContact: number | undefined;
    emailAddress: string | undefined;
    secondaryEmailAddress: string | undefined;
    keywordsJson: IKeyword[];
    organization: string | undefined;
    specialty: string | undefined;
    currentOrganization: string | undefined;
    underGraduateDegree: string | undefined;
    graduateDegreeProgram: string | undefined;
    deptOfStudy: string | undefined;
    professionalCertificates: string | undefined;
    professionalOrganizations: string | undefined;
    professionalExperience: string | undefined;
    professionalPublications: string | undefined;
    profilePictureImageURL: string | undefined;
    resumeCVLink: string | undefined;
    communityOfInterests: string | undefined;
    notificationFrequency: number | undefined;
    userDisplayName: string | undefined;
    keywords: number[];
    nodeTypeId: number;
    dateAtPost?: Date;
    rotationDate?: Date;
    dateOfRank?: Date;
    webSite: string;
    advisors: string;
    npsDegreeProgram: string | undefined;
}