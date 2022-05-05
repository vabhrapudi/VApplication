// <copyright file="router.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import SignInPage from "../components/signin/sign-in";
import SignInSimpleStart from "../components/signin/sign-in-start";
import SignInSimpleEnd from "../components/signin/sign-in-end";
import NewRequest from "../components/new-request/new-request";
import MyRequests from "../components/my-requests/my-requests";
import "../i18n";
import ErrorPage from "../components/error-page";
import CollectionsHome from "../components/collections-tab/collections-home/collections-home";
import NewCollection from "../components/collections-tab/new-collection/new-collection";
import NewsHome from "../components/research-news-tab/news-home/news-home";
import TabConfiguration from "../components/tab-configuration/tab-configuration";
import UserSettings from "../components/UserSettingsTab/user-settings";
import UserSettingsTaskModule from "../components/UserSettingsTab/user-settings-task-module";
import PreviewUserInfo from "../components/UserSettingsTab/preview-user-info";
import PreviewUserDetails from "../components/UserSettingsTab/preview-user-details";
import AdminTeamConfigurationTab from "../components/admin-team-tab-configuration/admin-team-tab-configuration";
import AthenaFeedback from "../components/athena-splash/feedback";
import HelpPage from "../components/athena-splash/help";
import About from "../components/athena-splash/about";
import AdminRequestsTab from "../components/admin-requests/admin-requests";
import ApproveRejectRequestsTaskModule from "../components/admin-requests/approve-reject-task-module";
import DiscoveryHome from "../components/discovery-tab/discovery-home/discovery-home";
import ResearchProposal from "../components/research-proposal/research-proposal";
import AddCollectionItem from "../components/add-collection-item/add-collection-item";
import HomeConfiguration from "../components/home-configuration/home-configuration";
import NewHomeConfigurationArticle from "../components/home-configuration/new-home-configuration-article";
import Home from "../components/home/home";
import FeedbackHome from "../components/feedback-tab/feedback-home/feedback-home";
import FeedbackDetailsTaskModule from "../components/feedback-tab/feedback-details-task-module/feedback-details-task-module";
import InsightsConfiguration from "../components/insights-configuration/insights-configuration";
import NewPriority from "../components/insights-configuration/new-priority";
import InsightsTab from "../components/insights-tab/insights-tab";
import AthenaIngestion from "../components/athena-ingestion/athena-ingestion";

export const AppRoute: React.FunctionComponent<{}> = () => {
    return (
        <React.Suspense fallback={<div className="container-div"><div className="container-subdiv"></div></div>}>
            <BrowserRouter>
                <Switch>
                    <Route exact path="/error" component={ErrorPage} />
                    <Route exact path="/" component={UserSettings} />
                    <Route exact path="/userContactInfo" component={UserSettingsTaskModule} />
                    <Route exact path="/preview-user-info" component={PreviewUserInfo} />
                    <Route exact path="/preview-user-details" component={PreviewUserDetails} />
                    <Route exact path="/signin" component={SignInPage} />
                    <Route exact path="/signin-simple-start" component={SignInSimpleStart} />
                    <Route exact path="/signin-simple-end" component={SignInSimpleEnd} />
                    <Route exact path="/news-home" component={NewsHome} />
                    <Route exact path="/tab-configuration" component={TabConfiguration} />
                    <Route exact path="/new-request" component={NewRequest} />
                    <Route exact path="/my-requests" component={MyRequests} />
                    <Route exact path="/collection-home" component={CollectionsHome} />
                    <Route exact path="/new-collection" component={NewCollection} />
                    <Route exact path="/admin-team-tab-configuration" component={AdminTeamConfigurationTab} />
                    <Route exact path="/athena-feedback" component={AthenaFeedback} />
                    <Route exact path="/help-page" component={HelpPage} />
                    <Route exact path="/about-page" component={About} />
                    <Route exact path="/admin-requests-tab" component={AdminRequestsTab} />
                    <Route exact path="/approve-reject-requests" component={ApproveRejectRequestsTaskModule} />
                    <Route exact path="/discover" component={DiscoveryHome} />
                    <Route exact path="/new-research-proposal" component={ResearchProposal} />
                    <Route exact path="/add-collection-item" component={AddCollectionItem} />
                    <Route exact path="/home-configuration" component={HomeConfiguration} />
                    <Route exact path="/new-home-configuration-article" component={NewHomeConfigurationArticle} />
                    <Route exact path="/home" component={Home} />
                    <Route exact path="/admin-feedback-tab" component={FeedbackHome} />
                    <Route exact path="/feedback-details" component={FeedbackDetailsTaskModule} />
                    <Route exact path="/insights-admin-tab" component={InsightsConfiguration} />
                    <Route exact path="/new-priority" component={NewPriority} />
                    <Route exact path="/insights" component={InsightsTab} />
                    <Route exact path="/athena-ingestion" component={AthenaIngestion} />
                </Switch>
            </BrowserRouter>
        </React.Suspense>
    );
};