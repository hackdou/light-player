import WekanTvProvider from "../../../src/tv/providers/wekan-tv";
import {Episode, Stream, Tv, TvProvider} from "../../../src/tv/types";

const provider: TvProvider = new WekanTvProvider();

test("tvs", async () => {
  const tvs: Tv[] = await provider.tvs("越狱兔前传");

  expect(tvs[0]).toStrictEqual({
    id : '301922525477001',
    providerId : 'tv.wekan',
    title : '越狱兔前传',
    cover :
        'https://asset.bixjf.com/2019/tv/20190531/dfda9b724f376038d6f5211ebbcd61ef.jpg',
  });
});

test("episodes", async () => {
  const episodes: Episode[] = await provider.episodes("301922525477001");

  expect(episodes.length).toBe(13);

  expect(episodes[0]).toStrictEqual({
    id : '161922525477001',
    tvId : '301922525477001',
    providerId : 'tv.wekan',
    title : '第1集',
  });
});

test("stream", async () => {
  const stream: Stream =
      await provider.stream("301922525477001", "161922525477001");

  expect(stream.providerId).toBe('tv.wekan');
  expect(stream.tvId).toBe('301922525477001');
  expect(stream.episodeId).toBe('161922525477001');

  expect(stream.url).toBeTruthy();

  expect(stream.format).toBe("m3u8");
});
