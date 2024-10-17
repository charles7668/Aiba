import { FC, useEffect, useState } from 'react';
import { Box, Spinner } from '@chakra-ui/react';
import { useParams, useSearchParams } from 'react-router-dom';
import { Api } from '../services/Api.ts';

function getRealImageLink(mediaUrl: string | null, link: string) {
  return (
    Api.baseUrl +
    '/api/Image/' +
    encodeURIComponent(mediaUrl ?? '') +
    '/' +
    encodeURIComponent(link)
  );
}

export const MangeReadingPage: FC = () => {
  const { providerName } = useParams<{ providerName: string }>();
  const [searchParams] = useSearchParams();
  const mediaUrl = searchParams.get('url');
  const libraryName = searchParams.get('library');
  const chapterName = searchParams.get('chapter');
  const [isLoading, setIsLoading] = useState(true);
  const [isNoData, setIsNoData] = useState(false);
  const [imageLinks, setImageLinks] = useState<string[]>([]);
  const [loadedImageLinks, setLoadedImageLinks] = useState<string[]>([]);

  if (mediaUrl === null || providerName === undefined || libraryName === null) {
    setIsNoData(true);
  }

  useEffect(() => {
    if (providerName === 'local') {
      Api.getImageLinks(
        providerName,
        mediaUrl as string,
        libraryName as string,
        chapterName
      ).then(async (response) => {
        if (response.status !== 200) {
          setIsNoData(true);
          return;
        }
        const data = await response.json();
        setImageLinks(data);

        setIsLoading(false);
      });
    }
  }, [chapterName, libraryName, mediaUrl, providerName]);

  useEffect(() => {
    if (!isLoading && imageLinks.length > 0) {
      const img = new Image();
      const link = imageLinks[0];
      img.src = getRealImageLink(mediaUrl, link ?? '');
      img.onload = () => {
        setLoadedImageLinks((prev) => [...prev, img.src]);
        setImageLinks(imageLinks.slice(1));
      };
    }
  }, [imageLinks, isLoading, mediaUrl]);

  // return if any of the required data is missing
  if (isNoData) return <div>no data</div>;

  return (
    <>
      {isLoading && (
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
        <Box display="flex" justifyContent="center">
          <Box width={'80%'}>
            {loadedImageLinks.map((link, index) => (
              <img width={'100%'} src={link} key={index} alt={''} />
            ))}
          </Box>
        </Box>
      )}
    </>
  );
};
