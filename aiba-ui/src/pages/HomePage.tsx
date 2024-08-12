import React, { createContext, FC, useContext, useEffect } from 'react';
import {
  Box,
  Flex,
  IconButton,
  MenuItem,
  MenuList,
  Spinner,
  VStack,
} from '@chakra-ui/react';
import { SideBar } from '../components/SideBar.tsx';
import { Api } from '../services/Api.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { VscLibrary } from 'react-icons/vsc';
import { MediaInfo } from '../models/MediaInfo.ts';
import { MediaInfoCard } from '../components/MediaInfoCard.tsx';
import { MdRefresh } from 'react-icons/md';
import { FixedCountPagination } from '../components/Pagination.tsx';
import { useParams } from 'react-router-dom';

export const HomePage: React.FC = () => {
  const [libraries, setLibraries] = React.useState([]);
  const [mediaInfos, setMediaInfos] = React.useState<Array<MediaInfo>>([]);
  const [selectedLibrary, setSelectedLibrary] =
    React.useState<LibraryInfo | null>(null);
  const [isScanning, setIsScanning] = React.useState(false);
  const [currentPage, setCurrentPage] = React.useState(1);
  const { libraryName: initLibraryName } = useParams<{ libraryName: string }>();
  const [isLoading, setIsLoading] = React.useState(true);
  let timerId: NodeJS.Timeout | undefined = undefined;

  const updateMediaInfos = async (library: LibraryInfo, page = 1) => {
    const response = await Api.getMediaInfosFromLibrary(library, page);
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
            to: '/collection/' + encodeURIComponent(library.name),
          };
        })
      );
      const targetLibraryName = initLibraryName
        ? decodeURIComponent(initLibraryName)
        : undefined;
      if (targetLibraryName === undefined) return;
      const targetLibrary = libraries.find((l: LibraryInfo) => {
        return l.name === targetLibraryName;
      });
      if (targetLibrary !== undefined) {
        setSelectedLibrary(targetLibrary);
      }
    });
  }, [initLibraryName, selectedLibrary?.name]);
  useEffect(() => {
    if (selectedLibrary === null) {
      return;
    }
    updateMediaInfos(selectedLibrary, currentPage).then(() => {
      setIsLoading(false);
    });
  }, [currentPage, selectedLibrary]);
  return (
    <InformationContext.Provider
      value={{
        mediaInfos,
        libraryInfo: selectedLibrary,
        currentPage: 1,
        updateMediaInfos: setMediaInfos,
        updateLibraryInfo: setSelectedLibrary,
        updateCurrentPage: setCurrentPage,
      }}
    >
      <Box display={'flex'} justifyContent={'start'}>
        <SideBar items={libraries}></SideBar>
        {isLoading && initLibraryName !== undefined && (
          <Box
            display="flex"
            alignItems="center"
            justifyContent="center"
            height="100%"
            width="100%"
          >
            <Spinner size="xl" />
          </Box>
        )}
        {!isLoading && (
          <VStack flex={'1'}>
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
            {selectedLibrary && (
              <FixedCountPagination
                currentPage={currentPage}
                maxPage={1}
                onNextPageClick={() => {
                  if (currentPage < 2) return;
                  setCurrentPage(currentPage + 1);
                }}
                onPreviousPageClick={() => {
                  if (currentPage > 0) return;
                  setCurrentPage(currentPage - 1);
                }}
                onTargetPageClick={(page) => () => {
                  setCurrentPage(page);
                }}
              ></FixedCountPagination>
            )}
          </VStack>
        )}
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
  updateMediaInfos?: (mediaInfos: Array<MediaInfo>) => void;
  updateLibraryInfo?: (libraryInfo: LibraryInfo) => void;
  currentPage: number;
  updateCurrentPage?: (currentPage: number) => void;
}

const InformationContext = createContext<InformationContextProps>({
  mediaInfos: [],
  libraryInfo: null,
  currentPage: 1,
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
