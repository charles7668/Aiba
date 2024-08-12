import React from 'react';
import { Box } from '@chakra-ui/react';
import { Outlet } from 'react-router-dom';
import { SideBar } from '../components/SideBar.tsx';
import { VscLibrary } from 'react-icons/vsc';
import { MdAccountCircle } from 'react-icons/md';

export const SettingsPage: React.FC = () => {
  const sideBarItems = [
    {
      icon: MdAccountCircle,
      title: 'Profile',
      to: '/settings/profile',
    },
    {
      icon: VscLibrary,
      title: 'Library',
      to: '/settings/library',
    },
  ];
  return (
    <>
      <Box display={'flex'} justifyContent={'start'}>
        <SideBar items={sideBarItems} />
        <Box flex={'1'}>
          <Outlet />
        </Box>
      </Box>
    </>
  );
};
