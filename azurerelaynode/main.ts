import * as  express from 'express';
import { ExpressServer } from './server/expressServer';

let expressApplication = express();
let port = process.env.PORT || 3000;
let expressServer = new ExpressServer(expressApplication, port as number);
expressServer.run();
