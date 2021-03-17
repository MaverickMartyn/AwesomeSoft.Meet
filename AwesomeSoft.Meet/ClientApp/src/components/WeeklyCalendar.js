import React, { Component } from 'react';
import './CalendarCommon.css';
import './WeeklyCalendar.css';

export class WeeklyCalendar extends Component {
  static displayName = WeeklyCalendar.name;

    render() {
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

        const dummyEvents = [
            <div key={1} className="event calendar1" style={{ gridColumn: 5, gridRow: "10/span 6", marginTop: "calc(3rem / 4)", marginBottom: "calc(-3rem / 4)" }}>Event 1</div>,
            <div key={2} className="event event2 calendar2">Event 2</div>,
            <div key={3} className="event event3 calendar2">Event 3</div>,
            <div key={4} className="event event4 calendar1">Event 4</div>
        ];

        return (
        <div>
            <div className="calendar_container">
                <div className="title">{this.props.children} February 2019 Week 6</div>
                <div className="days">
                        <div className="filler"></div>
                        <div className="filler"></div>
                        <div className="day">Mon 4</div>
                        <div className="day">Tue 5</div>
                        <div className="day">Wed 6</div>
                        <div className="day">Thu 7</div>
                        <div className="day current">Fri 8</div>
                        <div className="day">Sat 9</div>
                        <div className="day">Sun 10</div>
                </div>

                <div className="content">
                    {hours}
                    <div className="filler-col"></div>
                    {cols}
                    {rows}
                    {dummyEvents}
                    <div className="current-time">
                        <div className="circle"></div>
                    </div>
                </div>
            </div>
        </div>
    );
  }
}
