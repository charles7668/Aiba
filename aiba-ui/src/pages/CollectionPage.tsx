import React, { useEffect } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { SideBar } from '../components/SideBar.tsx';
import { Api } from '../services/Api.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { VscLibrary } from 'react-icons/vsc';
import { MediaInfo } from '../models/MediaInfo.ts';
import { MediaInfoCard } from '../components/MediaInfoCard.tsx';

export const CollectionPage: React.FC = () => {
  const [libraries, setLibraries] = React.useState([]);
  const [mediaInfos, setMediaInfos] = React.useState([]);
  useEffect(() => {
    Api.getLibraries().then(async (response) => {
      if (response.status !== 200) {
        return;
      }
      const libraries = await response.json();
      setLibraries(
        libraries.map((library: LibraryInfo) => {
          return {
            icon: VscLibrary,
            title: library.name,
            to: async () => {
              const response = await Api.getMediaInfosFromLibrary(library);
              if (response.status !== 200) {
                return;
              }
              const mediaInfos = await response.json();
              setMediaInfos(mediaInfos);
            },
          };
        }),
      );
    });
  }, []);
  return (
    <Box display={'flex'} justifyContent={'start'}>
      <SideBar items={libraries}></SideBar>
      <Box flex={'1'}>
        <MediaInfoList mediaInfos={mediaInfos} />
      </Box>
    </Box>
  );
};

const MediaInfoList = ({ mediaInfos }: { mediaInfos: Array<MediaInfo> }) => {
  return (
    <>
      <Flex flexWrap={'wrap'} justifyContent={'center'}>
        {mediaInfos.map((mediaInfo, index) => {
          return (
            <Box key={index} p={4}>
              <MediaInfoCard mediaInfo={mediaInfo}></MediaInfoCard>
            </Box>
          );
        })}
      </Flex>
    </>
  );
};