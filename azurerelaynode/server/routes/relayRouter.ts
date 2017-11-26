import * as express from 'express';
import { AppSettings } from '../../settings';
import * as RelaySocket from 'hyco-ws';

let relayRouter = express.Router();

let relayToken = encodeURIComponent(RelaySocket.createRelayToken(
    'http://' + AppSettings.NAMESPACE,
    AppSettings.KEYRULE,
    AppSettings.KEY));

relayRouter.get('/', (request: express.Request, response: express.Response) => {
    response.render('index.html',
        { relayToken: relayToken });
});

export default relayRouter;
