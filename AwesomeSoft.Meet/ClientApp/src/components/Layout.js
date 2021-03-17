import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { withRouter } from 'react-router'

class Layout extends Component {
  static displayName = Layout.name;

    constructor(props) {
        super(props);

        this.props.history.listen((location) => {
            this.setState({
                useFluidContainer: location.pathname === "/"
            })
        })
    }
    state = {
        useFluidContainer: this.props.history.location.pathname === "/"
    }

  render () {
    var containerType = 
    <Container>
      {this.props.children}
    </Container>
      if (this.state.useFluidContainer) {
      containerType = <Container fluid>
        {this.props.children}
      </Container>
    }
    return (
      <div>
        <NavMenu onLogout={this.props.onNavLogout} user={this.props.user} />
        {containerType}
      </div>
    );
  }
}
export default withRouter(Layout)