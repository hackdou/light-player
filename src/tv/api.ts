import JiqimaoTvProvider from './providers/jiqimao-tv';
import WekanTvProvider from './providers/wekan-tv';
import {Episode, Stream, Tv, TvProvider} from "./types";

const tvProviders: Array<TvProvider> = [

  new WekanTvProvider(),
  new JiqimaoTvProvider(),
];

function sim(s1: string, s2: string): number {

  const same = s1.split('').filter(x => s2.indexOf(x) !== -1).length;

  return 1 - 2 * same / (s1.length + s2.length);
}

function flatten(a: any[]): any[] {

  let result = [];

  for (let i = 0; i < a.length; i++) {
    for (let j = 0; j < a[i].length; j++) {
      result.push(a[i][j]);
    }
  }

  return result;
}

export function findTvs(keyword: string): Promise<Array<Tv>> {

  const tasks = tvProviders.map(p => p.tvs(keyword).catch(() => []));

  return Promise.all(tasks).then(
      ret => flatten(ret).sort((x, y) => sim(x.title, keyword) -
                                         sim(y.title, keyword)));
}

export function findEpisodes(providerId: string,
                             tvId: string): Promise<Array<Episode>> {

  const providers = tvProviders.filter(p => p.id == providerId);

  if (providers.length === 0)
    return Promise.resolve([]);

  return providers[0].episodes(tvId).catch(() => []);
}

export function findStream(providerId: string, tvId: string,
                           episodeId: string): Promise<Stream|null> {

  const providers = tvProviders.filter(p => p.id == providerId);

  if (providers.length === 0)
    return Promise.resolve(null);

  return providers[0].stream(tvId, episodeId).catch(() => null);
}
