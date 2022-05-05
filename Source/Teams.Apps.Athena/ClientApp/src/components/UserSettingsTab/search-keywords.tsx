// <copyright file="search-keywords.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { Flex, Text, Button, FormField, Divider } from '@fluentui/react-northstar';
import IUserSettings from '../../models/user-settings';
import IKeyword from "../../models/keyword";
import KeywordSearchDropdown from '../common/keyword-search-dropdown/keyword-search-dropdown';
import { cloneDeep } from 'lodash';

interface IUserSettingsPage2Props extends WithTranslation {
    userDetails: IUserSettings,
    keywords: IKeyword[],
    onBackClick: (userDetails: IUserSettings) => void,
    onNextClick: (userDetails: IUserSettings) => void
}

export interface ISearchKeywordState {
    isNextPage: boolean,
    selectedKeywords: IKeyword[];
    errorMessage: string,
}

class SearchKeywords extends React.Component<IUserSettingsPage2Props, ISearchKeywordState> {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;

        let keywordIdsStringArray = this.props.userDetails.keywords.map(String);

        let selectedKeywords: IKeyword[] = this.props.keywords
            .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId));

        this.state = {
            isNextPage: false,
            selectedKeywords: selectedKeywords,
            errorMessage:"",
        };

    }

    // On back button click.
    onBackClick = () => {
        let userDetails = cloneDeep(this.props.userDetails);
        userDetails.keywordsJson = cloneDeep(this.state.selectedKeywords);
        userDetails.keywords = this.state.selectedKeywords?.map((keyword: IKeyword) => {
            return Number(keyword.keywordId)
        });
        this.props.onBackClick(userDetails);
    }

    // On next button click.
    onNextButtonClick = () => {
        if (!this.state.selectedKeywords?.length) {
            this.setState({ errorMessage: this.localize("userSettingsKeywordsRequiredError") });
            return;
        }

        let userDetails = cloneDeep(this.props.userDetails);
        userDetails.keywordsJson = cloneDeep(this.state.selectedKeywords);
        userDetails.keywords = this.state.selectedKeywords?.map((keyword: IKeyword) => {
            return Number(keyword.keywordId)
        });

        this.props.onNextClick(userDetails);
    }

    /**
     * Event handler called when keyword selection get changed.
     * @param selectedKeywords The array of selected keywords.
     */
    getSlectedKeywords = (selectedKeywords: IKeyword[]) => {
        this.setState({
            errorMessage: "",
            selectedKeywords: selectedKeywords
        });
    }

    // render a component.
    render() {
        return (
            <Flex column gap="gap.medium" className="task-module-container">
                <Text className="page-heading" content={this.localize("searchKeywordsText")} />
                <Divider size={2} color="brand" />
                <Text content={this.localize("searchKeywordPageContentText")} />

                <Flex gap="gap.smaller" className="form-fields">
                    <Flex.Item grow>
                        <FormField
                            className="input-field-lable"
                            control={<KeywordSearchDropdown
                                keywords={this.props.keywords}
                                showSlectedKywordPills={true}
                                label={`${this.localize('keywordsText')}*`}
                                getSelectedKywords={this.getSlectedKeywords}
                                selectedKeywords={this.state.selectedKeywords}
                            />}
                            errorMessage={this.state.errorMessage}
                        />
                    </Flex.Item>
                </Flex>

                <Flex gap="gap.smaller">
                    <Flex.Item push>
                        <Button content={this.localize("backButtonText")} id="search-page-backbtn" onClick={this.onBackClick} />
                    </Flex.Item>
                    <Button className="athena-button" content={this.localize("nextButtonText")} id="search-page-nextbtn" onClick={this.onNextButtonClick} />
                </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(SearchKeywords);