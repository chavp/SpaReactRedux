import React, { Component } from 'react';

function Hello(props) {
    return <div><h1>
        Hello, {props.name}
    </h1>
    <a href="#" onClick={handleClick}>Click</a></div>;
}

function handleClick(e) {
    e.preventDefault();
    console.log('The link was clicked.');
}

export default class IntroReact extends React.Component {
    constructor(props) {
        super(props);
        this.state = { count: 0 };
    }

    render() {
        return <Hello name={this.state.count}/>;
    }
}