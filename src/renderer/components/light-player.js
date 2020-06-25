import React from "react";

import LeftPanel from "./left-panel";
import MainPanel from "./main-panel";

import IpcProxy from "../../ipc/proxy";
import IpcEvent from "../../ipc/event";

class LightPlayer extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      showPanel: true,
      tv: null,
    };
  }

  componentDidMount() {
    document.addEventListener("keypress", (e) => {
      if (e.ctrlKey && e.code === "KeyF") {
        this.setState({ showPanel: !this.state.showPanel });
      }
    });
  }

  handleClick(tv) {
    this.setState({
      showPanel: false,
      tv: tv,
    });
  }

  render() {
    return (
      <div className="LightPlayer">
        <title>Light Player</title>

        <div className={`LeftPanel ${this.state.showPanel ? "On" : "Off"}`}>
          <LeftPanel onClick={(tv) => this.handleClick(tv)} />
        </div>

        <div className="MainPanel">
          <MainPanel tv={this.state.tv} />
        </div>
      </div>
    );
  }
}

export default LightPlayer;
