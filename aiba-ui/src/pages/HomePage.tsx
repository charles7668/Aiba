import React, { useEffect } from 'react';
import { Box, Flex, IconButton } from '@chakra-ui/react';
import { SideBar } from '../components/SideBar.tsx';
import { Api } from '../services/Api.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { VscLibrary } from 'react-icons/vsc';
import { MediaInfo } from '../models/MediaInfo.ts';
import { MediaInfoCard } from '../components/MediaInfoCard.tsx';
import { MdRefresh } from 'react-icons/md';

export const HomePage: React.FC = () => {
  const [libraries, setLibraries] = React.useState([]);
  const [mediaInfos, setMediaInfos] = React.useState<Array<MediaInfo>>([]);
  const [selectedLibrary, setSelectedLibrary] =
    React.useState<LibraryInfo | null>(null);
  const getMediaInfos = async (library: LibraryInfo) => {
    const response = await Api.getMediaInfosFromLibrary(library);
    if (response.status !== 200) {
      return;
    }
    const mediaInfos = await response.json();
    setMediaInfos(mediaInfos);
  };
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
              setSelectedLibrary(library);
              await getMediaInfos(library);
            },
          };
        })
      );
    });
  }, []);
  return (
    <Box display={'flex'} justifyContent={'start'}>
      <SideBar items={libraries}></SideBar>
      <Box flex={'1'}>
        <MediaInfoList
          mediaInfos={mediaInfos}
          libraryInfo={selectedLibrary}
          mediaInfoNeedUpdateCallback={() => {
            if (selectedLibrary === null) {
              return;
            }
            getMediaInfos(selectedLibrary).then(() => {});
          }}
        />
      </Box>
    </Box>
  );
};

const MediaInfoList = ({
  mediaInfos,
  libraryInfo,
  mediaInfoNeedUpdateCallback,
}: {
  mediaInfos: Array<MediaInfo>;
  libraryInfo: LibraryInfo | null;
  mediaInfoNeedUpdateCallback: () => void;
}) => {
  const [isScanning, setIsScanning] = React.useState(false);
  let timerId: NodeJS.Timeout | undefined = undefined;
  const startScanTask = async () => {
    if (libraryInfo === null) {
      return;
    }
    const response = await Api.startMediaInfoScan({
      libraryName: libraryInfo.name,
    });
    if (response.status !== 200) {
      return;
    }
    setIsScanning(true);
    timerId = setInterval(async () => {
      const response = await Api.getMediaInfoScanStatus(libraryInfo.name);
      if (response.status !== 200 || (await response.text()) === 'false') {
        setIsScanning(false);
        clearInterval(timerId);
        return;
      }
      mediaInfoNeedUpdateCallback();
    }, 2000);
    return () => {
      if (timerId !== undefined) {
        clearInterval(timerId);
      }
    };
  };
  return (
    <>
      {libraryInfo !== null && (
        <Box>
          <IconButton
            isLoading={isScanning}
            aria-label="scan"
            icon={<MdRefresh />}
            onClick={() => startScanTask()}
          ></IconButton>
        </Box>
      )}
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
