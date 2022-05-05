// <copyright file="news-type-filter-popup.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { useTranslation } from 'react-i18next';
import { Text, Flex, Checkbox, Button, Divider } from "@fluentui/react-northstar";
import IDiscoveryTreeNodeType from "../../../models/discovery-tree-node-type";
import { cloneDeep } from "lodash";

import "./news-type-filter-popup.scss";

interface INewsTypeFilterPopupProps {
    getSelectedNewsTypes: (newsTypes: IDiscoveryTreeNodeType[]) => void;
    handleClickOutside: () => void;
    selectedNewsTypes: IDiscoveryTreeNodeType[];
    newsTypes: IDiscoveryTreeNodeType[];
}

const NewsTypeFilterPopup: React.FunctionComponent<INewsTypeFilterPopupProps> = (props) => {

    const localize = useTranslation().t;
    const filterRef = React.useRef(null);
    const [newsTypes, setNewsTypes] = React.useState<IDiscoveryTreeNodeType[]>([]);

    React.useEffect(() => {
        getNewsTypes();
    }, []);

    /**
     * Closes the filter dropdown if clicked outside 
     * @param ref The ref.
     */
    const useOutsideAlerter = (ref) => {
        React.useEffect(() => {
            // Function for click event
            function handleOutsideClick(event) {
                if (ref.current && !ref.current.contains(event.target)) {
                    props.handleClickOutside()
                }
            }
            // Adding click event listener
            document.addEventListener("click", handleOutsideClick);
        }, [ref]);
    }

    useOutsideAlerter(filterRef);

    // Gets the news type data.
    const getNewsTypes = () => {
        var newsTypes: IDiscoveryTreeNodeType[] = [];
        var existingNewsTypes = cloneDeep(props.newsTypes);
        existingNewsTypes.map((newsType: IDiscoveryTreeNodeType) => {
            newsTypes.push({ ...newsType, isChecked: props.selectedNewsTypes.some(selectedNewsType => selectedNewsType.nodeTypeId === newsType.nodeTypeId) ? true : false })
        })
        setNewsTypes(newsTypes)
    }

    /**
     * Sets the selected news type array.
     * @param index Index of selected news type.
     */
    const setSelectedNewsTypeArray = (index: number) => {
        let existingNewsTypeData = cloneDeep(newsTypes);
        existingNewsTypeData[index].isChecked = !existingNewsTypeData[index].isChecked;
        setNewsTypes(existingNewsTypeData);
        props.getSelectedNewsTypes(existingNewsTypeData);
    }

    // Removes all the selected keywords
    const handleClearButtonClick = () => {
        let existingNewsTypeData = cloneDeep(newsTypes);
        existingNewsTypeData.forEach((newsSource: IDiscoveryTreeNodeType) => {
            newsSource.isChecked = false;
        });
        setNewsTypes(existingNewsTypeData);
        props.getSelectedNewsTypes(existingNewsTypeData);
    }

    return (
        <div className="filter-dropdown-container" ref={filterRef}>
            <Flex vAlign="center" className="filter-header-container">
                <Text content={localize("newsTypeTitle")} weight="bold" />
                <Flex.Item push>
                    <Button text content={localize("clearText")} text-weight="regular" onClick={handleClearButtonClick} disabled={props.selectedNewsTypes.length === 0} />
                </Flex.Item>
            </Flex>
            <Divider className="filter-dropdown-divider" />
            <Flex column gap="gap.medium" padding="padding.medium" className="filter-list-container">
                {
                    newsTypes.length > 0 ?
                        newsTypes!.map((newsType: IDiscoveryTreeNodeType, index: number) => {
                            return (
                                <Flex vAlign="center" key={index}>
                                    <Checkbox checked={newsType.isChecked} onChange={() => setSelectedNewsTypeArray(index)} className="type-list-checkbox" />
                                    <Text content={newsType.title} className="type-text" />
                                </Flex>
                            )
                        })
                        :
                        <Text content={localize('noNewsTypesFoundError')} />
                }
            </Flex>
        </div>
    )
};

export default NewsTypeFilterPopup;