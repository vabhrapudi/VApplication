// <copyright file="keyword-search-dropdown.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { DropdownItemProps, Flex, Pill, Dropdown, Text } from "@fluentui/react-northstar";
import { cloneDeep } from "lodash";
import IKeyword from "../../../models/keyword";
import Constants from "../../../constants/constants";
import { useTranslation } from 'react-i18next';

interface IKeywordSearchDropdownProps {
    keywords: IKeyword[],
    selectedKeywords: IKeyword[],
    showSlectedKywordPills: boolean,
    getSelectedKywords: (selectedKeywords: IKeyword[]) => void,
    disabled?: boolean,
    label?: string
}

const KeywordSearchDropdown: React.FunctionComponent<IKeywordSearchDropdownProps> = (props: IKeywordSearchDropdownProps) => {

    const localize = useTranslation().t;
    const [searchResults, setSearchResults] = React.useState<DropdownItemProps[]>([]);
    const [searchText, setSearchText] = React.useState<string>("");
    const [selectedKeywords, setSelectedKeywords] = React.useState<IKeyword[]>(props.selectedKeywords);

    React.useEffect(() => {
        setSelectedKeywords(props.selectedKeywords);
    }, [props.selectedKeywords]);

    /**
     * Event handler called when keyword selection get changed.
     * @param event The event.
     * @param dropdownProps The dropdown props.
     */
    const handleKeywordDropdownChange = (event: any, dropdownProps?: any) => {
        if (dropdownProps.value) {
            let existingSelectedKeywords = cloneDeep(selectedKeywords);
            let isPresent = existingSelectedKeywords.some((keyword: IKeyword) => keyword.keywordId === dropdownProps.value!.key);
            if (!isPresent) {
                existingSelectedKeywords.push({ keywordId: dropdownProps.value!.key, title: dropdownProps.value!.header });
                setSelectedKeywords(existingSelectedKeywords);
                props.getSelectedKywords(existingSelectedKeywords);
            }
            setSearchText("");
        }
    }

    /**
     * Intiates the search when query string of dropdown changes.
     * @param searchQuery The search query.
     */
    const initiateSearch = (searchQuery: string) => {
        setSearchText(searchQuery);
        var result: IKeyword[] = [];
        if (searchQuery) {
            result = props.keywords.filter((keyword: IKeyword) => keyword.title.toLowerCase().includes(searchQuery.toLowerCase()));
        }
        else {
            result = cloneDeep(props.keywords);
        }
        let searchResults: DropdownItemProps[] = result.slice(0, Constants.KeywordSearchResultMaxCount)
            .sort((a: IKeyword, b: IKeyword) => a.title.localeCompare(b.title))
            .map((keyword: IKeyword) => {
                return {
                    key: keyword.keywordId,
                    value: keyword.title,
                    header: keyword.title,
                } as DropdownItemProps;
            });
        setSearchResults(searchResults);
    }

    /**
     * Deselects the selected keyword.
     * @param selectedKeyword The selected keyword.
     */
    const deselectSelectedKeyword = (selectedKeyword: IKeyword) => {
        let existingSelectedKeywords = cloneDeep(selectedKeywords);
        let index = existingSelectedKeywords.findIndex((keyword: IKeyword) => keyword.keywordId === selectedKeyword.keywordId);
        if (index !== -1) {
            existingSelectedKeywords.splice(index, 1);
            setSelectedKeywords(existingSelectedKeywords);
            props.getSelectedKywords(existingSelectedKeywords);
        }
    }

    return (
        <Flex column gap="gap.small" vAlign="center">
            {props.label && <Text content={props.label} />}
            <Dropdown
                search
                fluid
                placeholder={localize("findText")}
                items={searchResults}
                onSearchQueryChange={(e, { searchQuery }) => {
                    initiateSearch(searchQuery!);
                }}
                onChange={handleKeywordDropdownChange}
                noResultsMessage={localize("noKeywordsFoundDropdownMessage")}
                loadingMessage={localize("loadingLabel")}
                value={searchText}
                disabled={props.disabled}
                toggleIndicator={false}
            />
            {
                props.showSlectedKywordPills && selectedKeywords.length > 0 &&
                <Flex vAlign="center" wrap>
                    {
                        selectedKeywords.map((selectedKeyword: IKeyword, index: number) => {
                            return <Pill key={index} actionable rectangular size="smaller" onDismiss={() => deselectSelectedKeyword(selectedKeyword)}>
                                {selectedKeyword.title}
                            </Pill>
                        })
                    }
                </Flex>
            }
        </Flex>
        )
}

export default KeywordSearchDropdown;