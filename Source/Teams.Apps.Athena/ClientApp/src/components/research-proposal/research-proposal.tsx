// <copyright file="research-proposal.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Input, Text, Button, TextArea, FormLabel, FormField, FormDropdown } from '@fluentui/react-northstar';
import { DatePicker } from 'office-ui-fabric-react/lib/DatePicker';
import { useTranslation } from 'react-i18next';
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import * as microsoftTeams from "@microsoft/teams-js";
import { StatusCodes } from "http-status-codes";
import { withRouter, RouteComponentProps } from "react-router-dom";
import IResearchProposal from "../../models/research-proposal";
import IKeyword from "../../models/keyword";
import { getAllKeywordsAsync } from "../../api/keyword-api";
import KeywordSearchDropdown from "../common/keyword-search-dropdown/keyword-search-dropdown";
import moment from "moment";
import { getSecurityLevelsAsync } from "../../api/security-levels-api";
import { createResearchProposalAsync } from "../../api/research-proposal-api";
import ISecurityLevel from "../../models/security-level";
import IPriorityType from "../../models/priority-type";
import { getPriorityTypesAsync } from "../../api/priority-api";

import "./research-proposal.scss";

interface IResearchProposalProps extends RouteComponentProps {
}

const ResearchProposal: React.FunctionComponent<IResearchProposalProps> = (props: IResearchProposalProps) => {
    let minDate = new Date();
    minDate.setDate(minDate.getDate() + 1);

    const localize = useTranslation().t;
    const [title, setTitle] = React.useState<string>("");
    const [description, setDescription] = React.useState<string>("");
    const [titleError, setTitleError] = React.useState<string>("");
    const [descriptionError, setDescriptionError] = React.useState<string>("");
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>(false);
    const [selectedKeywords, setSelectedKeywords] = React.useState<IKeyword[]>([]);
    const [KeywordsError, setKeywordsError] = React.useState<string>("");
    const [startDate, setStartDate] = React.useState<Date>(minDate);
    const [details, setDetails] = React.useState<string>("");
    const [detailsError, setDetailsError] = React.useState<string>("");
    const [securityLevelData, setSecurityLevelData] = React.useState<ISecurityLevel[]>([]);
    const [selectedSecurityLevelId, setSelectedSecurityLevelId] = React.useState<number>(0);
    const [selectedSecurityLevelDescription, setSelectedSecurityLevelDescription] = React.useState<string>("");
    const [securityLevelError, setSecurityLevelError] = React.useState<string>("");
    const [budget, setBudget] = React.useState<string>("");
    const [budgetError, setBudgetError] = React.useState<string>("");
    const [potentialFunding, setPotentialFunding] = React.useState<string>("");
    const [potentialFundingError, setPotentialFundingError] = React.useState<string>("");
    const [selectedPriorityType, setSelectedPriorityType] = React.useState<any>(undefined);
    const [priorityError, setPriorityError] = React.useState<string>("");
    const [topicType, setTopicType] = React.useState<string>("");
    const [topicTypeError, setTopicTypeError] = React.useState<string>("");
    const [focusQuestion1, setFocusQuestion1] = React.useState<string>("");
    const [focusQuestion1Error, setFocusQuestion1Error] = React.useState<string>("");
    const [focusQuestion2, setFocusQuestion2] = React.useState<string>("");
    const [focusQuestion2Error, setFocusQuestion2Error] = React.useState<string>("");
    const [focusQuestion3, setFocusQuestion3] = React.useState<string>("");
    const [focusQuestion3Error, setFocusQuestion3Error] = React.useState<string>("");
    const [objectives, setObjectives] = React.useState<string>("");
    const [objectivesError, setObjectivesError] = React.useState<string>("");
    const [plan, setPlan] = React.useState<string>("");
    const [planError, setPlanError] = React.useState<string>("");
    const [deliverables, setDeliverables] = React.useState<string>("");
    const [deliverablesError, setDeliverablesError] = React.useState<string>("");
    const [completionTime, setCompletionTime] = React.useState<string>("");
    const [completionTimeError, setCompletionTimeError] = React.useState<string>("");
    const [endorsements, setEndorsements] = React.useState<string>("");
    const [endorsementsError, setEndorsementsError] = React.useState<string>("");
    const [keywords, setKeywords] = React.useState<IKeyword[]>([]);
    const [priorityTypes, setPriorityTypes] = React.useState<any[]>([]);

    React.useEffect(() => {
        fetchSecurityLevels();
        fetchAllKeywords();
        getPriorityTypes();
    }, []);

    // Fetches priority types.
    const getPriorityTypes = async () => {
        let response = await getPriorityTypesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let types = response.data
                .map((type: IPriorityType, index: number) => { return { value: type.id, header: type.title } });

            setPriorityTypes(types);
        }
        else {
            setStatus({ id: status.id + 1, message: localize("failedToLoadPriorityTypesError"), type: ActivityStatus.Error });
        }
    }

    // Fetch security levels.
    const fetchSecurityLevels = async () => {
        let response = await getSecurityLevelsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let data = response.data as ISecurityLevel[];
            setSecurityLevelData(data);
        }
        else {
            setStatus({ id: status.id + 1, message: localize("failedToLoadSecurityLevelsError"), type: ActivityStatus.Error });
        }
    }

    // Fetch all keywords.
    const fetchAllKeywords = async () => {
        let response = await getAllKeywordsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywords = response.data as IKeyword[];
            setKeywords(keywords);
        }
    }

    /**
     * Sets the research proposal's title.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onTitleChange = (event: any, inputProps: any) => {
        setTitle(inputProps.value);
        if (inputProps.value) {
            setTitleError("");
        }
    }

    /**
     * Sets the research proposal's budget.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onBudgetChange = (event: any, inputProps: any) => {
        setBudget(inputProps.value);
        if (inputProps.value) {
            setBudgetError("");
        }
    }

    /**
     * Sets the research proposal's potential funding.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onPotentialFundingChange = (event: any, inputProps: any) => {
        setPotentialFunding(inputProps.value);
        if (inputProps.value) {
            setPotentialFundingError("");
        }
    }

    /**
     * Sets the research proposal's description.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onDescriptionChange = (event: any, inputProps: any) => {
        setDescription(inputProps.value);
        if (inputProps.value) {
            setDescriptionError("");
        }
    }

    /**
     * Sets the research proposal's details.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onDetailsChange = (event: any, inputProps: any) => {
        setDetails(inputProps.value);
        if (inputProps.value) {
            setDetailsError("");
        }
    }

    /**
     * Event handler called when priority type gets changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onChangePriorityType = (eventDetails: any, eventData: any) => {
        setSelectedPriorityType(eventData.value);
    }

    /**
     * Sets the research proposal's topic type.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onTopicTypeChange = (event: any, inputProps: any) => {
        setTopicType(inputProps.value);
        if (inputProps.value) {
            setTopicTypeError("");
        }
    }

    /**
     * Sets the research proposal's focus question 1.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onFocusQuestion1Change = (event: any, inputProps: any) => {
        setFocusQuestion1(inputProps.value);
        if (inputProps.value) {
            setFocusQuestion1Error("");
        }
    }

    /**
     * Sets the research proposal's focus question 2.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onFocusQuestion2Change = (event: any, inputProps: any) => {
        setFocusQuestion2(inputProps.value);
        if (inputProps.value) {
            setFocusQuestion2Error("");
        }
    }

    /**
     * Sets the research proposal's focus question 3.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onFocusQuestion3Change = (event: any, inputProps: any) => {
        setFocusQuestion3(inputProps.value);
        if (inputProps.value) {
            setFocusQuestion3Error("");
        }
    }

    /**
     * Sets the research proposal's objectives.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onObjectivesChange = (event: any, inputProps: any) => {
        setObjectives(inputProps.value);
        if (inputProps.value) {
            setObjectivesError("");
        }
    }

    /**
     * Sets the research proposal's plan.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onPlanChange = (event: any, inputProps: any) => {
        setPlan(inputProps.value);
        if (inputProps.value) {
            setPlanError("");
        }
    }

    /**
     * Sets the research proposal's deliverables.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onDeliverablesChange = (event: any, inputProps: any) => {
        setDeliverables(inputProps.value);
        if (inputProps.value) {
            setDeliverablesError("");
        }
    }

    /**
     * Sets the research proposal's completion time.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onCompletionTimeChange = (event: any, inputProps: any) => {
        setCompletionTime(inputProps.value);
        if (inputProps.value) {
            setCompletionTimeError("");
        }
    }

    /**
     * Sets the research proposal's endorsements.
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onEndorsementsChange = (event: any, inputProps: any) => {
        setEndorsements(inputProps.value);
        if (inputProps.value) {
            setEndorsementsError("");
        }
    }

    // Event handler called when selected keywords gets changed.
    const getSlectedKeywords = React.useCallback((selectedKeywords: IKeyword[]) => {
        setSelectedKeywords(selectedKeywords);
        setKeywordsError("");
    }, [selectedKeywords]);

    // Handles create button click.
    const handleCreateBtnClick = async () => {
        let inputsValidated = validateInputs();
        if (inputsValidated) {
            setIsLoading(true);
            var researchProject: IResearchProposal = {
                title: title,
                description: description,
                details: details,
                startDate: startDate,
                budget: budget,
                potentialFunding: potentialFunding,
                securityLevel: selectedSecurityLevelId,
                keywordsJson: selectedKeywords,
                priority: selectedPriorityType?.value,
                topicType: topicType,
                focusQuestion1: focusQuestion1,
                focusQuestion2: focusQuestion2,
                focusQuestion3: focusQuestion3,
                objectives: objectives,
                plan: plan,
                deliverables: deliverables,
                completionTime: completionTime,
                endorsements: endorsements
            }
            var response = await createResearchProposalAsync(researchProject, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.CREATED) {
                setStatus({ id: status.id + 1, message: localize("submitSuccessMessage"), type: ActivityStatus.Success });
                microsoftTeams.tasks.submitTask();
            }
            else if (response && response.status === StatusCodes.CONFLICT) {
                setStatus({ id: status.id + 1, message: localize("researchProjectWithTitleExists"), type: ActivityStatus.Error });
            }
            else {
                setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            }
            setIsLoading(false);
        }
        else {
            return;
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Validates all the values in the input fields.
    const validateInputs = () => {
        let isValid = true;
        if (!title) {
            setTitleError(localize("enterTitleErrorMsg"));
            isValid = false;
        }
        if (!description) {
            setDescriptionError(localize("descriptionErrorMsg"));
            isValid = false;
        }
        if (!details) {
            setDetailsError(localize("detailsErrorMsg"));
            isValid = false;
        }
        if (!budget) {
            setBudgetError(localize("budgetErrorMsg"));
            isValid = false;
        }
        if (!potentialFunding) {
            setPotentialFundingError(localize("potentialFundingErrorMsg"));
            isValid = false;
        }
        if (!selectedSecurityLevelId && !selectedSecurityLevelDescription) {
            setSecurityLevelError(localize("securityLevelErrorMsg"));
            isValid = false;
        }
        if (!selectedKeywords?.length) {
            setKeywordsError(localize("createNewRequestKeywordsError"));
            isValid = false;
        }
        if (!selectedPriorityType) {
            setPriorityError(localize("priorityTypeErrorMsg"));
            isValid = false;
        }
        if (!topicType) {
            setTopicTypeError(localize("topicTypeErrorMsg"));
            isValid = false;
        }
        if (!focusQuestion1) {
            setFocusQuestion1Error(localize("focusQuestion1ErrorMsg"));
            isValid = false;
        }
        if (!focusQuestion2) {
            setFocusQuestion2Error(localize("focusQuestion2ErrorMsg"));
            isValid = false;
        }
        if (!focusQuestion3) {
            setFocusQuestion3Error(localize("focusQuestion3ErrorMsg"));
            isValid = false;
        }
        if (!objectives) {
            setObjectivesError(localize("objectivesErrorMsg"));
            isValid = false;
        }
        if (!plan) {
            setPlanError(localize("planErrorMsg"));
            isValid = false;
        }
        if (!deliverables) {
            setDeliverablesError(localize("deliverablesErrorMsg"));
            isValid = false;
        }
        if (!completionTime) {
            setCompletionTimeError(localize("completionTimeErrorMsg"));
            isValid = false;
        }
        if (!endorsements) {
            setEndorsementsError(localize("endorsementsErrorMsg"));
            isValid = false;
        }
        return isValid;
    }

    /**
    * Event handler on selecting start date. 
    * @param date The date.
    */
    const onStartDateChange = (date: Date | null | undefined) => {
        let startDate = moment(date)
            .set('hour', moment().hour())
            .set('minute', moment().minute())
            .set('second', moment().second());
        setStartDate(startDate.toDate());
    }

    /**
     * Event handler called when security level selection get changed.
     * @param eventData The event data.
     */
    const onSecurityLevelChange = (event: any, dropdownProps?: any) => {
        if (dropdownProps.value) {
            setSelectedSecurityLevelDescription(dropdownProps.value!.header);
            setSelectedSecurityLevelId(dropdownProps.value!.key);
            setSecurityLevelError("");
        }
    }

    // Renders the keywords search dropdown.
    const renderKeywordsSearchDropdown = React.useMemo(() => {
        return <KeywordSearchDropdown
            keywords={keywords}
            showSlectedKywordPills={true}
            label={`${localize('keywordsText')}*`}
            getSelectedKywords={getSlectedKeywords}
            selectedKeywords={selectedKeywords}
        />
    }, [selectedKeywords, keywords]);

    return (
        <Flex fill column gap="gap.medium" className="task-module-container">
            <Flex fill column gap="gap.medium" className="form-fields">
                <StatusBar status={status} isMobile={false} />
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('titleText')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("titlePlaceholderTxt")} value={title} onChange={onTitleChange} />
                    </FormField>
                    {
                        titleError &&
                        <Text content={titleError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('descriptionText')}*`} />
                    </FormLabel>
                    <FormField>
                        <TextArea placeholder={localize("typeHerePlaceholderTxt")} value={description} onChange={onDescriptionChange} className="desc-text-area" />
                    </FormField>
                    {
                        descriptionError &&
                        <Text content={descriptionError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('detailsTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <TextArea placeholder={localize("typeHerePlaceholderTxt")} value={details} onChange={onDetailsChange} className="desc-text-area" />
                    </FormField>
                    {
                        detailsError &&
                        <Text content={detailsError} error />
                    }
                </Flex>
                <Flex vAlign="center" gap="gap.small">
                    <Flex.Item size="size.half">
                        <FormDropdown
                            label={`${localize('securityLevelTitle')}*`}
                            items={securityLevelData.map((value: ISecurityLevel) => { return { key: value.securityId, header: value.description } })}
                            placeholder={localize("securityLevelPlaceholder")}
                            loadingMessage={localize("loadingLabel")}
                            onChange={onSecurityLevelChange}
                            value={selectedSecurityLevelDescription}
                            className="form-dropdown"
                            fluid
                            errorMessage={securityLevelError}
                        />
                    </Flex.Item>
                    <Flex.Item size="size.half">
                        <Flex column>
                            <FormLabel>
                                <Text content={`${localize('startDateTitle')}*`} />
                            </FormLabel>
                            <FormField>
                                <DatePicker
                                    className="date-picker-style"
                                    showMonthPickerAsOverlay={true}
                                    minDate={minDate}
                                    isMonthPickerVisible={true}
                                    value={startDate}
                                    onSelectDate={onStartDateChange}
                                />
                            </FormField>
                        </Flex>
                    </Flex.Item>
                </Flex>
                <Flex.Item grow>
                    <FormField
                        className="input-field-lable"
                        control={renderKeywordsSearchDropdown}
                        errorMessage={KeywordsError}
                    />
                </Flex.Item>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('budgetTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("budgetPlaceholder")} value={budget} onChange={onBudgetChange} />
                    </FormField>
                    {
                        budgetError &&
                        <Text content={budgetError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('potentialFundingTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("potentialFundingPlaceholder")} value={potentialFunding} onChange={onPotentialFundingChange} />
                    </FormField>
                    {
                        potentialFundingError &&
                        <Text content={potentialFundingError} error />
                    }
                </Flex>
                <FormDropdown
                    label={`${localize("priorityTypeLabel")}*`}
                    placeholder={localize("insightsConfigSelectPriorityTypeDropdownPlaceholder")}
                    items={priorityTypes}
                    value={selectedPriorityType}
                    onChange={onChangePriorityType}
                    fluid
                    errorMessage={priorityError}
                />
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('topicTypeTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("topicTypePlaceholder")} value={topicType} onChange={onTopicTypeChange} />
                    </FormField>
                    {
                        topicTypeError &&
                        <Text content={topicTypeError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('focusQuestion1Title')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("focusQuestion1Placeholder")} value={focusQuestion1} onChange={onFocusQuestion1Change} />
                    </FormField>
                    {
                        focusQuestion1Error &&
                        <Text content={focusQuestion1Error} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('focusQuestion2Title')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("focusQuestion2Placeholder")} value={focusQuestion2} onChange={onFocusQuestion2Change} />
                    </FormField>
                    {
                        focusQuestion2Error &&
                        <Text content={focusQuestion2Error} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('focusQuestion3Title')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("focusQuestion3Placeholder")} value={focusQuestion3} onChange={onFocusQuestion3Change} />
                    </FormField>
                    {
                        focusQuestion3Error &&
                        <Text content={focusQuestion3Error} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('objectivesTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("objectivesPlaceholder")} value={objectives} onChange={onObjectivesChange} />
                    </FormField>
                    {
                        objectivesError &&
                        <Text content={objectivesError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('planTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("planPlaceholder")} value={plan} onChange={onPlanChange} />
                    </FormField>
                    {
                        planError &&
                        <Text content={planError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('deliverablesTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("deliverablesPlaceholder")} value={deliverables} onChange={onDeliverablesChange} />
                    </FormField>
                    {
                        deliverablesError &&
                        <Text content={deliverablesError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('completionTimeTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("completionTimePlaceholder")} value={completionTime} onChange={onCompletionTimeChange} />
                    </FormField>
                    {
                        completionTimeError &&
                        <Text content={completionTimeError} error />
                    }
                </Flex>
                <Flex column>
                    <FormLabel>
                        <Text content={`${localize('endorsementsTitle')}*`} />
                    </FormLabel>
                    <FormField>
                        <Input fluid placeholder={localize("endorsementsPlaceholder")} value={endorsements} onChange={onEndorsementsChange} />
                    </FormField>
                    {
                        endorsementsError &&
                        <Text content={endorsementsError} error />
                    }
                </Flex>
            </Flex>
            <Flex>
                <Flex.Item push>
                    <Button className="athena-button" content={localize("createButtonText")} onClick={handleCreateBtnClick} loading={isLoading} disabled={isLoading} />
                </Flex.Item>
            </Flex>
        </Flex>
    )
}

export default withRouter(ResearchProposal);