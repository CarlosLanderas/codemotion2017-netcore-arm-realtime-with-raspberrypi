import React from "react";


const orderBusTimes = (busTimes) => {
    return busTimes.sort((firstTime, nextTime) => {
        return firstTime.busTimeLeft - nextTime.busTimeLeft;
    }).slice(0, 20);
}

const getTimeDescription = (timeLeft) => {
    return timeLeft === 0 ? "En parada" : `${timeLeft} segs`;
};

const NextBuses = (props) => {
    return (
        <div className="next-buses">
            <h2>Próximos buses</h2>
            <ul>
                {orderBusTimes(props.buses).map((bus, index) => {
                    return <li key={index}>{`BusId: ${bus.busId}`}<span>{` - ${getTimeDescription(bus.busTimeLeft)}`}</span></li>
                })}
            </ul>
        </div>
    );
};

export { NextBuses };