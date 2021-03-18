import React, { Component } from 'react';
import { Redirect } from "react-router-dom";
import { MonthlyCalendar } from './MonthlyCalendar';
import { WeeklyCalendar } from './WeeklyCalendar';
import axios from 'axios';
import moment from 'moment';

export class Calendar extends Component {
  static displayName = Calendar.name;

    constructor(props) {
      super(props);

        this.change = this.change.bind(this);
        this.onEventChangedHandler = this.onEventChangedHandler.bind(this);
        this.onEventDeletedHandler = this.onEventDeletedHandler.bind(this);
    }

    componentDidMount() {
        if (this.props.user !== null && this.props.user !== undefined) {
            var monday = moment().isoWeekday(1).hour(0).minute(0).second(0);
            var sunday = moment(monday).isoWeekday(7).hour(23).minute(59).second(59);
            axios.get("api/Meetings/" + monday.toISOString() + "/" + sunday.toISOString(), { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
                .then((resp) => {
                    this.setState({ meetings: resp.data });
                });
        }
    }

    state = {
        meetings: [],
        displayMonthly: "weekly",
    }

    change(event) {
        this.setState({displayMonthly: event.target.value});
    }

    onEventChangedHandler(event) {
        let meetings = this.state.meetings;
        let index = meetings.findIndex((value) => value.id === event.id);
        if (index > -1) {
            meetings[index] = event;
        }
        else {
            meetings.push(event);
        }
        this.setState({ meetings: meetings });
    }

    onEventDeletedHandler(event) {
        if (window.confirm('Are you sure you wish to delete the event "' + event.title + '"?')) {
            axios.delete("/api/Meetings/" + event.id,
                { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
                .then((resp) => {
                    this.setState({
                        meetings: this.state.meetings.filter((value, index, array) => {
                            return value.id !== event.id;
                        })
                    });
                });
        }
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
              ? <MonthlyCalendar user={this.props.user} value={this.state.meetings}>{viewSelect}</MonthlyCalendar>
              : <WeeklyCalendar user={this.props.user} onEventChanged={this.onEventChangedHandler} onEventDeleted={this.onEventDeletedHandler} value={this.state.meetings}>{viewSelect}</WeeklyCalendar>}
      </div>
    );
  }
}
