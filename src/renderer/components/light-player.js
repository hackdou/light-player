import React from 'react';

import LeftPanel from './left-panel';
import MainPanel from './main-panel';

class LightPlayer extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      showPanel: true,
      id: null,
    };
  }

  componentDidMount() {
    document.addEventListener('keypress', (e) => {
      if (e.ctrlKey && e.code === 'KeyF') {
        this.setState({ showPanel: !this.state.showPanel });
      }
    });
  }

  handleOpen(id) {
    this.setState({
      showPanel: false,
      id: id,
    });
  }

  render() {
    return (
      <div className="LightPlayer">
        <title>Light Player</title>

        <div className={`LeftPanel ${this.state.showPanel ? 'On' : 'Off'}`}>
          <LeftPanel onOpen={(id) => this.handleOpen(id)} />
        </div>
        <div className="MainPanel">
          <MainPanel id={this.state.id} />
        </div>
      </div>
    );
  }
}

export default LightPlayer;
