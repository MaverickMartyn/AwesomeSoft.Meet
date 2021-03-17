import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input, Row, Col } from 'reactstrap';
import axios from 'axios';

export class Login extends Component {
    constructor(props) {
        super(props);

        this.loginHandler = this.loginHandler.bind(this);
        this.handleUsernameChange = this.handleUsernameChange.bind(this);
        this.handlePasswordChange = this.handlePasswordChange.bind(this);
    }

    state = {
        username: "",
        password: ""
    }

    loginHandler() {
        axios.post("api/Users/Authenticate", this.state).then((resp) => {
            this.props.onAuthChange(resp.data)
            this.props.history.push("/")
        })
    }

    handleUsernameChange(event) {
        this.setState({ username: event.target.value });
    }

    handlePasswordChange(event) {
        this.setState({ password: event.target.value });
    }

    render() {
        return (
            <Row>
                <Col md={{ size: '6', offset: 3 }}>
                    <Form>
                        <FormGroup>
                            <Label for="username">Email</Label>
                            <Input type="text" name="username" value={this.state.username} onChange={this.handleUsernameChange} id="username" placeholder="Username" />
                        </FormGroup>
                        <FormGroup>
                            <Label for="password">Password</Label>
                            <Input type="password" name="password" value={this.state.password} onChange={this.handlePasswordChange} id="password" placeholder="Password" />
                        </FormGroup>
                        <Button onClick={this.loginHandler}>
                            Login
                        </Button>
                    </Form>
                </Col>
            </Row>
        );
    }
}