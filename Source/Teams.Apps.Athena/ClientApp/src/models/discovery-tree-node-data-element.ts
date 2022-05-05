// <copyright file="discovery-tree-node-data-element.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IResearchProject from "./research-project";
import IResearchRequest from "./research-request";
import ISponsorDetails from "./sponsor-details";
import IPartnerDetails from "./partner-details";
import IResearchProposal from "./research-proposal";
import ICoi from "./coi";
import INews from "./news";
import IAthenaEvent from "./athena-event";
import IUserSettings from "./user-settings";
import IAthenaInfoResource from "./athena-info-resource";
import IAthenaTool from "./athena-tool";

export default interface IDiscoveryTreeNodeDataElement {
    researchProjects: IResearchProject[];
    researchRequests: IResearchRequest[];
    sponsors: ISponsorDetails[];
    partners: IPartnerDetails[];
    researchProposals: IResearchProposal[];
    cois: ICoi[];
    newsArticles: INews[];
    events: IAthenaEvent[];
    users: IUserSettings[];
    athenaInfoResources: IAthenaInfoResource[];
    athenaTools: IAthenaTool[];
}