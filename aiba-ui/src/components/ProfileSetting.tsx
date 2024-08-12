import React, { useEffect, useState } from 'react';
import {
  Avatar,
  Box,
  Button,
  Flex,
  Heading,
  IconButton,
  useToast,
} from '@chakra-ui/react';
import { FaCamera } from 'react-icons/fa6';
import { Api } from '../services/Api.ts';
import { UserSetting } from '../models/UserSetting.ts';
import { useUserSetting } from '../modules/useUserSetting.ts';

export const ProfileSetting: React.FC = () => {
  const [avatar, setAvatar] = useState<string | undefined>(undefined);
  const toast = useToast();
  const { userSetting, setUserSetting } = useUserSetting();

  const updateSettings = async () => {
    const setting: UserSetting = {
      coverImage: avatar === undefined ? '' : avatar,
    };
    const response = await Api.updateUserSetting(setting);
    if (response.status !== 200) {
      toast({
        title: 'Failed to update settings status code : ' + response.status,
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
      return;
    }
    setUserSetting(setting);
    toast({
      title: 'Settings updated successfully!',
      status: 'success',
      duration: 3000,
      isClosable: true,
    });
  };

  const handleAvatarChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        setAvatar(e.target?.result as string);
        toast({
          title: 'Avatar updated successfully!',
          status: 'success',
          duration: 3000,
          isClosable: true,
        });
      };
      reader.readAsDataURL(file);
    }
  };

  useEffect(() => {
    setAvatar(userSetting?.coverImage || undefined);
  }, [userSetting]);

  return (
    <>
      <Box display={'flex'}>
        <Box p={6} maxW="1200px" mx="auto">
          <Heading as="h1" mb={6}>
            Profile Settings
          </Heading>
          <Flex alignItems="center" mb={6}>
            <Avatar size="xl" src={avatar} mr={4} />
            <Box>
              <input
                type="file"
                accept="image/*"
                id="avatar-upload"
                style={{ display: 'none' }}
                onChange={handleAvatarChange}
              />
              <label htmlFor="avatar-upload">
                <IconButton
                  as="span"
                  icon={<FaCamera />}
                  aria-label="Change Avatar"
                />
              </label>
            </Box>
          </Flex>
          <Flex justifyContent={'center'}>
            <Button colorScheme={'blue'} onClick={updateSettings}>
              Update
            </Button>
          </Flex>
        </Box>
      </Box>
    </>
  );
};
