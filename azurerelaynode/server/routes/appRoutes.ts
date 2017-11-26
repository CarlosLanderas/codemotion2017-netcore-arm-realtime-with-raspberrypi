import * as express from 'express';
import relayRouter from './relayRouter';

let routes: Array<express.RequestHandler> = [
    relayRouter
];

export { routes };
