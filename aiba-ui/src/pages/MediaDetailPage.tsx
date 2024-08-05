import { Box, Image, Text, HStack, Badge, VStack, Button, Link, Flex } from '@chakra-ui/react';
import React, { useEffect, useState } from 'react';
import { MediaInfo } from '../models/MediaInfo';
import { useParams, useSearchParams } from 'react-router-dom';
import { Api } from '../services/Api.ts';

const mediaInfoGenres: React.FC<{ mediaInfo: MediaInfo }> = ({ mediaInfo }) => {
  return (
    <>
      {
        mediaInfo.genres.map((genre, index) => {
          return (
            <Badge key={index} colorScheme="purple">{genre}</Badge>
          );
        })
      }
    </>
  );
};

const mediaInfoTags: React.FC<{ mediaInfo: MediaInfo }> = ({ mediaInfo }) => {
  return (
    <>
      {
        mediaInfo.tags.map((tag, index) => {
          return (
            <Badge key={index} colorScheme="purple">{tag}</Badge>
          );
        })
      }
    </>
  );
};

const mediaDetailContent: React.FC<{ mediaInfo: MediaInfo }> = ({ mediaInfo }) => {
  return (
    <Flex maxW="container.xl" mx="auto" p={4} bg="gray.900" color="white"
          flexDirection={'column'}
          justifyContent={'start'}>
      <Link textAlign={'start'} width={'100%'}
            isExternal href={mediaInfo.url} fontSize="2xl" mb={4}>{mediaInfo.name}</Link>
      <Box display="flex" mb={4} mt={4}>
        <Image
          src={mediaInfo.imageUrl}
          alt="cover Image"
          boxSize="300px"
          objectFit="cover"
        />
        <VStack align="start" ml={4} spacing={2}>
          <HStack spacing={2}>
            <Text textAlign={'start'}>Genre</Text>
            {mediaInfoGenres({ mediaInfo })}
          </HStack>
          <HStack spacing={2}>
            <Text textAlign={'start'}>Tags</Text>
            {mediaInfoTags({ mediaInfo })}
          </HStack>
          <Text textAlign={'start'}>
            Description : {mediaInfo.description}
          </Text>
          <Link textAlign={'start'} href={mediaInfo.providerUrl} isExternal>
            Provider Name : {mediaInfo.providerName}
          </Link>
          <HStack spacing={2}>
            <Button size="sm" colorScheme="orange">Start Reading</Button>
          </HStack>
        </VStack>
      </Box>
    </Flex>
  );
};

export const MediaDetailPage = () => {
  const [mediaInfo, setMediaInfo] = useState<MediaInfo | null>(null);
  const { providerName } = useParams<{ providerName: string }>();
  const [searchParams] = useSearchParams();
  const url = searchParams.get('url');

  useEffect(() => {
    console.log(providerName, url);
    if (providerName !== undefined && url != undefined) {
      Api.getDetailInfo(providerName, url).then((info) => {
        setMediaInfo(info);
      });
    }
  }, [providerName, url]);

  return (
    <>
      {!mediaInfo ? <div>no data</div> : mediaDetailContent({ mediaInfo })}
    </>
  );
};