import React from 'react';
import dotnetify from 'dotnetify';
import './kasse.css';
import Touchpad from './Touchpad';
import Bon from './Bon';
import Artikelliste from './Artikelliste';

dotnetify.hubServerUrl = "http://localhost:5000";

class Kasse extends React.Component {
  constructor(props,context) {
    super(props,context);
    this.vm = dotnetify.react.connect("KasseVM", this);
  }

  render() {
    return (
      <div className="verkauf">
        <Bon />
        <Touchpad />
        <Artikelliste />
      </div>
    );
  }
}


export default Kasse;