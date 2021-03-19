import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import { Calendar } from './components/Calendar';
import { Login } from './components/Login';
import cookie from 'react-cookies'
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

    render() {
        return (
            <Layout onNavLogout={() => this.authChangeHandler(null)} user={this.state.user}>
                <Route exact path='/' render={(props) => (
                    <Calendar {...props} user={this.state.user} />
                )} />

                <Route exact path='/login' render={(props) => (
                    <Login {...props} user={this.state.user} onAuthChange={this.authChangeHandler.bind(this)} />
                )} />
            </Layout>
        );
    }
}
