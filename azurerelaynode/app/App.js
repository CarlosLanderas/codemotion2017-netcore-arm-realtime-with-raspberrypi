import React, { Component } from "react";
import { Map } from "./Map";
import { NextBuses } from "./NextBuses";

export class App extends Component {

    constructor(props) {
        super(props);
        this.state = { lines: [], buses: [] };
        this.ws = new WebSocket(`wss://codemotionrelay.servicebus.windows.net:443/$hc/relay01?sb-hc-action=connect&sb-hc-token=${relayToken}`);
    }
    componentWillMount() {
        this.loadLinesAsync().then((resultLines) => {

            this.setState({ lines: resultLines.resultValues });

            this.ws.onmessage = (event) => {
                this.mapBusesChunk(JSON.parse(event.data)).then(mapInfo => {
                    this.setState({ buses: mapInfo });
                });
            }
        });
    }

    mapBusesChunk(stops) {
        return new Promise((resolve, reject) => {
            let buses = this.state.buses;
            for (var stop of stops) {
                let bus = buses.find(s => s.busId === stop.busId);
                if (bus) {
                    let busIndex = buses.indexOf(bus);
                    buses[busIndex] = stop;
                } else {
                    buses.push(stop);
                }
            }
            resolve([...buses]);
        });
    }
    loadLinesAsync() {
        return fetch('./routeLines.json')
            .then((res) => res.json());
    }

    render() {
        return (
            <div>
                <div className="header">
                    <h1 className="title">Autobuses en tiempo real
                        <span className="highlight"> Paradas línea 27</span>
                    </h1>
                    <img src="images/logo_white.png" className="logo" />
                </div>
                <NextBuses buses={this.state.buses}></NextBuses>
                <Map className="map" lines={this.state.lines}
                    buses={this.state.buses} />
            </div>);
    }
}
