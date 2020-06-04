import Axios from 'axios';

const client = Axios.create({
  baseURL: 'https://www.wekan.tv/index.php',
});

client.interceptors.response.use((ret) => {
  if (ret.data.code === 0) {
    return ret.data.data;
  } else {
    console.error(ret.data);
    return null;
  }
});

const WekanTv = {
  search: (content, page = 1, pagesize = 20) =>
    client.get('/search/list', {
      params: {
        page,
        content,
        pagesize,
      },
    }),
  getVideoParts: (id) =>
    client.get('/video/part', {
      params: {
        tvid: id,
      },
    }),
};

export default WekanTv;
