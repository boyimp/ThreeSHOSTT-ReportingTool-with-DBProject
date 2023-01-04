//@ts-check
import { Box } from '@chakra-ui/react';
import React from 'react';

const CustomVideoPlayer = ({
    src,
    cover,
    onLoadedData = () => {},
    onLoadedMetadata = () => {}
}) => {
    return (
        <Box position={"relative"} w={"full"} h={"full"}>
            <video
                crossOrigin="anonymous"
                className="mrnda-video-editor-area-videoplayer-wrapper--video"
                style={{
                    width: '100%',
                    height: '100%'
                }}
                controls
                preload="auto"
                poster={cover}
                data-setup="{}"
                onLoadedMetadata={onLoadedMetadata}
                onLoadedData={onLoadedData}>
                <source src={src} type={'video/mp4'} />
            </video>
        </Box>
    );
};

export default CustomVideoPlayer;
