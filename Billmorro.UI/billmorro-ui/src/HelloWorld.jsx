import React from 'react';
import dotnetify from 'dotnetify';

dotnetify.hubServerUrl = "http://localhost:5000";

class HelloWorld extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("HelloWorld", this);
        this.state = { Greetings: "", ServerTime: "" };
    }
    render() {
        return (
            <div>
                {this.state.Greetings}<br />
                The server time is: {this.state.ServerTime}
            </div>
        );
    }
}
export default HelloWorld;