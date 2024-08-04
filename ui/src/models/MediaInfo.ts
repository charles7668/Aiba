import { ChapterInfo } from './ChapterInfo.ts';

export interface MediaInfo {
  name: string;
  author: string[];
  description: string;
  imageUrl: string;
  url: string;
  status: string;
  type: string;
  genres: string[];
  tags: string[];
  chapters: ChapterInfo[];
  providerName: string;
  providerUrl: string;
}
