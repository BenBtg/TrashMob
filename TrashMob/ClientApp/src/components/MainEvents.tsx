import * as React from 'react'
import { useHistory } from 'react-router-dom';
import EventAttendeeData from './Models/EventAttendeeData';
import { apiConfig, getDefaultHeaders, msalClient } from '../store/AuthStore';
import { getEventType } from '../store/eventTypeHelper';
import EventData from './Models/EventData';
import EventTypeData from './Models/EventTypeData';
import UserData from './Models/UserData';

class DisplayEvent {
    id: string;
    name: string;
    eventDate: Date;
    eventTypeId: number;
    city: string;
    region: string;
    country: string;
    isAttending: string;
}

export interface MainEventsDataProps {
    eventList: EventData[];
    eventTypeList: EventTypeData[];
    myAttendanceList: EventData[];
    isDataLoaded: boolean;
    isUserLoaded: boolean;
    currentUser: UserData;
};

export const MainEvents: React.FC<MainEventsDataProps> = (props) => {
    const [displayEvents, setDisplayEvents] = React.useState([]);
    const history = useHistory();

    React.useEffect(() => {
        if (props.isDataLoaded && props.eventList && props.myAttendanceList) {
            const list = props.eventList.map((mobEvent) => {
                var dispEvent = new DisplayEvent()
                dispEvent.id = mobEvent.id;
                dispEvent.city = mobEvent.city;
                dispEvent.region = mobEvent.region;
                dispEvent.country = mobEvent.country;
                dispEvent.eventDate = mobEvent.eventDate;
                dispEvent.eventTypeId = mobEvent.eventTypeId;
                dispEvent.name = mobEvent.name;
                var isAttending = props.myAttendanceList.findIndex((e) => e.id === mobEvent.id) >= 0;
                dispEvent.isAttending = !props.isUserLoaded ? 'Log in to see your status' : (isAttending ? 'Yes' : 'No');
                return dispEvent;
            });
            setDisplayEvents(list);
        }
    }, [props.isDataLoaded, props.eventList, props.myAttendanceList, props.isUserLoaded])

    function addAttendee(eventId: string) {

        const account = msalClient.getAllAccounts()[0];

        var request = {
            scopes: apiConfig.b2cScopes,
            account: account
        };

        msalClient.acquireTokenSilent(request).then(tokenResponse => {

            var eventAttendee = new EventAttendeeData();
            eventAttendee.userId = props.currentUser.id;
            eventAttendee.eventId = eventId;

            var data = JSON.stringify(eventAttendee);

            const headers = getDefaultHeaders('POST');
            headers.append('Authorization', 'BEARER ' + tokenResponse.accessToken);

            // POST request for Add EventAttendee.  
            fetch('api/EventAttendees', {
                method: 'POST',
                body: data,
                headers: headers,
            }).then((response) => response.json())
        })
    }

    function handleAttend(eventId: string) {

        var accounts = msalClient.getAllAccounts();

        if (accounts === null || accounts.length === 0) {
            msalClient.loginRedirect().then(() => {
                addAttendee(eventId);
            })
        }
        else {
            addAttendee(eventId);
        }
    }

    function renderEventsTable(events: DisplayEvent[]) {
        return (
            <div>
                <table className='table table-striped' aria-labelledby="tabelLabel" width='100%'>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Date</th>
                            <th>Event Type</th>
                            <th>City</th>
                            <th>Region</th>
                            <th>Country</th>
                            <th>Am I Attending?</th>
                            <th />
                        </tr>
                    </thead>
                    <tbody>
                        {events.map(mobEvent =>
                            <tr key={mobEvent.id.toString()}>
                                <td>{mobEvent.name}</td>
                                <td>{new Date(mobEvent.eventDate).toLocaleString()}</td>
                                <td>{getEventType(props.eventTypeList, mobEvent.eventTypeId)}</td>
                                <td>{mobEvent.city}</td>
                                <td>{mobEvent.region}</td>
                                <td>{mobEvent.country}</td>
                                <td>{mobEvent.isAttending}</td>
                                <td>
                                    <button className="action" onClick={() => history.push('/eventdetails/' + mobEvent.id)}>View Details</button>
                                    <button className="action" onClick={() => handleAttend(mobEvent.id)}>Register to Attend Event</button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    }

    return (
        <>
            <div>
                {!props.isDataLoaded && <p><em>Loading...</em></p>}
                {props.isDataLoaded && renderEventsTable(displayEvents)}
            </div>
        </>
    );
}