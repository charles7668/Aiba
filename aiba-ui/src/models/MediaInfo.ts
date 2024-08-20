export interface ChapterInfo {
  chapterName: string;
  chapterUrl: string;
}

export interface TagInfo {
  name: string;
}

export interface GenreInfo {
  name: string;
}

export interface MediaInfo {
  name: string;
  author: string[];
  description: string;
  imageUrl: string;
  url: string;
  status: string;
  type: string;
  genres: GenreInfo[];
  tags: TagInfo[];
  chapters: ChapterInfo[];
  providerName: string;
  providerUrl: string;
}
