import {findTvs} from '../src/tv/api';

test("findTv.1", async () => {
  let tvs = await findTvs("隐秘的角落");

  expect(tvs[0].title).toBe("隐秘的角落");
});

test("findTv.2", async () => {
  let tvs = await findTvs("四驱兄弟");

  expect(tvs[0].title).toBe("四驱兄弟");
});
