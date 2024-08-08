import { MediaTypeFlag } from './MediaTypeEnum.ts';

export interface LibraryInfo {
  name: string;
  path: string;
  type: MediaTypeFlag;
  scannerName: string;
}
