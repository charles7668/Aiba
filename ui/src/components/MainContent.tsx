import './MainContent.css';
import { SearchBar } from './SearchBar.tsx';
import { Box, Flex } from '@chakra-ui/react';
import React from 'react';

export const MainContent: React.FC = () => {
  return (
    <div id="main-content">
      <Box css={'max-width: 80%;'} mx={'auto'} mt={5}>
        <Flex direction="column">
          <SearchBar />
        </Flex>
      </Box>
    </div>
  );
};
