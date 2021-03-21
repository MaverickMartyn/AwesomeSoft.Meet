import React, { Component } from 'react';
import moment from 'moment';
import { Button } from 'reactstrap';
import { AddEditEventModal } from './AddEditEventModal';
import { BsFillTrashFill, BsPencilSquare } from "react-icons/bs";
import './CalendarCommon.css';
import './WeeklyCalendar.css';

export class WeeklyCalendar extends Component {
    static displayName = WeeklyCalendar.name;

    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            eventToEdit: null
        };

        this.toggleModal = this.toggleModal.bind(this);
        this.eventModalChangedHandler = this.eventModalChangedHandler.bind(this);
    }

    toggleModal() {
        this.setState({ modal: !this.state.modal }, () => {
            if (!this.state.modal) {
                this.clearEventToEdit();
            }
        })
    }

    editEvent(event) {
        this.setState({ eventToEdit: event });
        this.toggleModal();
    }

    eventModalChangedHandler(event) {
        this.props.onEventChanged(event)
    }

    clearEventToEdit() {
        this.setState({ eventToEdit: null });
    }

    render() {
        let days = [];
        for (let i = 1; i < 8; i++) {
            days.push(<div key={i} className={"day" + ((moment().isSame(moment(this.props.startDate).isoWeekday(i), 'day')) ? " current" : "")}>
                {this.props.startDate && moment(this.props.startDate).isoWeekday(i).format("ddd D")}
            </div>)
        }

        let hours = [];
        for (let i = 0; i < 24; i++) {
            hours.push(<div className="time" key={i} style={{ gridRow: (i + 1) }}>{(i + 1).toString().padStart(2, '0')}:00</div>)
        }

        let cols = [];
        var colIndex;
        for (colIndex = 3; colIndex < 8; colIndex++) {
            cols.push(<div key={colIndex} className="col" style={{ gridColumn: colIndex }}></div>)
        }
        cols.push(<div key={colIndex} className="col weekend" style={{ gridColumn: colIndex }}></div>)
        colIndex++;
        cols.push(<div key={colIndex} className="col weekend" style={{ gridColumn: colIndex }}></div>)

        const rows = [];
        for (let i = 1; i < 24; i++) {
            rows.push(<div key={i} className="row" style={{ gridRow: i }}></div>)
        }

        let events = [];
        for (let i = 0; i < this.props.value.length; i++) {
            let event = this.props.value[i];
            let start = moment(event.startTime);
            let end = moment(event.endTime);
            let timeSpan = (end.hour() - start.hour());
            events.push(
                <div key={i}
                    className={'event calendar' + ((event.conflictingIds && event.conflictingIds.length > 0) ? ' conflict' : '')}
                    style={{
                        gridColumn: start.isoWeekday() + 2,
                        gridRow: ((start.hour() + 1) + "/span " + timeSpan),
                        width: ((event.conflictingIds && event.conflictingIds.length > 0) ? 100 / (event.conflictingIds.length + 1) + '%' : '100%')
                    }}>
                    <span className="title">{event.title}</span><span className="buttons"><BsPencilSquare onClick={() => this.editEvent(event)} /><BsFillTrashFill onClick={() => this.props.onEventDeleted(event)} /></span>
                    <p>Room: {event.room && event.room.name}</p>
                    <p>{event.description}</p>
                </div>)
        }

        let currentTimeBar = null;
        if (moment().isSame(this.props.startDate, 'isoWeek')) {
            currentTimeBar = <div className="current-time" style={{ gridColumn: moment().isoWeekday() + 2, gridRow: moment().hour() + 1 }}>
                <div className="circle"></div>
            </div>
        }

        return (
            <div>
                <div className="calendar_container">
                    <div className="title">
                        <div className="side_options d-flex">
                            <Button className="mr-2" onClick={this.toggleModal}>New event</Button>
                            {this.props.children}
                        </div>
                        <div>
                            <Button className="mr-2" onClick={() => this.props.onGo('back')}>Previous week</Button>
                            {this.props.startDate && this.props.startDate.format("MMMM YYYY [Week] w")}
                            <Button className="ml-2" onClick={() => this.props.onGo('forward')}>Next week</Button>
                        </div>
                    </div>
                    <div className="days">
                        <div className="filler"></div>
                        <div className="filler"></div>
                        {days}
                    </div>

                    <div className="content">
                        {hours}
                        <div className="filler-col"></div>
                        {cols}
                        {rows}
                        {events}
                        {currentTimeBar}
                    </div>
                </div>
                <AddEditEventModal eventToEdit={this.state.eventToEdit} onChange={this.eventModalChangedHandler} show={this.state.modal} user={this.props.user} onToggleChange={this.toggleModal} />
            </div>
        );
    }
}
