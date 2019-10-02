import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import TicTacTo from './components/tictacto/Game';
import IntoReact from './components/IntoReact';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/counter' component={Counter} />
    <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
    <Route path='/tic-tac-to' component={TicTacTo} />
    <Route path='/intro-react' component={IntoReact} />
  </Layout>
);
