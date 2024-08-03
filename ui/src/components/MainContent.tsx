import './MainContent.css';
import { SearchBar } from './SearchBar.tsx';
import { Box, Flex } from '@chakra-ui/react';
import React from 'react';
import { MediaCard } from './MediaCard.tsx';
import { Api } from '../services/Api.ts';
import { MediaInfo } from '../models/MediaInfo.ts';

const renderMediaCards = (mediaInfos: MediaInfo[]) => {
  const cards = [];
  for (let i = 0; i < mediaInfos.length; i++) {
    cards.push(
      <MediaCard mediaInfo={mediaInfos[i]} key={i} mx={'5px'} my={'5px'} />
    );
  }
  return cards;
};

export const MainContent: React.FC = () => {
  const [mediaInfos, setMediaInfos] = React.useState<MediaInfo[]>([]);
  const onSearchClick = async (searchText: string) => {
    const response = await Api.search({
      searchType: 'all',
      searchText: searchText,
      page: 1,
    });
    setMediaInfos(response);
  };
  return (
    <div id="main-content">
      <Box css={'max-width: 80%;'} mx={'auto'}>
        <Flex direction="column">
          <SearchBar mt={'5px'} mb={'5px'} onSearchClick={onSearchClick} />
          <Flex flexWrap={'wrap'} justifyContent={'center'}>
            {renderMediaCards(mediaInfos)}
          </Flex>
        </Flex>
      </Box>
    </div>
  );
};
