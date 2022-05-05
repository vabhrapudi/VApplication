// <copyright file="preview-user-details.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import IUserSettings from '../../models/user-settings';
import * as microsoftTeams from "@microsoft/teams-js";
import { Button, Flex, Text, Divider } from '@fluentui/react-northstar';
import { createUserAsync, updateUserSettingAsync } from '../../api/user-settings-tab-api';
import { ResponseStatus } from '../../constants/constants';
import { StatusCodes } from 'http-status-codes';
import { getStringJoinedBySeparator } from '../../helpers/common-helper';
import CardImage from '../common/card-image/card-image';
import IKeyword from '../../models/keyword';

import "./preview-user-details.scss";

interface IPreviewUserDetailsProps extends WithTranslation {
    onBackClick: (userDetails: IUserSettings) => void,
    userDetails: IUserSettings,
    keywords: IKeyword[]
}

export interface IPreviewUserDetailsState {
    isNextPage: boolean;
    isCreatingOrUpdating: boolean,
}

class PreviewUserDetails extends React.Component<IPreviewUserDetailsProps, IPreviewUserDetailsState> {
    localize: TFunction;
    previewLabel = { width: "25vw", minWidth:"25vw" };
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            isNextPage: false,
            isCreatingOrUpdating: false
        }
    }

    createUserAsync = async () => {
        this.setState({ isCreatingOrUpdating: true });
        let response = await createUserAsync(this.props.userDetails);

        if (response.status === ResponseStatus.CREATED && response.data) {
            this.setState({ isCreatingOrUpdating: false });
            microsoftTeams.tasks.submitTask({ data: true });
        }
        else {
            this.setState({ isCreatingOrUpdating: false });
            microsoftTeams.tasks.submitTask({ data: false });
        }
    }

    /** The HTTP POST call to add a new user in storage */
    updateUserAsync = async () => {
        this.setState({ isCreatingOrUpdating: true });

        let response = await updateUserSettingAsync(this.props.userDetails);

        if (response && response.status === StatusCodes.OK) {
            microsoftTeams.tasks.submitTask({data: true});
        }
        else {
            microsoftTeams.tasks.submitTask({ data: false });
        }
    }

    // Returns the keywords string.
    getKeywordsString = () => {
        let keywordIdsStringArray = this.props.userDetails.keywords.map(String);

        let keywordsTitleArray: string[] = this.props.keywords
            .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId))
            .map((keyword: IKeyword) => keyword.title);

        return keywordsTitleArray.length ? keywordsTitleArray.join(", ") : "NA";
    }

    // Returns the job title string.
    getJobTitleString = () => {
        let jobTitleStrings = this.props.userDetails.jobTitle;
        return getStringJoinedBySeparator(jobTitleStrings, ", ")
    }

    // Renders the component
    render() {
        return (
            <Flex column gap="gap.medium" className="task-module-container">
                <Text className="page-heading" content={this.localize("previewHeadText")} />
                <Divider size={2} color="brand" />
                <Text className="preview-page-title" content={this.localize("userContactInformationText")} />

                <Flex column gap="gap.smaller" className="form-fields">
                    <Flex gap="gap.small">
                        <Flex fill gap="gap.small" column>
                            <Flex gap="gap.small">
                                <Flex.Item styles={this.previewLabel}>
                                    <Text weight="semibold" content={this.localize("firstNameText")+":"} />
                                </Flex.Item>
                                <Text truncated content={this.props.userDetails.firstName} title={this.props.userDetails.firstName}/>
                            </Flex>
                            <Flex gap="gap.small">
                                <Flex.Item styles={this.previewLabel}>
                                    <Text weight="semibold" content={this.localize("middleNameText") + ":"} />
                                </Flex.Item>
                                <Text truncated content={this.props.userDetails.middleName} title={this.props.userDetails.middleName} />
                            </Flex>
                            <Flex gap="gap.small">
                                <Flex.Item styles={this.previewLabel}>
                                    <Text weight="semibold" content={this.localize("lastNameText") + ":"} />
                                </Flex.Item>
                                <Text truncated content={this.props.userDetails.lastName} title={this.props.userDetails.lastName} />
                            </Flex>
                            <Flex gap="gap.small">
                                <Flex.Item styles={this.previewLabel}>
                                    <Text weight="semibold" content={this.localize("jobTitleText") + ":"} />
                                </Flex.Item>
                                <Text truncated content={this.getJobTitleString()} title={this.getJobTitleString()} />
                            </Flex>
                        </Flex>
                        <Flex.Item size="size.quarter" push>
                            <CardImage imageSrc={this.props.userDetails.profilePictureImageURL!} className="preview-user-image" />
                        </Flex.Item>
                    </Flex>
                    <Flex fill gap="gap.small" column>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("otherContactText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.otherContact} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize(`secondaryOtherContactText`) + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.secondaryOtherContact} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("emailText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.emailAddress} title={this.props.userDetails.emailAddress} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize(`secondaryEmailText`) + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.secondaryEmailAddress} title={this.props.userDetails.secondaryEmailAddress} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("lastNameText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.lastName} title={this.props.userDetails.lastName} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("keywordsText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.getKeywordsString()} title={this.getKeywordsString()} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("organizationText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.organization} title={this.props.userDetails.organization} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("specialtyText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.specialty} title={this.props.userDetails.specialty} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("graduateDegreeProgramText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.graduateDegreeProgram} title={this.props.userDetails.graduateDegreeProgram} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize(`npsDegreeProgramText`) + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.npsDegreeProgram} title={this.props.userDetails.npsDegreeProgram} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("deptofStudyText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.deptOfStudy} title={this.props.userDetails.deptOfStudy} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("currentOrganizationText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.currentOrganization} title={this.props.userDetails.currentOrganization} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("underGraduateDegreeText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.underGraduateDegree} title={this.props.userDetails.underGraduateDegree} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("professionalCertificatesText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.professionalCertificates} title={this.props.userDetails.professionalCertificates} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("professionalOrganizationsText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.professionalOrganizations} title={this.props.userDetails.professionalOrganizations} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("professionalExperienceText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.professionalExperience} title={this.props.userDetails.professionalExperience} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("professionalPublicationsText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.professionalPublications} title={this.props.userDetails.professionalPublications} />
                        </Flex>
                        <Flex gap="gap.small">
                            <Flex.Item styles={this.previewLabel}>
                                <Text weight="semibold" content={this.localize("resumeLinkText") + ":"} />
                            </Flex.Item>
                            <Text truncated content={this.props.userDetails.resumeCVLink} title={this.props.userDetails.resumeCVLink} />
                        </Flex>
                    </Flex>
                </Flex>
                <Flex gap="gap.smaller">
                    <Flex.Item push>
                        <Button content={this.localize("backButtonText")} id="backbtn" onClick={() => this.props.onBackClick(this.props.userDetails)} />
                    </Flex.Item>
                    <Button className="athena-button" content={this.props.userDetails.tableId ? this.localize("updateBtnText") : this.localize("saveButtonText")} onClick={this.props.userDetails.tableId ? this.updateUserAsync : this.createUserAsync} id="updatebtn" loading={this.state.isCreatingOrUpdating} disabled={this.state.isCreatingOrUpdating} />
                </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(PreviewUserDetails);