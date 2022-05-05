// <copyright file="searchable-dropdown.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { DropdownItemProps, Flex, Pill, MenuButton, Input, Text, SearchIcon, ChevronDownIcon, Loader, ButtonProps, Button } from "@fluentui/react-northstar";
import { AxiosResponse } from "axios";
import { cloneDeep } from "lodash";
import { StatusCodes } from "http-status-codes";

import "./searchable-dropdown.scss";

interface ISearchableFormDropdownProps<T> {
    label?: string;
    placeholder: string;
    multiple: boolean;
    fluid: boolean;
    noResultsMessage: string;
    valuePropertyName: string;
    headerPropertyName: string;
    selectedItems?: any[],
    showSelectedItemsOutside?: boolean,
    apiToExcute: (searchQuery: string, handleTokenAccessFailure: (error: string) => void) => Promise<AxiosResponse<any>>,
    handleTokenAccessFailure: (error: string) => void,
    onChange: (selectedItems: any[]) => void;
    width?: number;
    isSearchIcon?: boolean;
    searchButtonProps?: ButtonProps;
    onSearchButtonClick?: (searchQuery: string, selectedItemValue: any) => void;
    inverted?: boolean;
}

const SearchDropdownTimeoutInMilliseconds: number = 2000;
const MaxItemsInDropdown: number = 40;

const SearchableFormDropdown = <T,>(props: ISearchableFormDropdownProps<T>) => {
    const [searchQuery, setSearchQuery] = React.useState("");
    const [dropdownItems, setDropdownItems] = React.useState([] as any[]);
    const [selectedDropdownItems, setSelectedDropdownItems] = React.useState(props.selectedItems ?? []);
    const [searchTimeoutId, setSearchTimeoutId] = React.useState(-1);
    const [isLoading, setLoading] = React.useState(true);
    const [initialData, setInitialData] = React.useState([] as DropdownItemProps[]);
    const [isDropdownOpen, setDropdownOpen] = React.useState(false);
    const [selectedItemValue, setSelectedItemValue] = React.useState(undefined as any | undefined);

    let isComponentMounted = React.useRef(true);

    React.useEffect(() => {
        getData("", true);

        return () => {
            isComponentMounted.current = false;
        }
    }, []);

    React.useEffect(() => {
        if (isComponentMounted.current) {
            setSelectedDropdownItems(props.selectedItems ?? []);
        }
    }, [props.selectedItems]);

    /**
     * Gets the data from an API to be loaded in dropdown.
     * @param searchQuery The search query text.
     * @param isInitialData Indicates that whether the data being loaded is treated as initial data in dropdown.
     */
    const getData = async (searchQuery: string, isInitialData: boolean = false) => {
        let response = await props.apiToExcute(searchQuery, props.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let dropdownItems = getDropdownItems(response.data as T[], selectedDropdownItems as T[]);

            if (isComponentMounted.current === true) {
                setDropdownItems(dropdownItems);

                if (isInitialData) {
                    setInitialData(dropdownItems);
                }
            }
        }

        if (isComponentMounted.current === true) {
            setLoading(false);
        }
    }

    /**
     * Prepares the API data in the form of dropdown items.
     * @param data The API data of which dropdown items to get.
     */
    const getDropdownItems = (data: T[], selectedItems: T[]): any[] => {
        let dropdownItems: any[] = [];

        data = data.slice(0, MaxItemsInDropdown);

        dropdownItems = data.map((value: T) => {
            return {
                index: value[props.valuePropertyName],
                content: value[props.headerPropertyName],
            } as any
        });

        return dropdownItems;
    }

    /**
     * Event handler called when search query gets changed in dropdown.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onSearchQueryChange = (eventDetails: any, eventData: any) => {
        setSelectedItemValue(undefined);

        if (eventData?.value?.trim() === "" && eventData.value.length > 0) {
            return;
        }

        if (eventData?.value?.trim() === "") {
            setSearchQuery(eventData?.value ?? "");

            if (!initialData?.length) {
                getData("", true);
            }
            else {
                setDropdownItems(initialData);
            }

            return;
        }

        setDropdownOpen(true);
        setLoading(true);

        setSearchQuery(eventData?.value ?? "");
        setDropdownItems([]);

        if (searchTimeoutId) {
            clearTimeout(searchTimeoutId);
        }

        setSearchTimeoutId(window.setTimeout(() => {
            getData(eventData.value?.trim() ?? "");
        }, SearchDropdownTimeoutInMilliseconds));
    }

    /**
     * Event handler called when any dropdown item is get selected.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onChange = (eventDetails: any, eventData: any) => {
        setDropdownOpen(false);
        setSearchQuery("");
        onSearchQueryChange(undefined, { value: "" });

        if (props.showSelectedItemsOutside) {
            let selectedItemsHasElement = selectedDropdownItems.some(x => x.index === eventData.index);

            if (!selectedItemsHasElement) {
                let items = [...selectedDropdownItems, eventData] as any[] ?? [];

                setSelectedDropdownItems(items);
                props.onChange(items);
            }
        }
        else {
            setSelectedDropdownItems([eventData] as any[] ?? []);
            props.onChange([eventData] as any[] ?? []);
        }
    }

    /**
     * Event handler called when any selected dropdown item gets unselected.
     * @param dismissedItemValue The value of dropdown item to be unselected.
     */
    const onSelectedItemDismiss = (dismissedItemValue: any) => {
        let selectedItems = cloneDeep(selectedDropdownItems);
        selectedItems = selectedItems.filter((selectedItem: any) => selectedItem.index !== dismissedItemValue);

        setSelectedDropdownItems(selectedItems as any[] ?? []);
        props.onChange(selectedItems as any[] ?? []);
    }

    // Renders the selected dropdown items.
    const renderSelectedItemsOutside = React.useMemo(() => {
        let selectedItems = selectedDropdownItems.map((selectedDropdownItem: any, index: number) => {
            return <Pill key={`searchable-dropdown-selected-item-${index}`} actionable rectangular size="smaller" onDismiss={() => onSelectedItemDismiss(selectedDropdownItem.index)}>
                {selectedDropdownItem.content}
            </Pill>
        });

        return <React.Fragment>
            {selectedItems}
        </React.Fragment>
    }, [selectedDropdownItems]);

    // Gets the style for width.
    const getDropdownWidthClass = () => {
        switch (props.width) {
            case 220:
                return "width-220";
            default:
                return "width-20";
        }
    }

    // Event handler called whn click on dropdown icon.
    const onDropdownIconClick = () => {
        setDropdownOpen(true);
    }

    /**
     * Event handler called when dropdown open change.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onOpenChange = (eventDetails: any, eventData: any) => {
        setDropdownOpen(eventData?.open);
    }

    // Event handler called when click on search button.
    const onSearchButtonClick = () => {
        if (props.onSearchButtonClick) {
            props.onSearchButtonClick(searchQuery, selectedItemValue?.index);
        }
    }
    /**
     * Event handler called when dropdown selected item change on simple dropdown.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onSimpleDropdownChange = (eventDetails: any, eventData: any) => {
        setSearchQuery(eventData?.content);
        setSelectedItemValue(eventData);
    }

    if (props.showSelectedItemsOutside) {
        return <Flex className={`searchable-dropdown ${props.fluid ? "fill" : getDropdownWidthClass()}`} column wrap>
            <Text content={props.label} />
            <MenuButton
                open={isDropdownOpen}
                menu={!isLoading && dropdownItems.length === 0 ? [{ content: props.noResultsMessage }] : dropdownItems}
                trigger={<Input
                    fluid={props.fluid ? true : false}
                    placeholder={props.placeholder}
                    value={searchQuery}
                    onChange={onSearchQueryChange}
                    icon={isLoading ? <Loader size="small" /> : props.isSearchIcon ? <SearchIcon /> : <ChevronDownIcon className="icon" onClick={onDropdownIconClick} />}
                />}
                onMenuItemClick={onChange}
                onOpenChange={onOpenChange}
            />
            <Flex vAlign="center" wrap>
                {renderSelectedItemsOutside}
            </Flex>
        </Flex>
    }

    return <Flex className={`searchable-dropdown ${props.fluid ? "fill" : getDropdownWidthClass()}`} gap="gap.small">
        {props.label && <Text content={props.label} />}
        <MenuButton
            open={isDropdownOpen}
            menu={!isLoading && dropdownItems.length === 0 ? [{ content: props.noResultsMessage, disabled: true }] : dropdownItems}
            trigger={<Input
                disabled={props.searchButtonProps?.disabled}
                fluid={props.fluid ? true : false}
                placeholder={props.placeholder}
                value={searchQuery}
                onChange={onSearchQueryChange}
                inverted={props.inverted}
                icon={isLoading ? <Loader size="small" /> : props.isSearchIcon ? <SearchIcon /> : <ChevronDownIcon className="icon" onClick={onDropdownIconClick} />}
            />}
            onMenuItemClick={onSimpleDropdownChange}
            onOpenChange={onOpenChange}
        />
        {props.searchButtonProps && <Button {...props.searchButtonProps} onClick={onSearchButtonClick} />}
    </Flex>;
}

export default SearchableFormDropdown;