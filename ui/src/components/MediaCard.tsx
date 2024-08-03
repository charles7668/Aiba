import { Box, Image, Link, Text } from '@chakra-ui/react';
import React from 'react';
import { MediaInfo } from '../models/MediaInfo.ts';

interface MediaCardProps {
  mx?: string;
  my?: string;
  mediaInfo: MediaInfo;
}

export const MediaCard: React.FC<MediaCardProps> = ({
  mediaInfo,
  ...props
}) => {
  return (
    <Box
      maxW={'250px'}
      width={'250px'}
      maxH={'350px'}
      height={'350px'}
      borderWidth="1px"
      borderColor="gray.200"
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
      <Link textAlign="start" href={mediaInfo.url}>
        {mediaInfo.name}
      </Link>
      <Text textAlign="start">{mediaInfo.author}</Text>
    </Box>
  );
};
