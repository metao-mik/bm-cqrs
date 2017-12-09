import React, { Component } from 'react';
import './App.css';
import Kasse from './Kasse';

class App extends Component {
  render() {
    return (
      <div className="App">
        {/* <header className="App-header">
          <img src="billmorro.png" className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header> */}
        <Kasse />
      </div>
    );
  }
}

export default App;
