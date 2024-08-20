import { Api } from './Api.ts';

/*
  if the url is a local file path, convert it to a url that can be accessed by the server
 */
const LocalImageUrlConverter = (imageBaseUrl: string, imageUrl: string) => {
  if (imageUrl.startsWith('file://')) {
    imageUrl = imageUrl.replace('file://', '');
    imageBaseUrl = imageBaseUrl.replace('file://', '');
    return (
      Api.baseUrl +
      '/api/Image/' +
      encodeURIComponent(imageBaseUrl) +
      '/' +
      encodeURIComponent(imageUrl)
    );
  }
  return imageUrl;
};

export const UrlHelper = {
  ImageUrlConverter: LocalImageUrlConverter,
};
