import JiqimaoTvProvider from './providers/jiqimao-tv';
import WekanTvProvider from './providers/wekan-tv';
import {TvProvider} from "./types.ts";

const tvProviders: Array<TvProvider> = [

  new WekanTvProvider(),
  new JiqimaoTvProvider(),
];

function sim(s1: string, s2: string): number {

  const same = s1.split('').filter(x => s2.indexOf(x) !== -1).length;

  return 1 - 2 * same / (s1.length + s2.length);
}

export function findTvs(keyword: string): Promise<Array<Tv>> {

  const tasks = tvProviders.map(p => p.tvs(keyword).catch(e => {
    console.warn(e);

    return [];
  }))

  return Promise.all(tasks).then(
      ret => ret.flat().sort((x, y) => sim(x.title, keyword) -
                                       sim(y.title, keyword)));
}

export function findEpisodes(providerId, tvId): Promise<Array<Episode>> {

  const providers = tvProviders.filter(p => p.id == providerId);

  if (providers.length === 0)
    return Promise.resolve([]);

  return providers[0].episodes(tvId);
}

export function findStream(providerId, tvId, episodeId): Promise<Stream> {

  const providers = tvProviders.filter(p => p.id == providerId);

  if (providers.length === 0)
    return Promise.resolve([]);

  return providers[0].stream(tvId, episodeId);
}
