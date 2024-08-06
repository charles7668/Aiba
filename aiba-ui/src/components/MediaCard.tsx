import {
  Flex,
  Image,
  Link,
  Spacer,
  Text,
  Menu,
  MenuList,
  MenuItem,
  MenuButton,
  IconButton,
  createIcon,
} from '@chakra-ui/react';
import React from 'react';
import { MediaInfo } from '../models/MediaInfo.ts';

interface MediaCardProps {
  mx?: string;
  my?: string;
  mediaInfo: MediaInfo;
}

const VerticalDotsIcon = createIcon({
  displayName: 'VerticalDotsIcon',
  viewBox: '0 0 24 24',
  d: 'M12 16a2 2 0 110 4 2 2 0 010-4zm0-6a2 2 0 110 4 2 2 0 010-4zm0-6a2 2 0 110 4 2 2 0 010-4z',
});

export const MediaCard: React.FC<MediaCardProps> = ({
  mediaInfo,
  ...props
}) => {
  return (
    <Flex
      flexDirection={'column'}
      maxW={'250px'}
      width={'250px'}
      maxH={'350px'}
      height={'350px'}
      borderWidth="1px"
      borderColor="gray.200"
      position={'relative'}
      {...props}
    >
      <Image
        src={mediaInfo.imageUrl}
        alt={'test'}
        width="100%"
        height={'250px'}
        maxH={'250px'}
        objectFit="cover"
      />
      <Link
        textAlign="start"
        isExternal={true}
        display="-webkit-box"
        overflow="hidden"
        textOverflow="ellipsis"
        whiteSpace="normal"
        css={{
          WebkitLineClamp: 2,
          WebkitBoxOrient: 'vertical',
        }}
        href={
          'detail/' +
          encodeURIComponent(mediaInfo.providerName) +
          '?url=' +
          encodeURIComponent(mediaInfo.url)
        }
      >
        {mediaInfo.name}
      </Link>
      {mediaInfo.author !== undefined && mediaInfo.author.length > 0 && (
        <Text textAlign={'start'}>Author : {mediaInfo.author}</Text>
      )}
      <Spacer />
      <Link textAlign="start" isExternal={true} href={mediaInfo.providerUrl}>
        Source : {mediaInfo.providerName}
      </Link>
      <Menu>
        <MenuButton
          as={IconButton}
          aria-label="Options"
          icon={<VerticalDotsIcon />}
          position="absolute"
          variant={'unstyled'}
          size={'xs'}
          bottom="4px"
          right="4px"
        />
        <MenuList>
          <MenuItem>Option 1</MenuItem>
          <MenuItem>Option 2</MenuItem>
          <MenuItem>Option 3</MenuItem>
        </MenuList>
      </Menu>
    </Flex>
  );
};
