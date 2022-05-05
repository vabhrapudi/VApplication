// <copyright file="new-priority.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import { Flex, Button, Text, FormInput, TextArea, FormDropdown } from "@fluentui/react-northstar";
import ContentLoader from "../common/loader/loader";
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import IKeyword from "../../models/keyword";
import Constants from "../../constants/constants";
import { StatusCodes } from "http-status-codes";
import IPriority from "../../models/priority";
import { createPriorityAsync, getPriorityByIdAsync, updatePriorityAsync, getPriorityTypesAsync } from "../../api/priority-api";
import IPriorityType from "../../models/priority-type";
import { getAllKeywordsAsync } from "../../api/keyword-api";
import KeywordSearchDropdown from "../common/keyword-search-dropdown/keyword-search-dropdown";

interface INewPriorityProps extends RouteComponentProps {
}

const TitleMaxLength: number = 75;
const DescriptionMaxLength: number = 300;
const MaximumNumberOfPrioritiesUnderAType: number = 5;

const NewPriority: React.FunctionComponent<INewPriorityProps> = (props: INewPriorityProps) => {
    const localize = useTranslation().t;

    let urlParams = new URLSearchParams(window.location.search);
    let priorityIdToEdit = React.useRef(urlParams.get(Constants.UrlParamInsightsConfigPriorityId));

    const [isLoadingDetails, setLoadingDetails] = React.useState(priorityIdToEdit.current ? true : false);
    const [status, setStatus] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);
    const [isSubmitting, setSubmitting] = React.useState(false);
    const [selectedKeywords, setSelectedKeywords] = React.useState([] as IKeyword[]);
    const [priorityDetails, setPriorityDetails] = React.useState({} as IPriority);
    const [priorityTypes, setPriorityTypes] = React.useState([] as any[]);
    const [isLoadingPriorityTypes, setLoadingPriorityTypes] = React.useState(true);
    const [selectedPriorityType, setSelectedPriorityType] = React.useState(undefined as any | undefined);
    const [keywords, setKeywords] = React.useState([] as IKeyword[]);
    const [isLoadingKeywords, setLoadingKeywords] = React.useState(true);

    let isComponentUnmounted = React.useRef(false);

    React.useEffect(() => {
        microsoftTeams.initialize();
        getKeywords();
        getPriorityTypes();

        if (!isComponentUnmounted.current && priorityIdToEdit.current) {
            getPriorityDetailsAsync();
        }

        return () => {
            isComponentUnmounted.current = true;
        }
    }, []);

    React.useEffect(() => {
        if (priorityIdToEdit.current && !isLoadingPriorityTypes && !isLoadingDetails) {
            let priorityType = priorityTypes.find(item => item.value === priorityDetails.type);

            if (priorityType) {
                setSelectedPriorityType({ value: priorityType.value, header: priorityType.header });
            }
        }
    }, [isLoadingPriorityTypes, isLoadingDetails]);

    React.useEffect(() => {
        if (priorityIdToEdit.current && !isLoadingKeywords && !isLoadingDetails) {
            let priorityKeywords = priorityDetails?.keywords?.map(String);

            let keywordsToSelect = keywords
                .filter((keyword: IKeyword) => priorityKeywords?.includes(keyword.keywordId));

            setSelectedKeywords(keywordsToSelect);
        }
    }, [isLoadingKeywords, isLoadingDetails]);

    React.useEffect(() => {
        if (priorityIdToEdit.current && !isLoadingKeywords && !isLoadingPriorityTypes) {
            setLoadingDetails(false);
        }
    }, [isLoadingKeywords, isLoadingPriorityTypes]);

    // Gets priority types.
    const getPriorityTypes = async () => {
        let response = await getPriorityTypesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let types = response.data
                .map((type: IPriorityType, index: number) => { return { value: type.id, header: type.title } });

            if (!isComponentUnmounted.current) {
                setPriorityTypes(types);
            }
        }
        else {
            if (!isComponentUnmounted.current) {
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }

        if (!isComponentUnmounted.current) {
            setLoadingPriorityTypes(false);
        }
    }

    // Gets the keywords.
    const getKeywords = async () => {
        let response = await getAllKeywordsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setKeywords(response.data as IKeyword[]);
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setLoadingKeywords(false);
    }

    // Gets the priority details.
    const getPriorityDetailsAsync = async () => {
        let response = await getPriorityByIdAsync(priorityIdToEdit.current!, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (!isComponentUnmounted.current) {
                setPriorityDetails(response.data as IPriority);
            }
        }
        else {
            if (!isComponentUnmounted.current) {
                setLoadingDetails(false);
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
    }

    // Validates all form field. Returns 'true' if all required fields filled and valid.
    const validateFields = (): boolean => {
        if (!priorityDetails.title?.trim() || !priorityDetails.description?.trim() || !selectedKeywords?.length || !selectedPriorityType) {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("newHomeConfigItemFormFieldsRequiredError"), type: ActivityStatus.Error }));

            return false;
        }

        return true;
    }

    /**
     * Redirects to the sign-in page when accessing token is failed in API.
     * @param error The error message.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    /**
     * The event handler called when title gets changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onTitleChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        setPriorityDetails(prevState => ({ ...prevState, title: eventData.value }));
    }

    /**
     * The event handler called when description gets changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onDescriptionChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        setPriorityDetails(prevState => ({ ...prevState, description: eventData.value }));
    }

    // Event handler called when selected keywords gets changed.
    const getSlectedKeywords = React.useCallback((selectedKeywords: IKeyword[]) => {
        setSelectedKeywords(selectedKeywords);

        let keywordIds: number[] = selectedKeywords?.map(keyword => keyword.keywordId).map(Number) ?? [];
        setPriorityDetails(prevState => ({ ...prevState, keywords: keywordIds }));
    }, [selectedKeywords]);

    // Event handler called when priority details to be submitted.
    const onSubmit = async () => {
        let isAllFieldsValid: boolean = validateFields();

        if (!isAllFieldsValid) {
            return;
        }

        setSubmitting(true);

        if (priorityIdToEdit.current) {
            var response = await updatePriorityAsync(priorityDetails, handleTokenAccessFailure);

            if (response) {
                if (response.status === StatusCodes.OK) {
                    microsoftTeams.tasks.submitTask({ type: "updated" });
                    return;
                }

                if (response.status === StatusCodes.NOT_FOUND && !isComponentUnmounted.current) {
                    setSubmitting(false);
                    setStatus(prevState => ({ id: prevState.id + 1, message: localize("insightsConfigUpdatedFailed", { MaximumNumberOfPrioritiesUnderAType }), type: ActivityStatus.Error }));
                    return;
                }
            }
        }
        else {
            var response = await createPriorityAsync(priorityDetails, handleTokenAccessFailure);

            if (response) {
                if (response.status === StatusCodes.CREATED) {
                    microsoftTeams.tasks.submitTask({ type: "created" });
                    return;
                }

                if (response.status === StatusCodes.BAD_REQUEST && !isComponentUnmounted.current) {
                    setSubmitting(false);
                    setStatus(prevState => ({ id: prevState.id + 1, message: localize("insightsConfigCreateFailed", { MaximumNumberOfPrioritiesUnderAType }), type: ActivityStatus.Error }));
                    return;
                }
            }
        }

        if (!isComponentUnmounted.current) {
            setSubmitting(false);
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }
    }

    /**
     * Event handler called when priority type gets changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onChangePriorityType = (eventDetails: any, eventData: any) => {
        setSelectedPriorityType(eventData.value);
        setPriorityDetails(prevState => ({ ...prevState, type: eventData.value.value }));
    }

    if (isLoadingDetails) {
        return <ContentLoader className="task-module-container" />;
    }

    return (
        <Flex className="task-module-container" column fill gap="gap.medium">
            <StatusBar status={status} isMobile={false} />
            <Flex className="overflow-y" column gap="gap.medium" fill>
                <FormInput
                    label={localize("titleText")}
                    placeholder={localize("newRequestNewsTitleInputPlaceholder")}
                    maxLength={TitleMaxLength}
                    value={priorityDetails.title}
                    onChange={onTitleChange}
                    showSuccessIndicator={false}
                    disabled={isSubmitting}
                    required
                    fluid />
                <Flex column>
                    <Text content={`${localize("newRequestDescriptionInputLabel")}*`} />
                    <TextArea
                        design={{ height: "8.6rem", marginTop: ".4rem" }}
                        placeholder={localize("newRequestDescriptionPlaceholder")}
                        maxLength={DescriptionMaxLength}
                        value={priorityDetails.description}
                        onChange={onDescriptionChange}
                        disabled={isSubmitting}
                        fluid />
                </Flex>
                <KeywordSearchDropdown
                    keywords={keywords}
                    showSlectedKywordPills={true}
                    label={`${localize('keywordsText')}*`}
                    getSelectedKywords={getSlectedKeywords}
                    selectedKeywords={selectedKeywords}
                />
                <FormDropdown
                    label={`${localize("priorityTypeLabel")}*`}
                    placeholder={localize("insightsConfigSelectPriorityTypeDropdownPlaceholder")}
                    loading={isLoadingPriorityTypes}
                    loadingMessage={localize("loadingLabel")}
                    items={priorityTypes}
                    value={selectedPriorityType}
                    onChange={onChangePriorityType}
                    fluid
                />
            </Flex>
            <Flex.Item push>
                <Flex>
                    <Flex.Item push>
                        <Button
                            className="athena-button"
                            content={priorityIdToEdit.current ? localize("updateBtnText") : localize("submitButtonContent")}
                            disabled={isSubmitting}
                            loading={isSubmitting}
                            onClick={onSubmit} />
                    </Flex.Item>
                </Flex>
            </Flex.Item>
        </Flex>
    );
}

export default withRouter(NewPriority);