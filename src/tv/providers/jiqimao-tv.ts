import Axios from "axios"
import adapter from 'axios/lib/adapters/http';
import Cheerio from "cheerio";

import {Episode, Tv, TvProvider} from "../types.ts";

const webClient = Axios.create({
  baseURL : "http://jiqimao.tv/",
  adapter,
});

webClient.interceptors.response.use((ret) => Cheerio.load(ret.data));

const apiRClient = Axios.create({
  baseURL : "http://apir.jiqimao.tv/",
  adapter,
});

apiRClient.interceptors.response.use((ret) => {
  if (!ret.data.success)
    throw new Error("Request failed from jiqimap.tv");

  return ret.data;
});

const apiCkClient = Axios.create({
  baseURL : "http://apick.jiqimao.tv/",
  adapter,
});

apiCkClient.interceptors.response.use((ret) => {
  if (!ret.data.success)
    throw new Error("Request failed from jiqimap.tv");

  return ret.data;
});

const PROVIDER_ID = "tv.jiqimao";

export default class JiqimaoTvProvider implements TvProvider {
  public id: string = PROVIDER_ID;

  public tvs(keyword: string): Promise<Tv[]> {

    return webClient.get(`/search/video/${encodeURI(keyword)}`)
        .then(
            $ =>
                $(".search-tv-box")
                    .map((_, elem) => {
                      const href = $(elem.parent).attr("href");

                      return <Tv>{
                        id : href.split("/")[href.split("/").length - 1].trim(),
                        providerId : PROVIDER_ID,
                        title : $(".search-tv-title", elem).text().trim(),
                        cover : $(".search-tv-img", elem)
                                    .attr('data-original')
                                    .trim()
                      };
                    })
                    .get());
  }

  public episodes(tvId: string): Promise<Episode[]> {

    return webClient.get(`/movie/show/${tvId}`)
        .then($ => {
          const href = $("ul.more-play-link-drama li a").first().attr("href");

          return href.split("/")[href.split("/").length - 1];
        })
        .then(sid => apiRClient.get("/service/player/episode", {
          params : {sid},
        }))
        .then(({data}) => data[0].episodes.map(({name, sid}) => <Episode>{
          id : sid,
          tvId : tvId,
          providerId : PROVIDER_ID,
          title : name,
        }));
  }

  public stream(tvId: string, episodeId: string): Promise<Stream> {

    return apiCkClient
        .get('/service/ckplayer/parser', {
          params : {sid : episodeId, type : '1', mode : 'phone'},
        })
        .then(ret => <Stream>{
          providerId : PROVIDER_ID,
          tvId : tvId,
          episodeId : episodeId,
          url : ret.data.parser.url
        });
  }
}
