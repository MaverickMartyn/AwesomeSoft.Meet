import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import { Calendar } from './components/Calendar';
import { Login } from './components/Login';
import cookie from 'react-cookies'
//import { FetchData } from './components/FetchData';
//import { Counter } from './components/Counter';
//import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
//import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
//import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  state = {
    user: cookie.load("user")
  }

  authChangeHandler(user) {
    this.setState({ user: user });
    cookie.save("user", user)
  }

  render () {
    return (
        <Layout onNavLogout={() => this.authChangeHandler(null)} user={this.state.user}>
            <Route exact path='/' render={(props) => (
                <Calendar {...props} user={this.state.user} />
            )} />

            <Route exact path='/login' render={(props) => (
                <Login {...props} user={this.state.user} onAuthChange={this.authChangeHandler.bind(this)} />
            )} />
        {/*<Route path='/counter' component={Counter} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />*/}
      </Layout>
    );
  }
}
