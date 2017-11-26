import React, { Component } from "react";
import { Marker } from "react-google-maps";

export class BusStop extends Component {

    constructor(props) {
        super(props);
    }
    render() {

        return (
            <Marker
                icon={"images/bus_stop.png"}
                position={{ lat: this.props.latitude, lng: this.props.longitude }}>                
            </Marker>
        );
    }
}
