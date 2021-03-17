import React, { Component } from 'react';
import moment from 'moment';
import './CalendarCommon.css';
import './WeeklyCalendar.css';

export class WeeklyCalendar extends Component {
  static displayName = WeeklyCalendar.name;

    render() {
        const days = [];
        for (let i = 1; i < 8; i++) {
            days.push(<div key={i} className={"day" + ((moment().isoWeekday() === i) ? " current" : "")}>{moment().isoWeekday(i).format("ddd D")}</div>)
        }

        const hours = [];
        for (let i = 0; i < 24; i++) {
            hours.push(<div className="time" key={i} style={{ gridRow: (i + 1) }}>{(i + 1).toString().padStart(2, '0')}:00</div>)
        }

        const cols = [];
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

        const events = [];
        for (let i = 0; i < this.props.value.length; i++) {
            let event = this.props.value[i];
            let start = moment(event.startTime);
            let end = moment(event.endTime);
            let timeSpan = (end.hour() - start.hour());
            events.push(
                <div key={i}
                    className="event calendar1"
                    style={{ gridColumn: start.isoWeekday() + 2, gridRow: ((start.hour() + 1) + "/span " + timeSpan) }}>
                    <span className="title">{event.title}</span>
                    <p>Room: {event.room.name}</p>
                    <p>{event.description}</p>
                </div>)
        }

        //const dummyEvents = [
        //    <div key={1} className="event calendar1" style={{ gridColumn: 5, gridRow: "10/span 6", marginTop: "calc(3rem / 4)", marginBottom: "calc(-3rem / 4)" }}>Event 1</div>,
        //    <div key={2} className="event event2 calendar2">Event 2</div>,
        //    <div key={3} className="event event3 calendar2">Event 3</div>,
        //    <div key={4} className="event event4 calendar1">Event 4</div>
        //];

        return (
        <div>
            <div className="calendar_container">
                <div className="title">{this.props.children} { moment().format("MMMM YYYY [Week] w") }</div>
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
                    {/*dummyEvents*/}
                    {events}
                    <div className="current-time" style={{ gridColumn: moment().isoWeekday()+2, gridRow: moment().hour()+1 }}>
                        <div className="circle"></div>
                    </div>
                </div>
            </div>
        </div>
    );
  }
}
