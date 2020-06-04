import React from 'react';
import ReactSelect from 'react-select';
import ReactHlsPlayer from 'react-hls-player';

import WekanTv from '../sources/wekan-tv';

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
      selected: null,
      partList: [],
    };
  }

  loadVideo(id) {
    if (id === null) {
      return this.setState({
        loading: false,
        selected: null,
        partList: [],
      });
    }

    this.setState({ loading: true }, () => {
      WekanTv.getVideoParts(id).then((ret) => {
        this.setState({
          loading: false,
          selected:
            ret.partList.length > 0
              ? {
                  value: ret.partList[0].url,
                  label: ret.partList[0].part_title,
                }
              : null,
          partList: ret.partList,
        });
      });
    });
  }

  componentDidMount() {
    this.loadVideo(this.props.id);
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.id != this.props.id) {
      this.loadVideo(this.props.id);
    }
  }

  playNext() {
    const { selected, partList } = this.state;

    for (let i = 0; i < partList.length; i++) {
      if (selected.label === partList[i].part_title) {
        if (i + 1 < partList.length) {
          this.setState({
            selected: {
              value: partList[i + 1].url,
              label: partList[i + 1].part_title,
            },
          });
        }
      }
    }
  }

  render() {
    return (
      <div>
        <div className="Select">
          {this.state.partList.length > 0 ? (
            <ReactSelect
              isLoading={this.state.loading}
              value={this.state.selected}
              onChange={(selected) => this.setState({ selected })}
              options={this.state.partList.map((part) => ({
                value: part.url,
                label: part.part_title,
              }))}
            />
          ) : null}
        </div>
        <div className="Player">
          {this.state.selected && (
            <RefreshableHlsPlayer
              autoplay={true}
              url={`http:${this.state.selected.value}`}
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
