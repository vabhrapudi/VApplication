// <copyright file="card-image.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Text, ImageUnavailableIcon } from "@fluentui/react-northstar";
import { useTranslation } from "react-i18next";

import "./card-image.scss";

interface ICardImage {
    className: string | undefined,
    imageSrc: string
}

/**
 * Renders card image. If image not loaded successfully, then renders placeholder
 * @param props The props of type INewsArticleImage
 */
const CardImage: React.FunctionComponent<ICardImage> = props => {

    const localize = useTranslation().t;
    let [isImageLoaded, setImageLoaded] = React.useState(false);
    let [isErrorLoadingImage, setImageError] = React.useState(false);

    React.useEffect(() => {
        if (!props.imageSrc) {
            setImageError(true);
        }
    }, [props.imageSrc])

    /** The event handler called when image loaded successfully */
    const onImageLoaded = () => {
        setImageLoaded(true);
    }

    /** The event handler called when image was not loaded or user aborted loading image */
    const onImageNotLoaded = () => {
        setImageError(true);
    }

    const renderImagePlaceholder = () => {
        if (!isImageLoaded && !isErrorLoadingImage) {
            return (
                <Flex
                    className={`${props.className} card-image-placeholder-container`}
                    vAlign="center"
                    hAlign="center">
                    <Flex className="card-image-placeholder" vAlign="center" hAlign="center" gap="gap.small" fill>
                        <Text
                            content={localize("cardImageLoadingPlaceholder")}
                            align="center" size="medium"
                            weight="semibold"
                            color="white"
                        />
                    </Flex>
                </Flex>
            );
        }
        else if (isErrorLoadingImage) {
            return (
                <Flex
                    className={`${props.className} card-image-placeholder-container`}
                    vAlign="center"
                    hAlign="center">
                    <Flex className="card-image-placeholder" vAlign="center" hAlign="center" gap="gap.small" fill column>
                        <ImageUnavailableIcon className="placeholder-icon" />
                        <Text
                            content={localize("cardImageNotFoundPlaceholder")}
                            align="center" size="small"
                            color="white"
                        />
                    </Flex>
                </Flex>
            );
        }
    }

    return (
        <React.Fragment>
            <img
                className={`${props.className} ${isImageLoaded && !isErrorLoadingImage ? 'card-image-renderer image-loaded' : 'card-image-renderer image-not-loaded'}`}
                src={props.imageSrc}
                onLoad={onImageLoaded}
                onError={onImageNotLoaded}
                onAbort={onImageNotLoaded} />
            {renderImagePlaceholder()}
        </React.Fragment>
    );
}

export default CardImage;