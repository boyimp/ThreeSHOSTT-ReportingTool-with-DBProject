import { Box, SkeletonText } from '@chakra-ui/react';

const Loading = ({ line, ...rest }) => {
    return (
        <>
            <Box rounded="xl" {...rest}>
                <SkeletonText noOfLines={line} spacing="4" />
            </Box>
        </>
    );
};

export default Loading;
