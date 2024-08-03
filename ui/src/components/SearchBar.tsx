import React from 'react';
import { Box, Flex, Icon, IconButton, Input } from '@chakra-ui/react';
import { MdSearch } from 'react-icons/md';
import './SearchBar.css';

interface SearchBarPops {
  id?: string;
  mt?: string;
  mb?: string;
  onSearchClick: (searchText: string) => void;
}

export const SearchBar: React.FC<SearchBarPops> = ({
  onSearchClick,
  ...props
}) => {
  const [searchText, setSearchText] = React.useState('');
  const handleKeyDown = async (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      onSearchClick(searchText);
    }
  };
  return (
    <Box {...props}>
      <Flex>
        <Input
          placeholder="please input your search text"
          value={searchText}
          onKeyDown={handleKeyDown}
          onChange={async (e) => setSearchText(e.target.value)}
        />
        <IconButton
          aria-label="search"
          icon={<Icon as={MdSearch} />}
          onClick={async () => {
            onSearchClick(searchText);
          }}
        />
      </Flex>
    </Box>
  );
};
