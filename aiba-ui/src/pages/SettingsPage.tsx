import React from 'react';
import {
  Box,
  Button,
  Divider,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Input,
  Stack,
  Switch,
} from '@chakra-ui/react';
import { Outlet } from 'react-router-dom';
import { SideBar } from '../components/SideBar.tsx';
import { VscLibrary } from 'react-icons/vsc';

export const ProfileSetting: React.FC = () => {
  return (
    <Box display={'flex'}>
      <Box p={6} maxW="1200px" mx="auto">
        <Heading as="h1" mb={6}>
          Settings
        </Heading>
        <Flex direction={{ base: 'column', md: 'row' }} gap={6}>
          <Box flex={1} p={6} bg="white" boxShadow="md" rounded="md">
            <Heading as="h2" size="md" mb={4}>
              User Information
            </Heading>
            <Stack spacing={4}>
              <FormControl>
                <FormLabel>Username</FormLabel>
                <Input placeholder="Enter your username" />
              </FormControl>
              <FormControl>
                <FormLabel>Email</FormLabel>
                <Input placeholder="Enter your email" type="email" />
              </FormControl>
              <FormControl>
                <FormLabel>Password</FormLabel>
                <Input placeholder="Enter a new password" type="password" />
              </FormControl>
              <Button colorScheme="blue">Save Changes</Button>
            </Stack>
          </Box>
          <Box flex={1} p={6} bg="white" boxShadow="md" rounded="md">
            <Heading as="h2" size="md" mb={4}>
              Account Settings
            </Heading>
            <Stack spacing={4}>
              <FormControl display="flex" alignItems="center">
                <FormLabel mb="0">Enable two-factor authentication</FormLabel>
                <Switch />
              </FormControl>
              <FormControl display="flex" alignItems="center">
                <FormLabel mb="0">Allow passwordless login</FormLabel>
                <Switch />
              </FormControl>
              <Button colorScheme="blue">Update Settings</Button>
            </Stack>
          </Box>
        </Flex>
        <Divider my={6} />
        <Box p={6} bg="white" boxShadow="md" rounded="md">
          <Heading as="h2" size="md" mb={4}>
            Notification Settings
          </Heading>
          <Stack spacing={4}>
            <FormControl display="flex" alignItems="center">
              <FormLabel mb="0">Email Notifications</FormLabel>
              <Switch />
            </FormControl>
            <FormControl display="flex" alignItems="center">
              <FormLabel mb="0">SMS Notifications</FormLabel>
              <Switch />
            </FormControl>
            <FormControl display="flex" alignItems="center">
              <FormLabel mb="0">Push Notifications</FormLabel>
              <Switch />
            </FormControl>
            <Button colorScheme="blue">Save Notification Preferences</Button>
          </Stack>
        </Box>
      </Box>
    </Box>
  );
};

export const SettingsPage: React.FC = () => {
  const sideBarItems = [
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
