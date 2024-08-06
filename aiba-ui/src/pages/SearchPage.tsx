import '../components/MainContent.css';
import { SearchBar } from '../components/SearchBar.tsx';
import { Box, Flex } from '@chakra-ui/react';
import React from 'react';
import { MediaCard } from '../components/MediaCard.tsx';
import { Api } from '../services/Api.ts';
import { MediaInfo } from '../models/MediaInfo.ts';
import { Pagination } from '../components/Pagination.tsx';
import { TopToolBar } from '../components/TopToolBar.tsx';

const renderMediaCards = (mediaInfos: MediaInfo[]) => {
  const cards = [];
  for (let i = 0; i < mediaInfos.length; i++) {
    cards.push(
      <MediaCard mediaInfo={mediaInfos[i]} key={i} mx={'5px'} my={'5px'} />
    );
  }
  return cards;
};

export const SearchPage: React.FC = () => {
  const [mediaInfos, setMediaInfos] = React.useState<MediaInfo[]>([]);
  const [page, setPage] = React.useState(0);
  const [hasData, setHasData] = React.useState(false);
  const [searchText, setSearchText] = React.useState('');
  const search = async (inputSearchText: string, page: number) => {
    setSearchText(inputSearchText);
    const response = await Api.search({
      searchType: 'all',
      searchText: inputSearchText,
      page: page,
    });
    setMediaInfos(response);
  };
  const onSearchClick = async (searchText: string) => {
    setPage(1);
    await search(searchText, 1);
    setHasData(true);
  };
  const onNextPageClick = async () => {
    if (mediaInfos.length === 0) {
      // no more data exists
      return;
    }
    setPage(page + 1);
    await search(searchText, page + 1);
  };
  const onPreviousPageClick = async () => {
    if (page === 1) {
      return;
    }
    setPage(page - 1);
    await search(searchText, page - 1);
  };
  return (
    <>
      <TopToolBar />
      <div id="main-content">
        <Box css={'max-width: 80%;'} mx={'auto'}>
          <Flex direction="column">
            <SearchBar mt={'5px'} mb={'5px'} onSearchClick={onSearchClick} />
            <Flex flexWrap={'wrap'} justifyContent={'center'}>
              {renderMediaCards(mediaInfos)}
            </Flex>
            {hasData && (
              <Pagination
                currentPage={page}
                onNextPageClick={onNextPageClick}
                onPreviousPageClick={onPreviousPageClick}
              />
            )}
          </Flex>
        </Box>
      </div>
    </>
  );
};
