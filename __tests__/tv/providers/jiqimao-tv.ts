import {Episode, Stream, Tv, TvProvider} from "../../../src/tv/api.ts";
import JiqimaoTvProvider from "../../../src/tv/providers/jiqimao-tv.ts";

const provider: TvProvider = new JiqimaoTvProvider();

test("tvs", async () => {
  const tvs: Tv[] = await provider.tvs("隐秘的角落");

  expect(tvs[0]).toStrictEqual({
    id : 'a331582dfd27fbcddd3d41edc96552c3890dc044',
    providerId : 'tv.jiqimao',
    title : '隐秘的角落',
    cover :
        'https://tu.tianzuida.com/pic/upload/vod/2020-06-16/202006161592307387.jpg'
  });
});

test("episodes", async () => {
  const episodes =
      await provider.episodes("a331582dfd27fbcddd3d41edc96552c3890dc044");

  expect(episodes[0]).toStrictEqual({
    id : 'b0cbf653d10708e206f64f372313c684f28eec42',
    tvId : 'a331582dfd27fbcddd3d41edc96552c3890dc044',
    providerId : 'tv.jiqimao',
    title : '01'
  });
});

test("stream", async () => {
  const stream: Stream =
      await provider.stream("a331582dfd27fbcddd3d41edc96552c3890dc044",
                            "b0cbf653d10708e206f64f372313c684f28eec42");

  expect(stream.providerId).toBe('tv.jiqimao');
  expect(stream.tvId).toBe('a331582dfd27fbcddd3d41edc96552c3890dc044');
  expect(stream.episodeId).toBe('b0cbf653d10708e206f64f372313c684f28eec42');

  expect(stream.url).toBeTruthy();

  expect(stream.format).toBe("m3u8");
});
