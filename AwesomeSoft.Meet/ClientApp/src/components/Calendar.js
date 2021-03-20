import React, { Component } from 'react';
import { Redirect } from "react-router-dom";
import { MonthlyCalendar } from './MonthlyCalendar';
import { WeeklyCalendar } from './WeeklyCalendar';
import axios from 'axios';
import moment from 'moment';
import { Input } from 'reactstrap';

export class Calendar extends Component {
    static displayName = Calendar.name;

    constructor(props) {
        super(props);

        this.change = this.change.bind(this);
        this.onEventChangedHandler = this.onEventChangedHandler.bind(this);
        this.onEventDeletedHandler = this.onEventDeletedHandler.bind(this);
        this.goHandler = this.goHandler.bind(this);
        let dates = this.getDates(moment());
        this.state.startDate = dates.startDate;
        this.state.endDate = dates.endDate;
    }

    componentDidMount() {
        if (this.props.user !== null && this.props.user !== undefined) {
            this.populateCalendar();
        }
    }

    populateCalendar() {
        axios.get("api/Meetings/" + this.state.startDate.toISOString() + "/" + this.state.endDate.toISOString(), { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
            .then((resp) => {
                this.setState({ meetings: resp.data });
            });
    }

    getDates(startDate) {
        if (this.state.display === "weekly") {
            let date1 = moment(startDate).isoWeekday(1).hour(0).minute(0).second(0);
            let date2 = moment(date1).isoWeekday(7).hour(23).minute(59).second(59);
            return {
                startDate: date1,
                endDate: date2
            };
        }
        else {
            let date1 = moment(startDate).date(1).hour(0).minute(0).second(0);
            let date2 = moment(date1).add(1, 'M').subtract(1, 'd').hour(23).minute(59).second(59);
            return {
                startDate: date1,
                endDate: date2
            };
        }
    }

    goHandler(direction) {
        let action = (direction === 'back') ? 'subtract' : 'add';
        let startMoment = moment(this.state.startDate);
        if (this.state.display === 'weekly') {
            startMoment[action](1, 'w');
        }
        else {
            startMoment[action](1, 'M');
        }
        this.setState(this.getDates(startMoment));
        this.populateCalendar();
    }

    state = {
        meetings: [],
        display: "weekly"
    }

    change(event) {
        this.setState({ display: event.target.value });
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
            <Input type="select" onChange={this.change} value={this.state.display}>
                <option value="weekly">Weekly</option>
                <option value="monthly">Monthly</option>
            </Input>

        return (
            <div>
                {this.state.display === "monthly"
                    ? <MonthlyCalendar user={this.props.user} value={this.state.meetings}>{viewSelect}</MonthlyCalendar>
                    : <WeeklyCalendar
                        user={this.props.user}
                        onGo={this.goHandler}
                        onEventChanged={this.onEventChangedHandler}
                        onEventDeleted={this.onEventDeletedHandler}
                        startDate={this.state.startDate}
                        value={this.state.meetings}>{viewSelect}</WeeklyCalendar>}
            </div>
        );
    }
}
