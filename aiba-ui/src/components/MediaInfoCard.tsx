import {
  Flex,
  Image,
  Link,
  Spacer,
  Text,
  Menu,
  MenuButton,
  IconButton,
} from '@chakra-ui/react';
import React, { ReactNode } from 'react';
import { MediaInfo } from '../models/MediaInfo.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { UrlHelper } from '../services/UrlHelper.ts';
import { VerticalDotsIcon } from '../icons/VerticalDotsIcon.ts';

interface MediaCardProps {
  mx?: string;
  my?: string;
  mediaInfo: MediaInfo;
  libraryInfo: LibraryInfo | null;
  menuComponents?: ReactNode;
}

export const MediaInfoCard: React.FC<MediaCardProps> = ({
  mediaInfo,
  menuComponents,
  libraryInfo,
  ...props
}) => {
  const realImageUrl = UrlHelper.ImageUrlConverter(
    mediaInfo.url,
    mediaInfo.imageUrl
  );
  let detailLink =
    '/detail/' +
    encodeURIComponent(mediaInfo.providerName) +
    '?url=' +
    encodeURIComponent(mediaInfo.url);
  if (libraryInfo !== null) {
    detailLink += '&library=' + encodeURIComponent(libraryInfo.name);
  }
  return (
    <Flex
      flexDirection={'column'}
      maxW={'200px'}
      width={'200px'}
      maxH={'300px'}
      height={'300px'}
      borderWidth="1px"
      borderColor="gray.200"
      position={'relative'}
      {...props}
    >
      <Image
        src={realImageUrl}
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
        href={detailLink}
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
          bottom="0"
          right="0"
        />
        {menuComponents}
      </Menu>
    </Flex>
  );
};
