import React from 'react';
import { Box, Flex } from '@chakra-ui/react';

export const HomePage: React.FC = () => {
  return (
    <>
      <Flex flexDirection={'column'} height={'100%'}>
        <Box flex={'1'}>
          <h1>Home Page</h1>
        </Box>
      </Flex>
    </>
  );
};
