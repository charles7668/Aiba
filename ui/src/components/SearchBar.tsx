import React from 'react';
import { Box, Flex, Icon, IconButton, Input } from '@chakra-ui/react';
import { MdSearch } from 'react-icons/md';
import './SearchBar.css';

export const SearchBar: React.FC = () => {
  return (
    <Box id="search-bar">
      <Flex>
        <Input placeholder="please input your search text" />
        <IconButton aria-label="search" icon={<Icon as={MdSearch} />} />
      </Flex>
    </Box>
  );
};
