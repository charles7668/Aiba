import React from 'react';
import {
  Box,
  Heading,
  Flex,
  FormControl,
  FormLabel,
  Input,
  Switch,
  Button,
  Divider,
  Text,
  Stack,
  Icon,
  VStack,
} from '@chakra-ui/react';
import { Link, Outlet } from 'react-router-dom';
import { MdDashboard } from 'react-icons/md';

const SidebarItem: React.FC<{
  icon: React.ElementType;
  title: string;
  to: string;
}> = ({ icon, title, to }) => {
  return (
    <Link to={to}>
      <Box
        w={'100%'}
        display="flex"
        alignItems="center"
        justifyContent="space-between"
        cursor="pointer"
        p={3}
        _hover={{ bg: 'gray.700' }}
      >
        <Box display="flex" alignItems="center">
          <Icon as={icon} mr={3} boxSize={6} />
          <Text>{title}</Text>
        </Box>
      </Box>
    </Link>
  );
};

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
  return (
    <>
      <Box display={'flex'} justifyContent={'start'}>
        <VStack align="start" spacing={4}>
          <SidebarItem
            icon={MdDashboard}
            title="Profile"
            to={'/settings/profile'}
          ></SidebarItem>
        </VStack>
        <Box flex={'1'}>
          <Outlet />
        </Box>
      </Box>
    </>
  );
};
