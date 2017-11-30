import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import {HubConnection} from '@aspnet/signalr-client/dist/browser/signalr-client-1.0.0-alpha2-final';

//ReactDOM.render(<App />, document.getElementById('root'));
//registerServiceWorker();


console.log(HubConnection);


let connection = new HubConnection('http://127.0.0.1/kasse');

connection.on('updatebon', data => {
    var DisplayMessagesDiv = document.getElementById("root");
    DisplayMessagesDiv.innerHTML += "<br/>" + data;
});

connection.start().then(() => alert('connected'));


/*
connection.start().then(() => connection.invoke('send', 'Hello'));
function SendMessage(){
    var msg = document.getElementById("txtMessage").value;
    connection.invoke('send', msg);
}*/