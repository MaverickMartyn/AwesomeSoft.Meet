import React, { Component } from 'react';
import './CalendarCommon.css';

export class MonthlyCalendar extends Component {
    static displayName = MonthlyCalendar.name;

    render() {
        return (
            <div class="calendar_container">
                <div class="title">{this.props.children} February 2019</div>
            </div>
        );
    }
}
