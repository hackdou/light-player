import React from "react";
import ReactSelect from "react-select";
import ReactHlsPlayer from "react-hls-player";

import IpcProxy from "../../ipc/proxy";
import IpcEvent from "../../ipc/event";

class RefreshableHlsPlayer extends ReactHlsPlayer {
  constructor(props) {
    super(props);
  }

  componentDidUpdate(prevProps) {
    if (this.props.url != prevProps.url) {
      this._initPlayer();
    }
  }
}

class MainPanel extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      episodes: [],
      stream: null,
    };
  }

  loadStream(episode) {
    this.setState({ loading: true }, () => {
      IpcProxy.invoke(IpcEvent.FIND_STREAM, {
        providerId: episode.providerId,
        tvId: episode.tvId,
        episodeId: episode.id,
      }).then((stream) => {
        this.setState({
          loading: false,
          stream: stream,
        });
      });
    });
  }

  loadEpisodes(tv, idx = 0) {
    if (tv === null) {
      return this.setState({
        loading: false,
        episodes: [],
        stream: null,
      });
    }

    console.log("loadEpisodes");

    this.setState({ loading: true }, () => {
      IpcProxy.invoke(IpcEvent.FIND_EPISODES, {
        providerId: tv.providerId,
        tvId: tv.id,
      }).then((episodes) => {
        console.log("episodes", episodes);

        this.setState(
          {
            loading: false,
            episodes: episodes,
            stream: null,
          },
          () => {
            if (episodes.length > 0) {
              this.loadStream(episodes[0]);
            }
          }
        );
      });
    });
  }

  componentDidMount() {
    this.loadEpisodes(this.props.tv);
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.tv !== this.props.tv) {
      this.loadEpisodes(this.props.tv);
    }
  }

  playingEpisode() {
    const { stream, episodes } = this.state;

    if (stream === null) return [null, null];

    for (let i = 0; i < episodes.length; i++) {
      if (stream.episodeId === episodes[i].id) {
        return [episodes[i], i];
      }
    }
  }

  playNext() {
    const { episodes } = this.state;

    let [_, idx] = this.playingEpisode();

    if (idx !== null && idx + 1 < episodes.length) {
      this.loadStream(episodes[idx + 1]);
    }
  }

  handleChange({ value }) {
    const episodes = this.state.episodes.filter(
      (episode) => episode.id === value
    );

    if (episodes.length > 0) {
      this.loadStream(episodes[0]);
    }
  }

  renderValue() {
    const [episode, _] = this.playingEpisode();

    if (episode === null) {
      return null;
    } else {
      return {
        value: episode.id,
        label: episode.title,
      };
    }
  }

  renderOptions() {
    return this.state.episodes.map(({ title, id }) => ({
      value: id,
      label: title,
    }));
  }

  render() {
    return (
      <div>
        <div className="Select">
          {this.state.episodes.length > 1 && (
            <ReactSelect
              isLoading={this.state.loading}
              onChange={this.handleChange.bind(this)}
              options={this.renderOptions()}
              value={this.renderValue()}
            />
          )}
        </div>
        <div className="Player">
          {this.state.stream && (
            <RefreshableHlsPlayer
              autoplay={true}
              url={this.state.stream.url}
              height="100%"
              width="100%"
              videoProps={{
                disablePictureInPicture: true,
                onEnded: () => this.playNext(),
              }}
            />
          )}
        </div>
      </div>
    );
  }
}

export default MainPanel;
