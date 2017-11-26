import React, { Component } from "react";
import { Marker } from "react-google-maps";
import { GoogleMapWrapper } from "./GoogleMapWrapper"

export class Map extends Component {

    constructor(props) {
        super(props);
    }
    render() {

        return (
            <div>
                <GoogleMapWrapper
                    routeLines={this.props.lines}
                    buses={this.props.buses}
                    containerElement={<div style={{ height: `600px` }} />}
                    mapElement={<div style={{ height: `100%` }} />}
                />
            </div>
        )
    }
}
