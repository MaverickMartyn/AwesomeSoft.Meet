import React, { Component } from 'react';
import './CalendarCommon.css';
import './WeeklyCalendar.css';

export class WeeklyCalendar extends Component {
  static displayName = WeeklyCalendar.name;

    render() {
        const hours = [];
        for (let i = 0; i < 24; i++) {
            hours.push(<div class="time" style={{ gridRow: (i + 1) }}>{(i + 1).toString().padStart(2, '0')}:00</div>)
        }

        const cols = [];
        var colIndex;
        for (colIndex = 3; colIndex < 8; colIndex++) {
            cols.push(<div class="col" style={{ gridColumn: colIndex }}></div>)
        }
        cols.push(<div class="col weekend" style={{ gridColumn: colIndex }}></div>)
        colIndex++;
        cols.push(<div class="col weekend" style={{ gridColumn: colIndex }}></div>)

        const rows = [];
        for (let i = 1; i < 24; i++) {
            rows.push(<div class="row" style={{ gridRow: i }}></div>)
        }

        const dummyEvents = [
            <div class="event calendar1" style={{ gridColumn: 5, gridRow: "10/span 6", marginTop: "calc(3rem / 4)", marginBottom: "calc(-3rem / 4)" }}>Event 1</div>,
            <div class="event event2 calendar2">Event 2</div>,
            <div class="event event3 calendar2">Event 3</div>,
            <div class="event event4 calendar1">Event 4</div>
        ];

        return (
        <div>
            <div class="calendar_container">
                <div class="title">{this.props.children} February 2019 Week 6</div>
                <div class="days">
                        <div class="filler"></div>
                        <div class="filler"></div>
                        <div class="day">Mon 4</div>
                        <div class="day">Tue 5</div>
                        <div class="day">Wed 6</div>
                        <div class="day">Thu 7</div>
                        <div class="day current">Fri 8</div>
                        <div class="day">Sat 9</div>
                        <div class="day">Sun 10</div>
                </div>

                <div class="content">
                    {hours}
                    <div class="filler-col"></div>
                    {cols}
                    {rows}
                    {dummyEvents}
                    <div class="current-time">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
        </div>
    );
  }
}
