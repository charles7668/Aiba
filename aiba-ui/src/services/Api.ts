import { SearchOption } from '../models/SearchOption.ts';
import { MediaInfo } from '../models/MediaInfo.ts';
import { RegisterInfo } from '../models/RegisterInfo.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';

const baseUrl = import.meta.env.VITE_API_BASEURL || '';

const search = async (params: SearchOption): Promise<MediaInfo[]> => {
  const response = await fetch(baseUrl + '/api/search', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(params),
  });

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data = await response.json();
  return data as MediaInfo[];
};

const getDetailInfo = async (
  providerName: string,
  url: string
): Promise<MediaInfo> => {
  const encodedProviderName = encodeURIComponent(providerName);
  const encodedUrl = encodeURIComponent(url);
  const response = await fetch(
    baseUrl +
      '/api/MediaInfo/detail/' +
      encodedProviderName +
      '?url=' +
      encodedUrl,
    {
      method: 'GET',
      credentials: 'include',
    }
  );

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  const data = await response.json();
  return data as MediaInfo;
};

const login = async (username: string, password: string) => {
  return await fetch(baseUrl + '/api/Account/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify({ username, password }),
  });
};

const register = async (registerInfo: RegisterInfo) => {
  return await fetch(baseUrl + '/api/Account/register', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(registerInfo),
  });
};

const authorizeStatus = async () => {
  return await fetch(baseUrl + '/api/Account/status', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
  });
};

const logout = async () => {
  return await fetch(baseUrl + '/api/Account/logout', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
  });
};

const getLibraries = async () => {
  return await fetch(baseUrl + '/api/Library', {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
  });
};

const addLibrary = async (info: LibraryInfo) => {
  return await fetch(baseUrl + '/api/Library', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(info),
  });
};

const deleteLibrary = async (info: LibraryInfo) => {
  return await fetch(baseUrl + '/api/Library', {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(info),
  });
};

const addMediaInfoToLibrary = async (
  mediaInfo: MediaInfo,
  libraryInfo: LibraryInfo
) => {
  return await fetch(baseUrl + '/api/mediaInfo', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify({ mediaInfo, libraryInfo }),
  });
};

export const Api = {
  search,
  getDetailInfo,
  login,
  register,
  authorizeStatus,
  logout,
  getLibraries,
  addLibrary,
  deleteLibrary,
  addMediaInfoToLibrary,
  baseUrl,
};
