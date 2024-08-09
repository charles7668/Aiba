import { SearchOption } from '../models/SearchOption.ts';
import { MediaInfo } from '../models/MediaInfo.ts';
import { RegisterInfo } from '../models/RegisterInfo.ts';
import { LibraryInfo } from '../models/LibraryInfo.ts';
import { RemoveMediaInfoRequest } from '../models/RemoveMediaInfoRequest.ts';

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
  url: string,
  libraryName: string | null
): Promise<MediaInfo> => {
  const encodedProviderName = encodeURIComponent(providerName);
  const encodedUrl = encodeURIComponent(url);
  const encodedLibraryName = libraryName
    ? encodeURIComponent(libraryName)
    : null;
  const response = await fetch(
    baseUrl +
      '/api/MediaInfo/detail/' +
      encodedProviderName +
      '?url=' +
      encodedUrl +
      (encodedLibraryName ? '&library=' + encodedLibraryName : ''),
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

const removeMediaInfoFromLibrary = async (request: RemoveMediaInfoRequest) => {
  return await fetch(baseUrl + '/api/mediaInfo', {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify(request),
  });
};

const getMediaInfosFromLibrary = async (libraryInfo: LibraryInfo) => {
  return await fetch(
    baseUrl + '/api/mediaInfo?libraryName=' + libraryInfo.name,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
    }
  );
};

const getScannerList = async (mediaTypeFlagNumber: number) => {
  return await fetch(
    baseUrl + '/api/Scan/scanner?flagNumber=' + mediaTypeFlagNumber,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
    }
  );
};

const startMediaInfoScan = async ({ libraryName }: { libraryName: string }) => {
  return await fetch(baseUrl + '/api/Scan/mediaInfos', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify({ libraryName }),
  });
};

const getMediaInfoScanStatus = async (libraryName: string) => {
  libraryName = encodeURIComponent(libraryName);
  return await fetch(baseUrl + '/api/Scan/status?library=' + libraryName, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
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
  removeMediaInfoFromLibrary,
  getMediaInfosFromLibrary,
  getScannerList,
  getMediaInfoScanStatus,
  startMediaInfoScan,
  baseUrl,
};
