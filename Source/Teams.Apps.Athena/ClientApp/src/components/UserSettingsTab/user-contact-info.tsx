// <copyright file="user-contact-info.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import IUserSettings from '../../models/user-settings';
import { Input, Flex, FormDropdown, Button, FormField, Divider, Text, Loader} from '@fluentui/react-northstar';
import { getGraduationDegreesAsync, getOrganizationsAsync, getSpecialtyAsync, getStudyDepartmentsAsync, getJobTitlesAsync } from "../../api/user-settings-tab-api";
import IGraduationDegree from '../../models/graduation-degree';
import IStudyDepartment from '../../models/study-department';
import IJobTitle from '../../models/job-title';
import ISpecialty from '../../models/specialty';
import IOrganization from '../../models/organization';
import IDropdownItem from '../../models/dropdown-item';
import { IsValidUrl } from '../../helpers/url-helper';
import IKeyword from '../../models/keyword';

import "./user-contact-info.scss";

interface IUserSettingsPage1State {
    userDetails: IUserSettings,
    isFirstName: boolean,
    isMiddleName: boolean,
    isLastName: boolean,
    isjobTitle: boolean,
    isCurrentOrganization: boolean,
    isEmailAddress: boolean,
    isSecondaryEmailAddress: boolean,
    isOtherContact: boolean,
    isSecondaryOtherContact: boolean,
    isOrganization: boolean,
    iskeywords: boolean,
    isgraduateDegreeProgram: boolean,
    isNPSDegreeProgram: boolean,
    isdeptofStudy: boolean,
    isSpecialty: boolean,
    isUnderGraduateDegree: boolean,
    isProfessionalCertificates: boolean,
    isProfessionalOrganizations: boolean,
    isProfessionalExperience: boolean,
    isProfessionalPublications: boolean,
    isProfilePictureImageURL: boolean,
    isResumeCVLink: boolean,
    isNextPage: boolean,
    loader: boolean,
    searchText: string,
    selectedKeywords: IDropdownItem[],
    isFirstTimeLogin: boolean,
    graduationDegreeList: string[],
    organizationList: string[],
    specialtyList: string[],
    jobTitleList: string[],
    departmentOfStudyList: string[]
}

interface IUserSettingsPage1Props extends WithTranslation {
    userDetails: IUserSettings,
    onBackClick: (userDetails: IUserSettings) => void,
    onNextClick: (userDetails: IUserSettings) => void
}

class UserContactInfo extends React.Component<IUserSettingsPage1Props, IUserSettingsPage1State> {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            userDetails: this.props.userDetails,
            graduationDegreeList: [] as string[],
            isFirstName: true,
            isMiddleName: true,
            isLastName: true,
            isjobTitle: true,
            isCurrentOrganization: true,
            isEmailAddress: true,
            isSecondaryEmailAddress: true,
            isOtherContact: true,
            isSecondaryOtherContact: true,
            isOrganization: true,
            iskeywords: true,
            isgraduateDegreeProgram: true,
            isNPSDegreeProgram: true,
            isdeptofStudy: true,
            isSpecialty: true,
            isUnderGraduateDegree: true,
            isProfessionalCertificates: true,
            isProfessionalOrganizations: true,
            isProfessionalExperience: true,
            isProfessionalPublications: true,
            isProfilePictureImageURL: true,
            isResumeCVLink: true,
            isNextPage: false,
            loader: false,
            searchText: "",
            selectedKeywords: [],
            isFirstTimeLogin: false,
            organizationList: [],
            specialtyList: [],
            jobTitleList: [],
            departmentOfStudyList: []
        }
    }

    componentDidMount() {
        this.getGraduationDegreeList();
        this.getOrganizationsList();
        this.getSpecialtyList();
        this.getDepartOfStudyList();
        this.getJobTitleList();
    }

    getGraduationDegreeList = async () => {
        let response = await getGraduationDegreesAsync();
        if (response && response.status) {
            let result = response.data.map((item: IGraduationDegree) => { return item.graduationDegreeTitle });
            this.setState({
                graduationDegreeList: result
            });
        }
    }

    getOrganizationsList = async () => {
        let response = await getOrganizationsAsync();
        if (response && response.status) {
            let result = response.data.map((item: IOrganization) => { return item.organizationTitle });
            this.setState({
                organizationList: result
            });
        }
    }

    getSpecialtyList = async () => {
        let response = await getSpecialtyAsync();
        if (response && response.status) {
            let result = response.data.map((item: ISpecialty) => { return item.specialtyTitle });
            this.setState({
                specialtyList: result
            });
        }
    }

    getDepartOfStudyList = async () => {
        let response = await getStudyDepartmentsAsync();
        if (response && response.status) {
            let result = response.data.map((item: IStudyDepartment) => { return item.studyDepartmentTitle });
            this.setState({
                departmentOfStudyList: result
            });
        }
    }

    // Get the job title list.
    getJobTitleList = async () => {
        let response = await getJobTitlesAsync(this.handleTokenAccessFailure);
        if (response && response.status) {
            let result = response.data.map((item: IJobTitle) => { return item.title});
            this.setState({
                jobTitleList: result
            });
        }
    }

    public UNSAFE_componentWillReceiveProps(nextProps: IUserSettingsPage1Props, nextState: IUserSettingsPage1State) {
        if (nextProps !== this.props && nextProps.userDetails !== this.props.userDetails) {
            this.setState({ userDetails: nextProps.userDetails });
        }
    }

    // Set state for back click.
    onBackClick = () => {
        this.setState({ isNextPage: false })
    }

    // Set state for user's first name.
    onFirstNameChange = (event: any) => {
        let firstName = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, firstName },
            isFirstName: true
        }));
    }

    // Validate and return user's first name validation error.
    getFirstNameValidationError = () => {
        let errorMessage = "";
        let regex = /^[A-Za-z]+$/;

        if (!this.state.isFirstName && !this.state.userDetails.firstName) {
            errorMessage = this.localize("requiredfirstNameText");
        }
        if (this.state.userDetails.firstName && !regex.test(this.state.userDetails.firstName)) {
            errorMessage = this.localize("requiredStringFirstNameText");
        }
        return errorMessage;
    }

    // Set state for user's middle name.
    onMiddleNameChange = (event: any) => {
        let middleName = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, middleName },
            isMiddleName: true
        }));
    }

    // Validate and return user's middle name validation error.
    getMiddleNameValidationError = () => {
        let errorMessage = "";
        let regex = /^[A-Za-z]+$/;

        if (!this.state.isMiddleName && !this.state.userDetails.middleName) {
            errorMessage = this.localize("requiredMiddleNameText");
        }
        if (this.state.userDetails.middleName && !regex.test(this.state.userDetails.middleName)) {
            errorMessage = this.localize("requiredStringMiddleNameText");
        }
        return errorMessage;
    }

    // Set state for user's last name.
    onLastNameChange = (event: any) => {
        let lastName = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, lastName },
            isLastName: true
        }));
    }

    // Validate and return user's last name validation error.
    getLastNameValidationError = () => {
        let errorMessage = "";
        let regex = /^[A-Za-z]+$/;

        if (!this.state.isLastName && !this.state.userDetails.lastName) {
            errorMessage = this.localize("requiredlastNameText");
        }
        if (this.state.userDetails.lastName && !regex.test(this.state.userDetails.lastName)) {
            errorMessage = this.localize("requiredStringLastNameText");
        }
        return errorMessage;
    }

    // Set state for job title.
    onJobTitleChange = (eventData: any, data: any) => {
        let jobTitle = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, jobTitle },
            isjobTitle: true
        }));
    }

    // Validate and return job_Title validation error.
    getJobTitleValidationError = () => {
        let errorMessage = "";
        if (!this.state.isjobTitle && !this.state.userDetails.jobTitle?.length) {
            errorMessage = this.localize("requiredJobTitleText");
        }
        return errorMessage;
    }

    // Set state for phone.
    onPhoneNumberChange = (event: any) => {
        let otherContact = event.target.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, otherContact },
            isOtherContact: true
        }));
    }

    // Validate and return phone validation error.
    getPhoneNumberValidationError = () => {
        let errorMessage = "";
        const reg = /^(\d+-?)+\d+$/;
        if (!this.state.isOtherContact && !this.state.userDetails.otherContact) {
            errorMessage = this.localize("requiredPhoneNumberText");
        }
        if (this.state.userDetails.otherContact && !reg.test(this.state.userDetails.otherContact!.toString())) {
            errorMessage = this.localize("requiredNumberText");
        }
        return errorMessage;
    }
 
    /**
     * Set state for secondary phone number.
     * @param event The event.
     */
    onSecondaryPhoneNumberChange = (event: any) => {
        let secondaryOtherContact = event.target.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, secondaryOtherContact },
            isSecondaryOtherContact: true
        }));
    }

    // Set state for email.
    onEmailIdChange = (event: any) => {
        let emailAddress = event.target.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, emailAddress },
            isEmailAddress: true
        }));

    }

    // Validate and return email validation error.
    getEmailValidationError = () => {
        let errorMessage = "";
        const reg = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w\w+)+$/;
        if (!this.state.isEmailAddress && !this.state.userDetails.emailAddress) {
            errorMessage = this.localize("requiredEmailIdText");
        }
        else if (this.state.userDetails.emailAddress && !reg.test(this.state.userDetails.emailAddress!)) {
            errorMessage = this.localize("requiredValidEmailFormatText");
        }
        return errorMessage;
    }

    /**
     * Set state for secondary email.
     * @param event The event.
     */
    onSecondaryEmailIdChange = (event: any) => {
        let secondaryEmailAddress = event.target.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, secondaryEmailAddress },
            isSecondaryEmailAddress: true
        }));

    }

    // Set state for organization.
    onOrganizationChange = (eventData: any, data: any) => {
        let organization = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, organization },
            isOrganization: true
        }));
    }

    // Validate and return organization validation error.
    getOraganizationValidationError = () => {
        let errorMessage = "";
        if (!this.state.isOrganization && !this.state.userDetails.organization) {
            errorMessage = this.localize("requiredOrganizationText");
        }
        return errorMessage;
    }

    // Set state for specialty.
    onSpecialtyChange = (eventData: any, data: any) => {
        let specialty = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, specialty },
            isSpecialty: true
        }));
    }

    // Validate and return specialty validation error.
    getSpecialtyValidationError = () => {
        let errorMessage = "";
        if (!this.state.isSpecialty && !this.state.userDetails.specialty) {
            errorMessage = this.localize("requiredspecialtyText");
        }
        return errorMessage;
    }

    /**
     * Event handler called when keyword selection get changed.
     * @param eventData The event data.
     */
    onChangeKeywords = (eventData: any) => {
        this.setState({
            selectedKeywords: eventData ?? []
        });

        let keywords = eventData
            .map(keywordDetails => {
                return { keywordId: keywordDetails.value, title: keywordDetails.header } as IKeyword
            });

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, keywordsJson: keywords }
        }));
    }

    // Validate and return keywords validation error.
    getkeywordsValidationError = () => {
        let errorMessage = "";
        if (!this.state.iskeywords && !this.state.userDetails.keywordsJson) {
            errorMessage = this.localize("requiredkeywordsText");
        }
        return errorMessage;
    }

    // Set state for graduate degree program.
    onGraduateDegreeProgramChange = (eventData: any, data: any) => {
        let graduateDegreeProgram = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, graduateDegreeProgram },
            isgraduateDegreeProgram: true
        }));
    }

    // Validate and return post grad validation error.
    getGraduateDegreeProgramValidationError = () => {
        let errorMessage = "";
        if (!this.state.isgraduateDegreeProgram && !this.state.userDetails.graduateDegreeProgram) {
            errorMessage = this.localize("requireGraduateDegreeProgramText");
        }
        return errorMessage;
    }

    /**
     * Set state for nps degree program.
     * @param eventData The event data.
     * @param data The data.
     */
    onNPSDegreeProgramChange = (eventData: any, data: any) => {
        let npsDegreeProgram = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, npsDegreeProgram },
            isNPSDegreeProgram: true
        }));
    }

    // Validate and return nps degree program validation error.
    getNPSDegreeProgramValidationError = () => {
        let errorMessage = "";
        if (!this.state.isNPSDegreeProgram && !this.state.userDetails.npsDegreeProgram) {
            errorMessage = this.localize("requireNPSDegreeProgramText");
        }
        return errorMessage;
    }

    // Set state for department of study.
    onDeptOfStudyChange = (eventData: any, data: any) => {
        let deptOfStudy = data.value;
        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, deptOfStudy },
            isdeptofStudy: true
        }));
    }

    // Validate and return department of study validation error.
    getDeptOfStudyValidationError = () => {
        let errorMessage = "";
        if (!this.state.isdeptofStudy && !this.state.userDetails.deptOfStudy) {
            errorMessage = this.localize("requiredDeptofStudyText");
        }
        return errorMessage;
    }

    // Set state for current organization name.
    onCurrentOrganizationChange = (event: any) => {
        let currentOrganization = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, currentOrganization },
            isCurrentOrganization: true
        }));
    }

    // Validate and return current organization validation error.
    getCurrentOrganizationValidationError = () => {
        let errorMessage = "";

        if (!this.state.isCurrentOrganization && !this.state.userDetails.currentOrganization) {
            errorMessage = this.localize("requiredCurrentOrganizationText");
        }
        return errorMessage;
    }

    // Set state for undergraduate degree.
    onUnderGraduateDegreeChange = (event: any) => {
        let underGraduateDegree = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, underGraduateDegree },
            isUnderGraduateDegree: true
        }));
    }

    // Validate and return undergraduate degree validation error.
    getUnderGraduateDegreeValidationError = () => {
        let errorMessage = "";
        if (!this.state.isUnderGraduateDegree && !this.state.userDetails.underGraduateDegree) {
            errorMessage = this.localize("requireUnderGraduateDegreeText");
        }
        return errorMessage;
    }

    // Set state for professional certificates.
    onProfessionalCertificatesChange = (event: any) => {
        let professionalCertificates = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, professionalCertificates },
            isProfessionalCertificates: true
        }));
    }

    // Validate and return professional certificates validation error.
    getProfessionalCertificatesValidationError = () => {
        let errorMessage = "";
        if (!this.state.isProfessionalCertificates && !this.state.userDetails.professionalCertificates) {
            errorMessage = this.localize("requiredProfessionalCertificatesText");
        }
        return errorMessage;
    }

    // Set state for professional organizations.
    onProfessionalOrganizationsChange = (event: any) => {
        let professionalOrganizations = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, professionalOrganizations },
            isProfessionalOrganizations: true
        }));
    }

    // Validate and return professional organizations validation error.
    getProfessionalOrganizationsValidationError = () => {
        let errorMessage = "";
        if (!this.state.isProfessionalOrganizations && !this.state.userDetails.professionalOrganizations) {
            errorMessage = this.localize("requiredProfessionalOrganizationsText");
        }
        return errorMessage;
    }

    // Set state for professional experience.
    onProfessionalExperienceChange = (event: any) => {
        let professionalExperience = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, professionalExperience },
            isProfessionalExperience: true
        }));
    }

    // Validate and return professional experience validation error.
    getProfessionalExperienceValidationError = () => {
        let errorMessage = "";
        if (!this.state.isProfessionalExperience && !this.state.userDetails.professionalExperience) {
            errorMessage = this.localize("requiredProfessionalExperienceText");
        }
        return errorMessage;
    }

    // Set state for professional publications.
    onProfessionalPublicationsChange = (event: any) => {
        let professionalPublications = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, professionalPublications },
            isProfessionalPublications: true
        }));
    }

    // Validate and return professional publications validation error.
    getProfessionalPublicationsValidationError = () => {
        let errorMessage = "";
        if (!this.state.isProfessionalPublications && !this.state.userDetails.professionalPublications) {
            errorMessage = this.localize("requiredProfessionalPublicationsText");
        }
        return errorMessage;
    }

    // Set state for image URL.
    onImageURLChange = (event: any) => {
        let profilePictureImageURL = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, profilePictureImageURL },
            isProfilePictureImageURL: true
        }));
    }

    // Validate and return image URL validation error.
    getImageURLValidationError = () => {
        let errorMessage = "";
        var isValidUrl = IsValidUrl(this.state.userDetails?.profilePictureImageURL!);
        if (!this.state.isProfilePictureImageURL && !this.state.userDetails.profilePictureImageURL) {
            errorMessage = this.localize("requiredImageURL");
        }
        if (this.state.userDetails.profilePictureImageURL && !isValidUrl) {
            errorMessage = this.localize("requiredStringImageURLText");
        }

        return errorMessage;
    }

    // Set state for resume link.
    onResumeLinkChange = (event: any) => {
        let resumeCVLink = event.target.value;

        this.setState((prevState: IUserSettingsPage1State) => ({
            userDetails: { ...prevState.userDetails, resumeCVLink },
            isResumeCVLink: true
        }));
    }

    // Validate and return resume link validation error.
    getResumeLinkValidationError = () => {
        let errorMessage = "";
        var isValidUrl = IsValidUrl(this.state.userDetails?.resumeCVLink!);
        if (!this.state.isResumeCVLink && !this.state.userDetails.resumeCVLink) {
            errorMessage = this.localize("requiredResumeLink");
        }
        if (this.state.userDetails.resumeCVLink && !isValidUrl) {
            errorMessage = this.localize("requiredStringResumeURLText");
        }
        return errorMessage;
    }

    // Invoke when user clicks 'Next' button to add users in project.
    onNextButtonClick = (e: any) => {
        let isNext = this.checkIfNextAllowed();
        if (isNext === true) {
            this.props.onNextClick(this.state.userDetails);
            this.setState({ isNextPage: true });
        }
    }

    /**
     * Redirects to the sign-in page when accessing token is failed in API.
     * @param error The error message.
     */
    handleTokenAccessFailure = (error: string) => {
        //this.props.history.push("/signin");
    }

    // Function for applying validation on the fields before moving onto next step.
    checkIfNextAllowed = () => {
        const emailRegex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w\w+)+$/;
        const numberRegex = /^(\d+-?)+\d+$/;
        const regex = /^[A-Za-z]+$/;

        let validationStatus = {
            isFirstNameValid: true,
            isMiddleNameValid: true,
            isLastNameValid: true,
            isKeywordsValid: true,
            isEmailAddressValid: true,
            isOtherContactValid: true,
            isOrganizationValid: true,
            isSpecialtyValid: true,
            isJobTitleValid: true,
            isCurrentOrganizationValid: true,
            isUnderGraduateDegreeValid: true,
            isGraduateDegreeProgramValid: true,
            isdeptofStudyValid: true,
            isProfessionalCertificatesValid: true,
            isProfessionalOrganizationsValid: true,
            isProfessionalExperienceValid: true,
            isProfessionalPublicationsValid: true,
            isProfilePictureImageURLValid: true,
            isResumeCVLinkValid: true,
            isNPSDegreeProgramValid: true,
        };

        // Validate all user inputs and show error message if required.
        if (!this.state.userDetails.firstName || !regex.test(this.state.userDetails.firstName)) {
            validationStatus.isFirstNameValid = false
        }
        else if (!this.state.userDetails.middleName || !regex.test(this.state.userDetails.middleName)) {
            validationStatus.isMiddleNameValid = false
        }
        else if (!this.state.userDetails.lastName || !regex.test(this.state.userDetails.lastName)) {
            validationStatus.isLastNameValid = false
        }
        else if (!this.state.userDetails.emailAddress || !emailRegex.test(this.state.userDetails.emailAddress!)) {
            validationStatus.isEmailAddressValid = false
        }
        else if (!this.state.userDetails.otherContact || !numberRegex.test(this.state.userDetails.otherContact!.toString())) {
            validationStatus.isOtherContactValid = false
        }
        else if (!this.state.userDetails.organization) {
            validationStatus.isOrganizationValid = false
        }
        else if (!this.state.userDetails.specialty) {
            validationStatus.isSpecialtyValid = false
        }
        else if (!this.state.userDetails.jobTitle?.length) {
            validationStatus.isJobTitleValid = false
        }
        else if (!this.state.userDetails.currentOrganization) {
            validationStatus.isCurrentOrganizationValid = false
        }
        else if (!this.state.userDetails.underGraduateDegree) {
            validationStatus.isUnderGraduateDegreeValid = false
        }
        else if (!this.state.userDetails.graduateDegreeProgram) {
            validationStatus.isGraduateDegreeProgramValid = false
        }
        else if (!this.state.userDetails.deptOfStudy) {
            validationStatus.isdeptofStudyValid = false
        }
        else if (!this.state.userDetails.professionalCertificates) {
            validationStatus.isProfessionalCertificatesValid = false
        }
        else if (!this.state.userDetails.professionalOrganizations) {
            validationStatus.isProfessionalOrganizationsValid = false
        }
        else if (!this.state.userDetails.professionalExperience) {
            validationStatus.isProfessionalExperienceValid = false
        }
        else if (!this.state.userDetails.professionalPublications) {
            validationStatus.isProfessionalPublicationsValid = false
        }
        else if (!this.state.userDetails.profilePictureImageURL || !IsValidUrl(this.state.userDetails.profilePictureImageURL)) {
            validationStatus.isProfilePictureImageURLValid = false
        }
        else if (!this.state.userDetails.resumeCVLink || !IsValidUrl(this.state.userDetails.resumeCVLink)) {
            validationStatus.isResumeCVLinkValid = false
        }
        else if (!this.state.userDetails.npsDegreeProgram) {
            validationStatus.isNPSDegreeProgramValid = false
        }

        this.setState({
            isFirstName: validationStatus.isFirstNameValid,
            isMiddleName: validationStatus.isMiddleNameValid,
            isLastName: validationStatus.isLastNameValid,
            iskeywords: validationStatus.isKeywordsValid,
            isEmailAddress: validationStatus.isEmailAddressValid,
            isOtherContact: validationStatus.isOtherContactValid,
            isOrganization: validationStatus.isOrganizationValid,
            isSpecialty: validationStatus.isSpecialtyValid,
            isjobTitle: validationStatus.isJobTitleValid,
            isCurrentOrganization: validationStatus.isCurrentOrganizationValid,
            isUnderGraduateDegree: validationStatus.isUnderGraduateDegreeValid,
            isgraduateDegreeProgram: validationStatus.isGraduateDegreeProgramValid,
            isdeptofStudy: validationStatus.isdeptofStudyValid,
            isProfessionalCertificates: validationStatus.isProfessionalCertificatesValid,
            isProfessionalOrganizations: validationStatus.isProfessionalOrganizationsValid,
            isProfessionalExperience: validationStatus.isProfessionalExperienceValid,
            isProfessionalPublications: validationStatus.isProfessionalPublicationsValid,
            isProfilePictureImageURL: validationStatus.isProfilePictureImageURLValid,
            isResumeCVLink: validationStatus.isResumeCVLinkValid,
            isNPSDegreeProgram: validationStatus.isNPSDegreeProgramValid
        });

        return validationStatus.isFirstNameValid &&
            validationStatus.isMiddleNameValid &&
            validationStatus.isLastNameValid &&
            validationStatus.isKeywordsValid &&
            validationStatus.isEmailAddressValid &&
            validationStatus.isOtherContactValid &&
            validationStatus.isOrganizationValid &&
            validationStatus.isSpecialtyValid &&
            validationStatus.isJobTitleValid &&
            validationStatus.isCurrentOrganizationValid &&
            validationStatus.isUnderGraduateDegreeValid &&
            validationStatus.isGraduateDegreeProgramValid &&
            validationStatus.isdeptofStudyValid &&
            validationStatus.isProfessionalCertificatesValid &&
            validationStatus.isProfessionalOrganizationsValid &&
            validationStatus.isProfessionalExperienceValid &&
            validationStatus.isProfessionalPublicationsValid &&
            validationStatus.isProfilePictureImageURLValid &&
            validationStatus.isNPSDegreeProgramValid &&
            validationStatus.isResumeCVLinkValid;
    }

    // Renders the component
    render() {
        if (this.state.loader) {
            return (
                <Flex hAlign="center" vAlign="center" column gap="gap.medium" className="task-module-container user-contact-info" fill>
                    <Loader />
                </Flex>
            );
        }
        else {
            return (
                <Flex column gap="gap.medium" className="task-module-container user-contact-info" fill>
                    <Text className="page-heading" content={this.localize("userContactInfoText")} />
                    <Divider size={2} color="brand" />
                    <Flex column gap="gap.medium" className="form-fields">
                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    label={this.localize("firstNameText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={25} name="firstName" id="firstName" fluid value={this.state.userDetails.firstName} onChange={this.onFirstNameChange} />}
                                    errorMessage={this.getFirstNameValidationError()}
                                />
                            </Flex.Item>

                            <Flex.Item grow>
                                <FormField
                                    label={this.localize("middleNameText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={25} name="middleName" id="middleName" fluid value={this.state.userDetails.middleName} onChange={this.onMiddleNameChange} />}
                                    errorMessage={this.getMiddleNameValidationError()}
                                />
                            </Flex.Item>

                            <Flex.Item grow>
                                <FormField
                                    label={this.localize("lastNameText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={25} name="lastName" id="lastName" fluid value={this.state.userDetails.lastName} onChange={this.onLastNameChange} />}
                                    errorMessage={this.getLastNameValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item size="size.half">
                                <FormField
                                    label={this.localize("emailText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={64} name="email" id="email" fluid value={this.state.userDetails.emailAddress} onChange={this.onEmailIdChange} />}
                                    errorMessage={this.getEmailValidationError()}
                                />
                            </Flex.Item>

                            <Flex.Item size="size.half">
                                <FormField
                                    label={this.localize("otherContactText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={64} name="phoneNumber" id="phoneNumber" fluid value={this.state.userDetails.otherContact} onChange={this.onPhoneNumberChange} />}
                                    errorMessage={this.getPhoneNumberValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item size="size.half">
                                <FormField
                                    label={this.localize("secondaryEmailText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={64} name="secondaryEmail" id="secondaryEmail" fluid value={this.state.userDetails.secondaryEmailAddress} onChange={this.onSecondaryEmailIdChange} />}
                                />
                            </Flex.Item>

                            <Flex.Item size="size.half">
                                <FormField
                                    label={this.localize("secondaryOtherContactText")}
                                    required
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={64} name="SecondaryPhoneNumber" id="SecondaryPhoneNumber" fluid value={this.state.userDetails.secondaryOtherContact} onChange={this.onSecondaryPhoneNumberChange} />}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormDropdown
                                    label={`${this.localize("organizationText")}*`}
                                    items={this.state.organizationList}
                                    placeholder={this.localize("newRequestOrganizationPlaceholder")}
                                    loadingMessage={this.localize("loadingLabel")}
                                    onChange={this.onOrganizationChange}
                                    defaultValue={this.state.userDetails.organization}
                                    value={this.state.userDetails.organization}
                                    errorMessage={this.getOraganizationValidationError()}
                                    className="form-dropdown"
                                    fluid />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormDropdown
                                    label={`${this.localize("specialtyText")}*`}
                                    items={this.state.specialtyList}
                                    placeholder={this.localize("newRequestSpecialtyPlaceholder")}
                                    loadingMessage={this.localize("loadingLabel")}
                                    onChange={this.onSpecialtyChange}
                                    defaultValue={this.state.userDetails.specialty}
                                    value={this.state.userDetails.specialty}
                                    errorMessage={this.getSpecialtyValidationError()}
                                    className="form-dropdown"
                                    fluid />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item size="size.half"> 
                                <FormField
                                    required
                                    label={this.localize("currentOrganizationText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="currentOrganization" id="currentOrganization" fluid value={this.state.userDetails.currentOrganization} onChange={this.onCurrentOrganizationChange} />}
                                    errorMessage={this.getCurrentOrganizationValidationError()}
                                />
                            </Flex.Item>

                            <Flex.Item size="size.half">
                                <FormField
                                    required
                                    label={this.localize("underGraduateDegreeText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="underGraduateDegree" id="underGraduateDegree" fluid value={this.state.userDetails.underGraduateDegree} onChange={this.onUnderGraduateDegreeChange} />}
                                    errorMessage={this.getUnderGraduateDegreeValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small" vAlign="center">
                            <Flex.Item size="size.half">
                                <FormField
                                    required
                                    label={this.localize("graduateDegreeProgramText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="graduateDegreeProgram" id="graduateDegreeProgram" fluid value={this.state.userDetails.graduateDegreeProgram} onChange={this.onGraduateDegreeProgramChange} />}
                                    errorMessage={this.getGraduateDegreeProgramValidationError()}
                                />
                            </Flex.Item>

                            <Flex.Item size="size.half">
                                <FormDropdown
                                    label={`${this.localize("npsDegreeProgramText")}*`}
                                    items={this.state.graduationDegreeList}
                                    placeholder={this.localize("newNPSDegreeProgramPlaceholder")}
                                    loadingMessage={this.localize("loadingLabel")}
                                    onChange={this.onNPSDegreeProgramChange}
                                    defaultValue={this.state.userDetails.graduateDegreeProgram}
                                    value={this.state.userDetails.npsDegreeProgram}
                                    errorMessage={this.getNPSDegreeProgramValidationError()}
                                    className="form-dropdown"
                                    fluid />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormDropdown
                                    multiple
                                    label={`${this.localize("jobTitleText")}*`}
                                    items={this.state.jobTitleList}
                                    placeholder={this.localize("newRequestJobTitlePlaceholder")}
                                    loadingMessage={this.localize("loadingLabel")}
                                    onChange={this.onJobTitleChange}
                                    defaultValue={this.state.userDetails.jobTitle}
                                    value={this.state.userDetails.jobTitle}
                                    errorMessage={this.getJobTitleValidationError()}
                                    className="form-dropdown"
                                    fluid />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormDropdown
                                    label={`${this.localize("deptofStudyText")}*`}
                                    items={this.state.departmentOfStudyList}
                                    placeholder={this.localize("newRequestDeptOfStudyPlaceholder")}
                                    loadingMessage={this.localize("loadingLabel")}
                                    onChange={this.onDeptOfStudyChange}
                                    defaultValue={this.state.userDetails.deptOfStudy}
                                    value={this.state.userDetails.deptOfStudy}
                                    errorMessage={this.getDeptOfStudyValidationError()}
                                    className="form-dropdown"
                                    fluid />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("professionalCertificatesText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="professionalCertificates" id="professionalCertificates" fluid value={this.state.userDetails.professionalCertificates} onChange={this.onProfessionalCertificatesChange} />}
                                    errorMessage={this.getProfessionalCertificatesValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("professionalOrganizationsText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="professionalOrganizations" id="professionalOrganizations" fluid value={this.state.userDetails.professionalOrganizations} onChange={this.onProfessionalOrganizationsChange} />}
                                    errorMessage={this.getProfessionalOrganizationsValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("professionalExperienceText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="professionalExperience" id="professionalExperience" fluid value={this.state.userDetails.professionalExperience} onChange={this.onProfessionalExperienceChange} />}
                                    errorMessage={this.getProfessionalExperienceValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("professionalPublicationsText")}
                                    control={<Input placeholder={this.localize("placeholderText")} maxLength={100} name="professionalPublications" id="professionalPublications" fluid value={this.state.userDetails.professionalPublications} onChange={this.onProfessionalPublicationsChange} />}
                                    errorMessage={this.getProfessionalPublicationsValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("resumeLinkText")}
                                    control={<Input placeholder={this.localize("resumePlaceholderText")} name="resumeLink" id="resumeLink" fluid value={this.state.userDetails.resumeCVLink} onChange={this.onResumeLinkChange} />}
                                    errorMessage={this.getResumeLinkValidationError()}
                                />
                            </Flex.Item>
                        </Flex>

                        <Flex gap="gap.small">
                            <Flex.Item grow>
                                <FormField
                                    required
                                    label={this.localize("imageUrlText")}
                                    control={<Input placeholder={this.localize("photoPlaceholderText")} name="imageUrl" id="imageUrl" fluid value={this.state.userDetails.profilePictureImageURL} onChange={this.onImageURLChange} />}
                                    errorMessage={this.getImageURLValidationError()}
                                />
                            </Flex.Item>
                        </Flex>
                    </Flex>
                    <Flex.Item push>
                        <Flex>
                            <Flex.Item push>
                                <Button className="athena-button" content={this.localize("nextButtonText")} id="nextbtn" onClick={this.onNextButtonClick} />
                            </Flex.Item>
                        </Flex>
                    </Flex.Item>
                </Flex >
            );
        }
    }
}

export default withTranslation()(UserContactInfo);