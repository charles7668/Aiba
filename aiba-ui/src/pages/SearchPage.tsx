import '../components/MainContent.css';
import { SearchBar } from '../components/SearchBar.tsx';
import {
  Box,
  Flex,
  Menu,
  MenuButton,
  MenuItem,
  MenuList,
  useToast,
} from '@chakra-ui/react';
import React, { FC, useEffect, useState } from 'react';
import { MediaInfoCard } from '../components/MediaInfoCard.tsx';
import { Api } from '../services/Api.ts';
import { MediaInfo } from '../models/MediaInfo.ts';
import { Pagination } from '../components/Pagination.tsx';
import { LibraryInfo } from '../models/LibraryInfo.ts';

const MenuComponents: FC<{
  mediaInfo: MediaInfo;
  libraries: Array<LibraryInfo>;
}> = ({ mediaInfo, libraries }) => {
  const [addToLibrarySubMenuOpen, setAddToLibrarySubMenuOpen] = useState(false);
  const toast = useToast();
  const handleMouseEnter = () => {
    setAddToLibrarySubMenuOpen(true);
  };
  const handleMouseLeave = () => {
    setAddToLibrarySubMenuOpen(false);
  };
  return (
    <MenuList p={0} m={0} width={'100%'}>
      {libraries.length > 0 && (
        <MenuItem
          pr={0}
          onMouseEnter={handleMouseEnter}
          onMouseLeave={handleMouseLeave}
        >
          <Menu
            isOpen={addToLibrarySubMenuOpen}
            placement={'right'}
            offset={[0, 0]}
          >
            <MenuButton
              w="full"
              borderRadius="none"
              border={'none'}
              textAlign={'left'}
            >
              Add to Library
            </MenuButton>
            <MenuList
              onMouseEnter={handleMouseEnter}
              onMouseLeave={handleMouseLeave}
            >
              {libraries.map((library, index) => {
                return (
                  <MenuItem
                    key={index}
                    onClick={async () => {
                      const response = await Api.addMediaInfoToLibrary(
                        mediaInfo,
                        library
                      );
                      if (response.status !== 200) {
                        toast({
                          title: 'Error ' + response.status,
                          description:
                            'Failed to add media to library : ' +
                            (await response.text()),
                          status: 'error',
                          duration: 5000,
                          isClosable: true,
                        });
                        return;
                      }
                      toast({
                        title: 'Success',
                        description: 'Successfully added media to library',
                        status: 'success',
                        duration: 5000,
                        isClosable: true,
                      });
                    }}
                  >
                    {library.name}
                  </MenuItem>
                );
              })}
            </MenuList>
          </Menu>
        </MenuItem>
      )}
    </MenuList>
  );
};

const renderMediaCards = ({
  mediaInfos,
  libraries,
}: {
  mediaInfos: MediaInfo[];
  libraries: Array<LibraryInfo>;
}) => {
  const cards = [];

  for (let i = 0; i < mediaInfos.length; i++) {
    cards.push(
      <MediaInfoCard
        menuComponents={
          <MenuComponents mediaInfo={mediaInfos[i]} libraries={libraries} />
        }
        mediaInfo={mediaInfos[i]}
        key={i}
        mx={'5px'}
        my={'5px'}
      />
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
  const [libraries, setLibraries] = useState([]);
  useEffect(() => {
    Api.getLibraries().then(async (response) => {
      if (response.status !== 200) {
        return;
      }
      const libraries = await response.json();
      setLibraries(libraries);
    });
  }, []);
  return (
    <>
      <div id="main-content">
        <Box css={'max-width: 80%;'} mx={'auto'}>
          <Flex direction="column">
            <SearchBar mt={'5px'} mb={'5px'} onSearchClick={onSearchClick} />
            <Flex flexWrap={'wrap'} justifyContent={'center'}>
              {renderMediaCards({ mediaInfos, libraries })}
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
