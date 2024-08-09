import React, { createContext, FC, useContext, useEffect } from 'react';
import { Box, Flex, IconButton, MenuItem, MenuList } from '@chakra-ui/react';
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
  const [isScanning, setIsScanning] = React.useState(false);
  let timerId: NodeJS.Timeout | undefined = undefined;

  const updateMediaInfos = async (library: LibraryInfo) => {
    const response = await Api.getMediaInfosFromLibrary(library);
    if (response.status !== 200) {
      return;
    }
    const mediaInfos = await response.json();
    setMediaInfos(mediaInfos);
  };
  const startScanTask = async () => {
    if (selectedLibrary === null) {
      return;
    }
    const response = await Api.startMediaInfoScan({
      libraryName: selectedLibrary.name,
    });
    if (response.status !== 200) {
      return;
    }
    setIsScanning(true);
    timerId = setInterval(async () => {
      const response = await Api.getMediaInfoScanStatus(selectedLibrary.name);
      if (response.status !== 200) {
        // if response is not success then not need to update
        setIsScanning(false);
        clearInterval(timerId);
        return;
      } else if ((await response.text()) === 'false') {
        setIsScanning(false);
        clearInterval(timerId);
      }
      await updateMediaInfos(selectedLibrary);
    }, 2000);
    return () => {
      if (timerId !== undefined) {
        clearInterval(timerId);
      }
    };
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
              await updateMediaInfos(library);
            },
          };
        })
      );
    });
  }, []);
  return (
    <InformationContext.Provider
      value={{
        mediaInfos,
        libraryInfo: selectedLibrary,
        updateMediaInfos: setMediaInfos,
        updateLibraryInfo: setSelectedLibrary,
      }}
    >
      <Box display={'flex'} justifyContent={'start'}>
        <SideBar items={libraries}></SideBar>
        <Box flex={'1'}>
          {selectedLibrary !== null && (
            <Box>
              <IconButton
                isLoading={isScanning}
                aria-label="scan"
                icon={<MdRefresh />}
                onClick={() => startScanTask()}
              ></IconButton>
            </Box>
          )}
          <MediaInfoList />
        </Box>
      </Box>
    </InformationContext.Provider>
  );
};

interface MenuContentProps {
  mediaInfo: MediaInfo;
}

interface InformationContextProps {
  mediaInfos: Array<MediaInfo>;
  libraryInfo: LibraryInfo | null;
  updateMediaInfos?: (mediaInfos: Array<MediaInfo>) => void | null;
  updateLibraryInfo?: (libraryInfo: LibraryInfo) => void | null;
}

const InformationContext = createContext<InformationContextProps>({
  mediaInfos: [],
  libraryInfo: null,
});

const RemoteMediaInfoMenuContent: FC<MenuContentProps> = (props) => {
  const { mediaInfo } = props;
  const { mediaInfos, libraryInfo, updateMediaInfos } =
    useContext(InformationContext);
  const removeMediaInfo = async () => {
    if (libraryInfo === null) {
      return;
    }
    const response = await Api.removeMediaInfoFromLibrary({
      mediaInfo,
      libraryInfo,
    });
    if (response.status !== 200) {
      return;
    }
    if (updateMediaInfos)
      updateMediaInfos(mediaInfos.filter((m) => m !== mediaInfo));
  };
  return (
    <MenuList>
      <MenuItem onClick={removeMediaInfo}>Remove From Library</MenuItem>
    </MenuList>
  );
};
const LocalMediaInfoMenuContent: FC<MenuContentProps> = (props) => {
  const { mediaInfo } = props;
  const { mediaInfos, libraryInfo, updateMediaInfos } =
    useContext(InformationContext);
  const removeMediaInfo = async () => {
    if (libraryInfo === null) {
      return;
    }
    const response = await Api.removeMediaInfoFromLibrary({
      mediaInfo,
      libraryInfo,
    });
    if (response.status !== 200) {
      return;
    }
    if (updateMediaInfos)
      updateMediaInfos(mediaInfos.filter((m) => m !== mediaInfo));
  };
  return (
    <MenuList>
      <MenuItem onClick={removeMediaInfo}>Remove From Library</MenuItem>
    </MenuList>
  );
};

const MediaInfoList = () => {
  const { mediaInfos, libraryInfo } = useContext(InformationContext);
  return (
    <>
      <Flex flexWrap={'wrap'} justifyContent={'center'}>
        {mediaInfos.map((mediaInfo, index) => {
          const isLocalProvider =
            mediaInfo.providerName.toLowerCase() === 'local';
          return (
            <Box key={index} p={4}>
              <MediaInfoCard
                mediaInfo={mediaInfo}
                menuComponents={
                  isLocalProvider ? (
                    <LocalMediaInfoMenuContent mediaInfo={mediaInfo} />
                  ) : (
                    <RemoteMediaInfoMenuContent mediaInfo={mediaInfo} />
                  )
                }
                libraryInfo={libraryInfo}
              ></MediaInfoCard>
            </Box>
          );
        })}
      </Flex>
    </>
  );
};
