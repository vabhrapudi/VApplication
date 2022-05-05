// <copyright file="athena-ingestion.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import moment from "moment";
import { Flex, Table, Checkbox, Text, Button, Input, Loader } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import { RouteComponentProps, withRouter } from "react-router-dom";
import { cloneDeep } from "lodash";
import { updateAthenaIngestionAsync, getAthenaIngestionDetailsAsync } from "../../api/athena-ingestion-api";
import { IAthenaIngestionItems } from "../../models/athena-ingestion";
import * as microsoftTeams from "@microsoft/teams-js";
import { useEffect } from "react";
import { StatusCodes } from "http-status-codes";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import { IsValidUrl } from '../../helpers/url-helper';
import StatusBar from "../common/status-bar/status-bar";

import "./athena-ingestion.scss";



interface IAthenaIngestionProps extends RouteComponentProps {
}

export const AthenaIngestion: React.FunctionComponent<IAthenaIngestionProps> = (props: IAthenaIngestionProps) => {

    const localize = useTranslation().t;

    const [tableRows, setTableRows] = React.useState<any[]>([]);
    const [athenaIngestion, setAthenaIngestion] = React.useState<IAthenaIngestionItems[]>([]);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [isPageLoading, setIsPageLoading] = React.useState<boolean>(true);
    const [isFilePathURL, setIsFilePathURL] = React.useState<boolean>(true)
    const tableDbEntityColumnDesign = { minWidth: "22vw", maxWidth: "22vw" };
    const tableUpdateColumnDesign = { minWidth: "18vw", maxWidth: "18vw" };
    const tableFrequencyColumnDesign = { minWidth: "10vw", maxWidth: "18vw" };
    const tableFilePathColumnDesign = { minWidth: "25vw", maxWidth: "25vw" };
    const tableLastUpdatedColumnDesign = { minWidth: "20vw", maxWidth: "20vw" };


    useEffect(() => {
        microsoftTeams.initialize();
        athenaIngestionData();
    }, []);

    useEffect(() => {
        getTableRows();
    }, [athenaIngestion]);

    // Gets Athena ingestion details.
    const athenaIngestionData = async () => {
        let response = await getAthenaIngestionDetailsAsync(handleTokenAccessFailure);
        if (response && response.status === StatusCodes.OK) {
            setIsPageLoading(false);
            setIsLoading(false)
            setAthenaIngestion(response.data);
        }
        else {
            setIsPageLoading(false);
            setIsLoading(false)
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
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

    /**
     * Handles the file change.
     * @param evenData event data.
     * @param fileName file name.
     */
    const onFilePathChange = (evenData: any, fileName: string | undefined) => {
        var value = evenData.target.value;

        var isValidURL = IsValidUrl(value)
        if (!isValidURL) {
            setStatus({ id: status.id + 1, message: localize(`requiredStringFilePathURLText`), type: ActivityStatus.Error });
        }

        var existingIngestionData = cloneDeep(athenaIngestion);
        var selectedIngestion = existingIngestionData.find(item => item.dbEntity === fileName);

        if (selectedIngestion) {
            selectedIngestion.filePath = evenData.target.value;

            setAthenaIngestion(existingIngestionData);
            setIsFilePathURL(true);
        }
    }

    // Validate and return image URL validation error.
    const getFilePathURLValidationError = (url) => {
        let errorMessage = "";
        var isValidUrl = IsValidUrl(url);
        if (!isFilePathURL && !url) {
            errorMessage = localize(`requiredFilePathURL`);
        }
        if (url && !isValidUrl) {
            errorMessage = localize(`requiredStringFilePathURLText`);
        }

        return errorMessage;
    }

    /**
     * Handles the update button activity.
     * @param fileName file name.
     * @param entity entity name.
     */
    const updateButtonHandler = async (entity: string | undefined, filePath: string | undefined) => {
        var isUpdate = checkIfUpdateAllowed(filePath)
        if (!isUpdate) {
            setStatus({ id: status.id + 1, message: localize(`invalidFilePathText`), type: ActivityStatus.Error });
        }
        else {
            setIsPageLoading(true);
            setIsLoading(true)
            var response = await updateAthenaIngestionAsync(entity!, filePath!, handleTokenAccessFailure);
            if (isUpdate && response.status === StatusCodes.OK) {
                setIsPageLoading(false);
                setIsLoading(false)
                setStatus({ id: status.id + 1, message: localize(`updateSuccessfulText`), type: ActivityStatus.Success });
                
                return athenaIngestionData();
            }
            else {
                setIsPageLoading(false);
                setIsLoading(false)
                setStatus({ id: status.id + 1, message: localize(`updateFailedText`), type: ActivityStatus.Error });;
            }
        }
    }

    // Table header
    const tableHeader = {
        key: 'header',
        items: [
            {
                content:
                    <Flex gap="gap.small" className="table-title" >
                        <Text content={localize("dbEntityTitle")} />
                    </Flex>,
                design: tableDbEntityColumnDesign
            },
            {
                content: <Flex gap="gap.small" className="table-title" >
                    <Text content={localize("filePathTitle")} />
                </Flex>,
                design: tableFilePathColumnDesign
            },
            {
                content: <Flex gap="gap.small" className="table-title" >
                    <Text content={localize("lastUpdatedTitle")} />
                </Flex>,
                design: tableLastUpdatedColumnDesign
            },
            {
                content: <Flex gap="gap.small" className="table-title" >
                    <Text content={localize("frequencyTitle")} />
                </Flex>,
                design: tableFrequencyColumnDesign
            },
            {
                content: <Flex gap="gap.small" className="table-title" >
                    <Text content={localize("updateTitle")} />
                </Flex>,
                design: tableUpdateColumnDesign
            }
        ],
    };


    // Sets the table row.
    const getTableRows = () => {
        debugger
        setTableRows(athenaIngestion.map((item: IAthenaIngestionItems, index: number) => ({
            key: index,
            items:
                [
                    {
                        content: <Text content={item.dbEntity} title={item.dbEntity} />,
                        design: tableDbEntityColumnDesign
                    },
                    {
                        content: <Input clearable placeholder="Enter file url..." value={item.filePath} onChange={(event: any, eventData: any) => onFilePathChange(eventData, item.dbEntity)} />,
                        design: tableFilePathColumnDesign
                    },
                    {
                        content: <Text content={item.updatedAt ? moment(item.updatedAt).format("DD-MMM-YYYY hh:mm A") : "NA"} />,
                        design: tableLastUpdatedColumnDesign
                    },
                    {
                        content: <Text content={item.frequency} />,
                        design: tableFrequencyColumnDesign
                    },
                    {
                        content: <Button className="athena-button" content={localize("updateButtonText")} onClick={() => updateButtonHandler(item.dbEntity, item.filePath)} />,
                        design: tableUpdateColumnDesign
                    }
                ]
        })));
    }

    const checkIfUpdateAllowed = (filePath) => {
        var filePathValidationStatus = {
            isFilePathURL: true,
        };
        if (!filePath || !IsValidUrl(filePath)) {
            filePathValidationStatus.isFilePathURL = false
        }

        setIsFilePathURL(filePathValidationStatus.isFilePathURL);

        return filePathValidationStatus.isFilePathURL;
    }

    return (
        <Flex column >
            <StatusBar status={status} isMobile={false} />
            <Loader hidden={!isLoading} className="page-loader" />
            {
                isPageLoading ?
                    <Loader className="page-loader" />
                    :
                    <Flex column>
                        <Flex padding="padding.medium" className="title" vAlign="center" hAlign="center">
                            <Text content="Athena Ingestion Manager" size="large" weight="bold" />
                        </Flex>
                        {<Table
                            header={tableHeader}
                            rows={tableRows}
                            className="collections-table-style"
                        />}
                    </Flex>               
            }
'       </Flex>)
};

export default withRouter(AthenaIngestion);