import { MediaInfo } from './MediaInfo.ts';
import { LibraryInfo } from './LibraryInfo.ts';

export interface RemoveMediaInfoRequest {
  mediaInfo: MediaInfo;
  libraryInfo: LibraryInfo;
}
