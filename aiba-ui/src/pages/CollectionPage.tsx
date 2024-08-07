import React, { useEffect } from 'react';
import { Box } from '@chakra-ui/react';
import { SideBar } from '../components/SideBar.tsx';
import { Api } from '../services/Api.ts';
import { MdBook } from 'react-icons/md';
import { LibraryInfo } from '../models/LibraryInfo.ts';

export const CollectionPage: React.FC = () => {
  const [libraries, setLibraries] = React.useState([]);
  useEffect(() => {
    Api.getLibraries().then(async (response) => {
      if (response.status !== 200) {
        return;
      }
      const libraries = await response.json();
      setLibraries(
        libraries.map((library: LibraryInfo) => {
          return {
            icon: MdBook,
            title: library.name,
            to: () => {
              // todo get media infos from library
            },
          };
        })
      );
    });
  }, []);
  return (
    <Box display={'flex'} justifyContent={'start'}>
      <SideBar items={libraries}></SideBar>
      <Box flex={'1'}></Box>
    </Box>
  );
};
