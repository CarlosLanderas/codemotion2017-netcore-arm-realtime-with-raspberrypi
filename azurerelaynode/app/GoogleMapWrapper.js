import { GoogleMap, withGoogleMap, Marker } from "react-google-maps";
import React from "react";
import { BusStop } from "./BusStop";

const madridCoordinates = { lat: 40.416775, lng: -3.703790 };
export const GoogleMapWrapper = withGoogleMap((props) =>
    <GoogleMap
        defaultZoom={15}
        defaultCenter={madridCoordinates}>
        {props.routeLines.map((item, index) => {
            return <BusStop key={index} latitude={item.latitude} longitude={item.longitude} />
        })}
        {props.buses.map((item, index) => {
            return <Marker title={String(item.busTimeLeft)} icon={"images/bus.png"} key={`${index}_bus`} position={{ lat: item.latitude, lng: item.longitude }} />
        })}
    </GoogleMap>
);
