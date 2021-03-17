import React, { Component } from 'react';
import { Redirect } from "react-router-dom";
import { MonthlyCalendar } from './MonthlyCalendar';
import { WeeklyCalendar } from './WeeklyCalendar';

export class Calendar extends Component {
  static displayName = Calendar.name;

    constructor(props) {
      super(props);

        this.change = this.change.bind(this);
    }

    state = {
        meetings: [],
        displayMonthly: "weekly",
    }

    change(event) {
        this.setState({displayMonthly: event.target.value});
    }

  render() {
    if (this.props.user === null || this.props.user === undefined) {
      return <Redirect to={"/login"} />
    }
    var viewSelect =
          <select onChange={this.change} value={this.state.displayMonthly}>
              <option value="weekly">Weekly</option>
              <option value="monthly">Monthly</option>
          </select>

    return (
        <div>
            {this.state.displayMonthly === "monthly"
              ? <MonthlyCalendar value={this.state.meetings}>{viewSelect}</MonthlyCalendar>
              : <WeeklyCalendar value={this.state.meetings}>{viewSelect}</WeeklyCalendar>}
      </div>
    );
  }
}
