import React, { Component } from 'react';
import './App.css';
import Verkauf from './Verkauf';

class App extends Component {
  render() {
    return (
      <div className="App">
        {/* <header className="App-header">
          <img src="billmorro.png" className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header> */}
        <Verkauf />
      </div>
    );
  }
}

export default App;
