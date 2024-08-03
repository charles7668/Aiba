import { SearchOption } from '../models/SearchOption.ts';
import { MediaInfo } from '../models/MediaInfo.ts';

const baseUrl = import.meta.env.VITE_API_BASEURL || '';

const search = async (params: SearchOption): Promise<MediaInfo[]> => {
  const response = await fetch(baseUrl + '/api/search', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(params),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data = await response.json();
  return data as MediaInfo[];
};

export const Api = {
  search,
};
