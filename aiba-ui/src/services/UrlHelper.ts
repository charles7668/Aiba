import { Api } from './Api.ts';

/*
  handle local image url
 */
const LocalImageUrlConverter = (url: string) => {
  if (url.startsWith('file://')) {
    const replaceFileProtocol = url.replace('file://', '');
    return (
      Api.baseUrl + '/api/Image/' + encodeURIComponent(replaceFileProtocol)
    );
  }
  return Api.baseUrl + '/api/Image/' + encodeURIComponent(url);
};

export const UrlHelper = {
  ImageUrlConverter: LocalImageUrlConverter,
};
