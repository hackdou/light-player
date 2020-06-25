export interface Tv {

  // Tv id.
  id: string;

  // Tv provider id.
  providerId: string;

  // Title.
  title: string;

  // Cover image url.
  cover: string;
}

export interface Episode {

  // Episode id.
  id: string;

  // Related tv id.
  tvId: string;

  // Related tv provider id.
  providerId: string;

  // Title.
  title: string;
}

export interface Stream {

  // Episode id.
  episodeId: string;

  // Tv id.
  tvId: string;

  // Provider id.
  providerId: string;

  // Video stream url.
  url: string;
}

export interface TvProvider {

  // Provider id.
  id: string;

  // Search for tvs.
  tvs(keyword: string): Promise<Tv[]>;

  // Retrive all episodes.
  episodes(tvId: string): Promise<Episode[]>;

  // Retrive stream url.
  stream(tvId: string, episodeId: string): Promise<Stream>;
}
