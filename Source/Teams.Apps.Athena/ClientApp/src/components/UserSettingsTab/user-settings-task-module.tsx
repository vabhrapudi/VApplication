// <copyright file="user-contact-info.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import IUserSettings from '../../models/user-settings';
import { getUserDetailsAsync, getUserSettingAsync, getUserDetailsByEmailAddressAsync } from "../../api/user-settings-tab-api";
import UserSettingPage1 from './user-contact-info';
import SearchKeywords from './search-keywords';
import UserSettingsPage3 from './coi-page';
import UserSettingsPreview from './preview-user-details';
import Loader from "../common/loader/loader";
import IKeyword from '../../models/keyword';
import { getAllKeywordsAsync } from '../../api/keyword-api';
import { StatusCodes } from 'http-status-codes';
import { withRouter, RouteComponentProps } from "react-router-dom";

import "./user-settings.scss";

interface IUserContactInfoProps extends WithTranslation, RouteComponentProps {
}

interface IUserContactInfoParams {
    userDetails: string,
}

interface IUserContactInfoState {
    userDetails: IUserSettings,
    currentPage: number,
    loading: boolean,
    keywords: IKeyword[]
}

class UserContactInfo extends React.Component<IUserContactInfoProps, IUserContactInfoState> {
    localize: TFunction;
    params: IUserContactInfoParams = {} as IUserContactInfoParams;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            userDetails: {} as IUserSettings,
            currentPage: 0,
            loading: true,
            keywords: []
        }
    }
    public componentDidMount() {
        this.getUserSettingDetails();
        this.fetchKeywords();
    }

    // Fetches the keywords data.
    fetchKeywords = async () => {
        let response = await getAllKeywordsAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywords = response.data as IKeyword[];
            this.setState({ keywords });
        }
    }

    public getUserSettingDetails = async () => {
        // get user details from database APi call.
        let response = await getUserSettingAsync();

        if (response && response.status === 404) {
            // get user details from graph APi call.
            let loggedInUserDetails = await getUserDetailsAsync();

            if (loggedInUserDetails && loggedInUserDetails.status === 200) {
                let data: any = loggedInUserDetails.data;

                let userDetails = {
                    firstName: data.firstName,
                    lastName: data.surname,
                    emailAddress: data.mail,
                    otherContact: data.mobilePhone
                } as IUserSettings;

                let apiResponse = await getUserDetailsByEmailAddressAsync(data.mail);

                if (apiResponse && apiResponse.status === 200) {
                    let details: any = apiResponse.data;
                    userDetails.communityOfInterests = details.communityOfInterests;
                    userDetails.tableId = details.tableId;
                    userDetails.middleName = details.middleName;
                    userDetails.jobTitle = details.jobTitle;
                    userDetails.keywordsJson = details.keywords;
                    userDetails.specialty = details.specialty;
                    userDetails.currentOrganization = details.currentOrganization;
                    userDetails.underGraduateDegree = details.underGraduateDegree;
                    userDetails.graduateDegreeProgram = details.graduateDegreeProgram;
                    userDetails.deptOfStudy = details.deptOfStudy;
                }

                this.setState({ userDetails, loading: false });
            } else {
                this.setState({ loading: false });
            }
        }
        else {
            this.setState({
                userDetails: response.data,
                loading: false
            });
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    public onBack = (userDetails: IUserSettings) => {
        this.setState({ currentPage: this.state.currentPage - 1, userDetails: { ...userDetails } });
    }

    public onNext = (userDetails: IUserSettings) => {
        this.setState({ currentPage: this.state.currentPage + 1, userDetails: { ...userDetails } });
    }

    private getPageToRender = () => {
        let componentToRender = <></>;
        switch (this.state.currentPage) {
            case 0: componentToRender = <UserSettingPage1 onBackClick={this.onBack} onNextClick={this.onNext} userDetails={this.state.userDetails}/>
                break;
            case 1: componentToRender = <SearchKeywords onBackClick={this.onBack} onNextClick={this.onNext} userDetails={this.state.userDetails} keywords={this.state.keywords} />
                break;
            case 2: componentToRender = <UserSettingsPage3 onBackClick={this.onBack} onNextClick={this.onNext} userDetails={this.state.userDetails} keywords={this.state.keywords} />
                break;
            case 3: componentToRender = <UserSettingsPreview onBackClick={this.onBack} userDetails={this.state.userDetails} keywords={this.state.keywords} />
                break;
            default: componentToRender = <UserSettingPage1 onBackClick={this.onBack} onNextClick={this.onNext} userDetails={this.state.userDetails}/>
                break;
        }

        return componentToRender;
    }

    // Renders the component
    render() {
        if (this.state.loading) {
            return <Loader />;
        }
        else {
            return this.getPageToRender();
        }
    }
}

export default withTranslation()(withRouter(UserContactInfo));