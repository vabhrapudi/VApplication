// <copyright file="filter-popup.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { Flex, Checkbox, Text, Button, Divider } from "@fluentui/react-northstar";
import IFilterItem from "../../../models/filter-item";

import "./filter-popup.scss";

interface IFilterPopupProps {
    title: string;
    clearText: string;
    items: IFilterItem[];
    disabled: boolean;
    selectedFilterItemKeys: any[];
    onCheckedChange: (updatedFilterItemKeys: any[]) => void;
}

const FilterPopup: React.FunctionComponent<IFilterPopupProps> = props => {
    // Event handler called when filter item checked state change.
    const onFilterItemCheckedChange = (filterItemKey: string, isChecked: boolean) => {
        var filterItem: IFilterItem | undefined = props.items
            .find((filterItem: IFilterItem) => filterItem.key === filterItemKey);

        if (filterItem) {
            let filterItems: string[] = [...props.selectedFilterItemKeys];

            if (isChecked) {
                filterItems.push(filterItem.key);
            }
            else {
                filterItems = filterItems.filter((item: string) => item !== filterItem?.key);
            }

            props.onCheckedChange(filterItems);
        }
    }

    // Event handler called when click on 'Clear' button to clear all selected filter items.
    const onClearSelectedItems = () => {
        props.onCheckedChange([]);
    }

    // Renders filter items.
    const renderFilterItems = () => {
        return props.items.map((filterItem: IFilterItem) =>
            <Flex vAlign="center">
                <Text content={filterItem.header?.trim()} />
                <Flex.Item push>
                    <Checkbox
                        disabled={props.disabled}
                        checked={props.selectedFilterItemKeys.indexOf(filterItem.key) > -1}
                        onChange={(event, eventData) => onFilterItemCheckedChange(filterItem.key, eventData?.checked ?? false)} />
                </Flex.Item>
            </Flex>
        );
    }

    return <Flex className="filter-popup" column fill>
        <Flex vAlign="center">
            <Text content={props.title} weight="semibold" />
            <Flex.Item push>
                <Button
                    className="clear-button"
                    text
                    primary
                    content={props.clearText} disabled={!props.selectedFilterItemKeys || props.selectedFilterItemKeys.length === 0 || props.disabled}
                    onClick={onClearSelectedItems}
                />
            </Flex.Item>
        </Flex>
        <Divider />
        { renderFilterItems() }
    </Flex>;
}

export default FilterPopup;