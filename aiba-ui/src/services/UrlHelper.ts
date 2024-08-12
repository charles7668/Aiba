import { Api } from './Api.ts';

/*
  if the url is a local file path, convert it to a url that can be accessed by the server
 */
const LocalImageUrlConverter = (url: string) => {
  if (url.startsWith('file://')) {
    const replaceFileProtocol = url.replace('file://', '');
    return (
      Api.baseUrl + '/api/Image/' + encodeURIComponent(replaceFileProtocol)
    );
  }
  return url;
};

export const UrlHelper = {
  ImageUrlConverter: LocalImageUrlConverter,
};
