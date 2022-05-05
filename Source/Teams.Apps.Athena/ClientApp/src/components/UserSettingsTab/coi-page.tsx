// <copyright file="coi-page.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { ICoiEntity } from '../../models/coi-entity';
import * as microsoftTeams from "@microsoft/teams-js";
import { Button, Flex, FormField, Input, SearchIcon, Divider, Text, Table, Checkbox } from '@fluentui/react-northstar';
import IUserSettings from '../../models/user-settings';
import { getApprovedCoiRequestAsync } from '../../api/coi-request-api';
import IKeyword from '../../models/keyword';
import Loader from "../common/loader/loader";

import "./coi-page.scss";

interface ICoiPageProps extends WithTranslation {
    onBackClick: (userDetails: IUserSettings) => void,
    onNextClick: (userDetails: IUserSettings) => void,
    userDetails: IUserSettings,
    keywords: IKeyword[]
}

export interface ICoiState {
    coiEntity: ICoiEntity[];
    isNextPage: boolean,
    searchText: string,
    filterArray: ICoiEntity[],
    checkedAll: boolean,
    loader: boolean;
    existingCOIList: ICoiEntity[]
}

class CommunityOfInterest extends React.Component<ICoiPageProps, ICoiState> {
    localize: TFunction;
    header: any;
    existingCOIHeader: any;
    selectedCOIs = [] as ICoiEntity[];
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            coiEntity: [],
            isNextPage: false,
            searchText: "",
            filterArray: [],
            checkedAll: false,
            loader: true,
            existingCOIList: (this.props.userDetails?.communityOfInterests?.length || 0) > 0 ? JSON.parse(this.props.userDetails!.communityOfInterests!) as ICoiEntity[] : [] as ICoiEntity[]
        };

        this.header = {
            key: "header",
            items: [
                {
                    content: <Checkbox checked={this.state.checkedAll} onChange={() => this.setSelectAllItems()} />,
                    styles: { minWidth: "4rem", maxWidth: "4rem" }
                },
                {
                    content: <Text content={this.localize("communityOfInterestText")} />,
                },
                {
                    content: <Text content={this.localize("keywordsText")} />
                },
            ]
        };
        this.existingCOIHeader = {
            key: "existingcoiheader",
            items: [
                {
                    content: <Text content={this.localize("communityOfInterestText")} />,
                },
                {
                    content: <Text content="" />,
                    styles: { minWidth: "15rem", maxWidth: "15rem" }
                },
            ]
        };
    }

    public componentDidMount() {
        microsoftTeams.initialize();
        this.getApprovedCoiRequest();
    }

    public getApprovedCoiRequest = async () => {
        let selectedKeywords = this.props.userDetails.keywordsJson!;

        let existingCoiTableIDs = this.state.existingCOIList.map((coi: ICoiEntity) => { return coi.tableId });

        let response = await getApprovedCoiRequestAsync(selectedKeywords, false);
        if (response && response.status === 200 && response.data.length > 0) {
            if (this.state.existingCOIList.length > 0) {
                let cois = [] as ICoiEntity[];
                for (var i = 0; i < response.data.length; i++) {
                    if (!existingCoiTableIDs.find((coiTableId: string) => response.data[i].tableId === coiTableId)) {
                        cois.push(response.data[i] as ICoiEntity);
                    }
                }
                this.setState({ coiEntity: cois, filterArray: cois, loader: false });
            }
            else {
                this.setState({ coiEntity: response.data, filterArray: response.data, loader: false });
            }
        }
        else {
            this.setState({ loader: false });
        }
    }

    /**
     * Sets the selected items array
     * @param index
     */
    setSelectedItemsArray = (index: number) => {
        let requestsData: ICoiEntity[] = [...this.state.coiEntity];
        requestsData[index].isChecked = !requestsData[index].isChecked;
        this.setState({ coiEntity: requestsData });
    }

    // Sets the all items array
    setSelectAllItems = () => {
        let isCheckAll = !this.state.checkedAll;
        let requestsData: ICoiEntity[] = [...this.state.coiEntity];
        requestsData.map((value: ICoiEntity) => value.isChecked = isCheckAll);
        this.setState({ checkedAll: isCheckAll, coiEntity: requestsData });
    }

    // On next button click.
    onNextButtonClick = () => {
        let checkedCoi = this.state.coiEntity.filter((coi: ICoiEntity) => coi.isChecked === true);
        let existingCOI = [...this.state.existingCOIList];
        for (var i = 0; i < checkedCoi.length; i++) {
            existingCOI.push(checkedCoi[i]);
        }
        let userSettings = { ...this.props.userDetails };
        userSettings.communityOfInterests = JSON.stringify(existingCOI);
        this.props.onNextClick(userSettings);
    }

    // On back button click.
    onBackClick = () => {
        let checkedCoi = this.state.coiEntity.filter((coi: ICoiEntity) => coi.isChecked === true);
        let userSettings = { ...this.props.userDetails };
        userSettings.communityOfInterests = JSON.stringify(checkedCoi);
        this.props.onBackClick(userSettings);
    }

    /**
    * Method to set search text given in the search box.
    */
    public handleSearchInputChange = (searchText: any) => {
        if (searchText.length === 0) {
            this.setState({
                searchText: "",
                filterArray: [...this.state.coiEntity]
            });
        }
        else {
            var filtered = this.state.coiEntity.filter((coi: ICoiEntity) => coi.coiName?.toUpperCase().includes(searchText.toUpperCase()) === true);

            this.setState({
                searchText: searchText,
                filterArray: [...filtered]
            })
        }
    }

    private removeExistingCOI = (coiTableId: string) => {
        let coiList = [...this.state.existingCOIList];
        let filtered = coiList.filter((coi: ICoiEntity) => coi.tableId !== coiTableId);

        this.setState({ existingCOIList: filtered });
    }

    private getTableRows = () => {
        const rows = this.state.filterArray.map((coi: ICoiEntity, index: number) => {
            let keywordIdsStringArray = coi.keywords.map(String);

            let keywords = this.props.keywords
                .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId))
                .map((keyword: IKeyword) => keyword.title).join(", ");

            return {
                "key": index,
                "items": [
                    {
                        content: <Checkbox checked={coi.isChecked} onChange={() => this.setSelectedItemsArray(index)} />,
                        styles: { minWidth: "4rem", maxWidth: "4rem" }
                    },
                    {
                        content: coi.coiName,
                        title: coi.coiName,
                        truncateContent: true
                    },
                    {
                        content: keywords,
                        title: keywords,
                        truncateContent: true
                    },
                ]
            };

        });

        return rows;
    }

    private getExistingCOITableRows = () => {
        const rows = this.state.existingCOIList.map((coi: ICoiEntity, index: number) => {
            return {
                "key": index,
                "items": [
                    {
                        content: coi.coiName,
                        title: coi.coiName,
                        truncateContent: true
                    },
                    {
                        content: <Button content="Leave" text onClick={() => this.removeExistingCOI(coi.tableId!)} />,
                        styles: { minWidth: "15rem", maxWidth: "15rem" }
                    },
                ]
            };

        });

        return rows;
    }

    // Renders the component
    render(): JSX.Element {
        if (this.state.loader) {
            return (<Flex hAlign="center" vAlign="center" column gap="gap.medium" className="task-module-container">
                <Loader />
            </Flex>)
        }
        else {
            return (
                <Flex column gap="gap.medium" className="task-module-container">
                    <Text className="page-heading" content={this.localize("userSettingsCommunityOfInterestTitle")} />
                    <Divider size={2} color="brand" />
                    <Flex column gap="gap.small" className="form-fields">
                        {this.state.coiEntity.length > 0 && <>
                            <Flex>
                                <Flex.Item push>
                                    <FormField>
                                        <Input icon={<SearchIcon />} className="coi-search-box" placeholder={this.localize("coiSearchBarPlaceholder")} onChange={(event: any) => this.handleSearchInputChange(event.target.value)} fluid />
                                    </FormField>
                                </Flex.Item>
                            </Flex>

                            {this.state.coiEntity.length > 0 && <Table header={this.header} rows={this.getTableRows()} />}
                        </>
                        }
                        {this.state.coiEntity.length === 0 && <Text content="No COI to show" />}
                        <Flex gap="gap.small" column>
                            <Text weight="semibold" content="Existing COIs" />
                            {this.state.existingCOIList.length > 0 && <Table styles={{ width: "100%" }} header={this.existingCOIHeader} rows={this.getExistingCOITableRows()} />}
                            {this.state.existingCOIList.length === 0 && <Text content="No existing COI present" />}
                        </Flex>
                    </Flex>

                    <Flex gap="gap.smaller">
                        <Flex.Item push>
                            <FormField>
                                <Button content={this.localize("backButtonText")} id="backbtn" onClick={this.onBackClick} />
                            </FormField>
                        </Flex.Item>
                        <Button className="athena-button" content={this.localize("nextButtonText")} id="nextbtn" onClick={this.onNextButtonClick} />
                    </Flex>
                </Flex>
            );
        }
    }
}

export default withTranslation()(CommunityOfInterest);