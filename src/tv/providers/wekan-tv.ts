import Axios from "axios";

import {Episode, Stream, Tv, TvProvider} from "../types.ts";

const client = Axios.create({
  baseURL : "https://www.wekan.tv/index.php",
});

client.interceptors.response.use((ret) => {
  if (ret.data.code !== 0)
    throw new Error(ret.data);

  return ret.data.data;
});

const PROVIDER_ID = "tv.wekan";

export default class WekanTvProvider implements TvProvider {

  public id: string = PROVIDER_ID;

  public tvs(keyword: string): Promise<Tv[]> {

    return client
        .get("/search/list", {
          params : {
            page : 1,
            pagesize : 10,
            content : keyword,
          }
        })
        .then(ret => ret.list.map(({_id, title, photo}) => <Tv>{
          id : `${_id}`,
          providerId : PROVIDER_ID,
          title : title,
          cover : photo
        }));
  }

  public episodes(tvId: string): Promise<Episode[]> {

    return client.get("/video/part", {params : {tvid : tvId}})
        .then(ret => ret.partList.map(({part_id, part_title}) => <Episode>{
          id : `${part_id}`,
          tvId : tvId,
          providerId : PROVIDER_ID,
          title : part_title,
        }));
  }

  public stream(tvId: string, episodeId: string): Promise<Stream> {

    return client.get("/video/part", {params : {tvid : tvId}})
        .then(ret =>
                  ret.partList.filter(({part_id}) => `${part_id}` === episodeId)
                      .map(({part_title, url}) => <Stream>{
                        providerId : PROVIDER_ID,
                        tvId : tvId,
                        episodeId : episodeId,
                        url : `http:${url}`,
                        format : 'm3u8',
                      })[0]);
  }
}
