import React from "react";

import IpcProxy from "../../ipc/proxy";
import IpcEvent from "../../ipc/event";

class LeftPanel extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      keyword: "",
      tvs: [],
      loading: false,
    };
  }

  handleChange(e) {
    this.setState({ keyword: e.target.value });
  }

  handleSubmit(e) {
    e.preventDefault();

    this.setState({ loading: true }, () => {
      IpcProxy.invoke(IpcEvent.FIND_TVS, { keyword: this.state.keyword }).then(
        (tvs) => {
          this.setState({ tvs, loading: false });
        }
      );
    });
  }

  handleClick(tv) {
    this.props.onClick && this.props.onClick(tv);
  }

  render() {
    return (
      <div>
        <form className="Search" onSubmit={this.handleSubmit.bind(this)}>
          <input
            type="search"
            placeholder="search"
            onChange={this.handleChange.bind(this)}
          />
        </form>

        <div className="List">
          {this.state.tvs.map((tv) => (
            <div
              key={tv.id}
              style={{ backgroundImage: `url(${tv.cover})` }}
              onClick={() => this.handleClick(tv)}
            >
              <div className="List-Title">{tv.title}</div>
            </div>
          ))}
        </div>
      </div>
    );
  }
}

export default LeftPanel;
