// <copyright file="discovery-filter.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { useTranslation } from 'react-i18next';
import { Text, Flex, Checkbox, Button, Divider, CloseIcon } from "@fluentui/react-northstar";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { IDiscoveryTreeFilter } from "../../../models/discovery-filter";
import { cloneDeep } from "lodash";

import "./discovery-filter.scss";

const FilterGroupParentId: number = 0;

interface IDiscoveryFilterProps extends RouteComponentProps {
    handleClickOutside: () => void;
    handleCloseButtonClick: () => void;
    initialFilterData: IDiscoveryTreeFilter[];
    handleApplyButtonClick: (filterData: IDiscoveryTreeFilter[]) => void;
}

const DiscoveryFilter: React.FunctionComponent<IDiscoveryFilterProps> = (props) => {

    const localize = useTranslation().t;
    const filterRef = React.useRef(null);
    const [initialFilterData, setInitialFilterData] = React.useState<IDiscoveryTreeFilter[]>([]);
    const [filterData, setFilterData] = React.useState<IDiscoveryTreeFilter[]>([]);
    const [isSelectAll, setIsSelectAll] = React.useState<boolean>(false);

    React.useEffect(() => {
        let filterData = cloneDeep(props.initialFilterData);
        let filterGroups = filterData.filter(x => x.parentId === FilterGroupParentId);
        filterGroups.forEach((group: any) => {
            let filtersValues = filterData.filter(x => x.parentId === group.filterId);
            filtersValues.map((filter: IDiscoveryTreeFilter) => {
                let index = filterData.findIndex(filterItem => filterItem.filterId === filter.filterId);
                filterData[index].isChecked = filter.isVisible;
            })
        });
        setInitialFilterData(filterData);
    }, [props.initialFilterData])

    React.useEffect(() => {
        getFilters();
    }, [initialFilterData])

    /**
     * Closes the filter dropdown if clicked outside.
     * @param ref The ref.
     */
    const useOutsideAlerter = (ref) => {
        React.useEffect(() => {
            // Function for click event.
            function handleOutsideClick(event) {
                if (ref.current && !ref.current.contains(event.target)) {
                    props.handleClickOutside()
                }
            }
            // Adding click event listener.
            document.addEventListener("click", handleOutsideClick);
        }, [ref]);
    }

    useOutsideAlerter(filterRef);

    /**
    * Sets the selected configure filter.
    * @param filterId Selected filter item Id.
    */
    const setSelectedFilter = (filterId: number) => {
        let existingData: any = cloneDeep(initialFilterData);
        let index = existingData.findIndex(filter => filter.filterId === filterId);
        existingData[index].isChecked = !existingData[index].isChecked;
        existingData[index].isVisible = existingData[index].isChecked ? true : false;
        setInitialFilterData(existingData);
    }

    // Removes all the selected items.
    const handleClearButtonClick = () => {
        let existingData = cloneDeep(initialFilterData);
        existingData.forEach((item: any) => {
            item.isChecked = false;
            item.isVisible = false;
        });
        setInitialFilterData(existingData);
    }

    // Handles apply button click.
    const handleApplyButtonClick = () => {
        props.handleApplyButtonClick(initialFilterData);
        props.handleCloseButtonClick();
    }

    // Returns the filter data.
    const getFilters = () => {
        let filterData: IDiscoveryTreeFilter[] = [];
        let filterGroups = initialFilterData.filter(x => x.parentId === 0);
        filterGroups.forEach((group: any) => {
            let filtersValues = initialFilterData.filter(x => x.parentId === group.filterId);
            filtersValues.map((filter: IDiscoveryTreeFilter) => {
                filterData.push(filter);
            })
        });
        let index = filterData.findIndex(filter => filter.isChecked === false);
        if (index === -1) {
            setIsSelectAll(true);
        }
        else {
            setIsSelectAll(false);
        }
        setFilterData(filterData);
    }

    /**
     * Handles select all button click.
     * @param e The event.
     * @param v The v.
     */
    const setSelectAllFilter = (e: any, v: any) => {
        setIsSelectAll(v.checked);
        let existingData = cloneDeep(initialFilterData);
        filterData.map(filter => {
            let index = existingData.findIndex(filterItem => filterItem.filterId === filter.filterId);
            existingData[index].isChecked = v.checked;
            existingData[index].isVisible = v.checked;
        })
        setInitialFilterData(existingData);
    }

    return (
        <div className="discovery-filter-container" ref={filterRef}>
            <Flex vAlign="center" className="discovery-filter-header">
                <Text content={localize("configureFilterText")} weight="bold" size="large" />
                <Flex.Item push>
                    <CloseIcon size="medium" onClick={props.handleCloseButtonClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Divider className="filter-dropdown-divider" />
            <Flex column padding="padding.medium" className="discovery-filter-list">             
                <Flex column gap="gap.medium" className="discovery-filter-item">
                    <Flex vAlign="center">
                        <Text content={localize("filtersText")} weight="bold" />
                        <Flex.Item push>
                            <Checkbox className="keyword-list-checkbox" checked={isSelectAll} onChange={(e: any, v: any) => setSelectAllFilter(e, v)} />
                        </Flex.Item>
                    </Flex>
                    {
                        filterData!.map((item: any, index: number) => {
                            return (
                                <Flex vAlign="center" key={index}>
                                    <Text content={item.title} />
                                    <Flex.Item push>
                                        <Checkbox className="keyword-list-checkbox" checked={item.isChecked} onChange={() => setSelectedFilter(item.filterId)} />
                                    </Flex.Item>
                                </Flex>
                            )
                        })
                    }
                </Flex>
            </Flex>
            <Divider className="filter-dropdown-divider" />
            <Flex vAlign="center" className="discovery-filter-footer" space="between">
                <Button primary text content={localize("clearAllButtonContent")} className="clear-all-button" onClick={handleClearButtonClick} />
                <Flex gap="gap.smaller">
                    <Button content={localize("cancelButtonContent")} onClick={props.handleCloseButtonClick} />
                    <Button className="athena-button" content={localize("applyButtonContent")} onClick={handleApplyButtonClick} />
                </Flex>
            </Flex>
            
        </div>
    )
};

export default withRouter(DiscoveryFilter);