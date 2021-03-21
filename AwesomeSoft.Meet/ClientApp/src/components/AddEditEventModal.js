import React, { Component } from 'react';
import moment from 'moment';
import {
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Form,
    FormGroup,
    Label,
    Input,
} from 'reactstrap';
import DatePicker from 'react-datepicker';
import axios from 'axios';

export class AddEditEventModal extends Component {
    static displayName = AddEditEventModal.name;

    constructor(props) {
        super(props);

        this.toggle = this.toggle.bind(this);
        this.updateField = this.updateField.bind(this);
        this.saveEvent = this.saveEvent.bind(this);
        this.updateParticipantIdsField = this.updateParticipantIdsField.bind(this);
    }

    componentDidMount() {
        this.getUsers();
        this.getRooms();
    }

    componentDidUpdate(prevProps) {
        if (prevProps.eventToEdit !== this.props.eventToEdit && !!this.props.eventToEdit) {
            this.getUsers();
            let event = this.props.eventToEdit;
            event.participantIds = event.participants.map((value) => value.id.toString());
            event.roomId = event.room && event.room.id;
            this.setState(this.props.eventToEdit, () => {
                this.getRooms();
            });
        }
        else if (!this.props.eventToEdit && this.state.id > 0) {
            this.setState(this.emptyEvent);
        }
    }

    emptyEvent = {
        id: 0,
        title: "",
        description: "",
        startTime: moment().toISOString(),
        endTime: moment().add(1, 'h').toISOString(),
        participantIds: []
    }

    state = this.emptyEvent;

    saveEvent() {
        if (this.state.id > 0) {
            axios.put("/api/Meetings/" + this.state.id, {
                id: this.state.id,
                title: this.state.title,
                description: this.state.description,
                startTime: this.state.startTime,
                endTime: this.state.endTime,
                roomId: this.state.roomId,
                participantIds: this.state.participantIds,
            },
                { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
                .then((resp) => {
                    this.setState(this.emptyEvent);
                    this.props.onChange(resp.data);
                    this.toggle();
                });
        }
        else {
            axios.post("/api/Meetings/", {
                title: this.state.title,
                description: this.state.description,
                startTime: this.state.startTime,
                endTime: this.state.endTime,
                roomId: this.state.roomId,
                participantIds: this.state.participantIds,
            },
                { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
                .then((resp) => {
                    this.setState(this.emptyEvent);
                    this.props.onChange(resp.data);
                    this.toggle();
                });
        }
    }

    getUsers() {
        let user = this.props.user;
        axios.get("/api/Users",
            { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
            .then((resp) => {
                this.setState({
                    availableParticipants: resp.data.filter(function (value, index, arr) {
                        return value.id !== user.id;
                    })
                });
            });
    }

    getRooms() {
        axios.get("/api/Room/" + moment(this.state.startTime).toISOString() + "/"
            + moment(this.state.endTime).toISOString()
            + ((this.state.id > 0) ? '/' + this.state.id : ''),
            { headers: { 'Authorization': 'Bearer ' + this.props.user.token } })
            .then((resp) => {
                this.setState({ availableRooms: resp.data });
            });
    }

    updateDate(date, dateProp) {
        let event = this.state;
        event[dateProp] = date.toISOString();
        this.setState(event)
        this.getRooms();
    }

    updateField(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    updateParticipantIdsField(event) {
        var list = this.state.participantIds;
        if (event.target.checked && list.indexOf(event.target.value) < 0) {
            list.push(event.target.value);
        }
        if (!event.target.checked) {
            list = list.filter(function (value, index, arr) {
                return value !== event.target.value;
            });
        }
        this.setState({ participantIds: list });
    }

    toggle() {
        this.props.onToggleChange(!this.props.show);
    }

    render() {
        var roomOptions = this.state.availableRooms && this.state.availableRooms.map((r) => <option key={r.id} value={r.id}>{r.name}</option>);

        var participantOptions = this.state.availableParticipants && this.state.availableParticipants.map((r) => (<div key={r.id}><input onChange={this.updateParticipantIdsField} type="checkbox" defaultChecked={this.state.participantIds.indexOf(r.id.toString()) > -1} value={r.id} /> {r.name}</div>));

        return (
            <Modal isOpen={this.props.show} toggle={this.toggle}>
                <ModalHeader toggle={this.toggle}>{this.state.id > 0 ? "Edit" : "Add"} event</ModalHeader>
                <ModalBody>
                    <Form>
                        <FormGroup>
                            <Label for="title">Title</Label>
                            <Input type="text" name="title" value={this.state.title} onChange={this.updateField} id="title" />
                        </FormGroup>
                        <FormGroup>
                            <Label for="description">Description</Label>
                            <Input type="textarea" name="description" value={this.state.description} onChange={this.updateField} id="description" />
                        </FormGroup>
                        <FormGroup>
                            <Label for="start-time">Start time</Label><br />
                            <DatePicker
                                name="start-time"
                                selected={moment(this.state.startTime).toDate()}
                                onChange={date => this.updateDate(date, "startTime")}
                                selectsStart
                                startDate={moment(this.state.startTime).toDate()}
                                endDate={moment(this.state.endTime).toDate()}
                                showTimeSelect
                                dateFormat="MMMM d, yyyy h:mm aa"
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="end-time">End time</Label><br />
                            <DatePicker
                                name="end-time"
                                selected={moment(this.state.endTime).toDate()}
                                onChange={date => this.updateDate(date, "endTime")}
                                selectsEnd
                                startDate={moment(this.state.startTime).toDate()}
                                endDate={moment(this.state.endTime).toDate()}
                                minDate={moment(this.state.startTime).add(1, 'h').toDate()}
                                showTimeSelect
                                dateFormat="MMMM d, yyyy h:mm aa"
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="roomId">Room</Label><br />
                            <select name="roomId" value={this.state.roomId} onChange={this.updateField}>
                                <option value="0">Select a room</option>
                                {roomOptions}
                            </select>
                        </FormGroup>
                        <FormGroup>
                            <div className="participants-scroller">
                                <Label for="participants">Participants</Label><br />
                                {participantOptions}
                            </div>
                        </FormGroup>
                    </Form>
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={this.saveEvent}>Save</Button>{' '}
                    <Button color="secondary" onClick={() => { this.toggle(); this.setState(this.emptyEvent) }}>Cancel</Button>
                </ModalFooter>
            </Modal>)
    }
}