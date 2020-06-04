import React from 'react';

import WekanTv from '../sources/wekan-tv';

class LeftPanel extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      search: '',
      list: [],
    };
  }

  handleSearchChange(e) {
    this.setState({ search: e.target.value });
  }

  submit(e) {
    e.preventDefault();

    WekanTv.search(this.state.search).then((ret) => {
      this.setState({ list: ret.list });
    });
  }

  handleClick(id) {
    if (this.props.onOpen) {
      this.props.onOpen(id);
    }
  }

  render() {
    return (
      <div>
        <form className="Search" onSubmit={(e) => this.submit(e)}>
          <input
            type="search"
            placeholder="search"
            onChange={(e) => this.handleSearchChange(e)}
          />
        </form>

        <div className="List">
          {this.state.list.map((item) => (
            <div
              key={item._id}
              style={{ backgroundImage: `url(${item.photo})` }}
              onClick={() => this.handleClick(item._id)}
            >
              <div className="List-Title">{item.title}</div>
            </div>
          ))}
        </div>
      </div>
    );
  }
}

export default LeftPanel;
