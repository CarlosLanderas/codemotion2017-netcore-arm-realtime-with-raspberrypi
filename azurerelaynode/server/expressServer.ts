import * as express from 'express';
import { routes } from './routes/appRoutes';
import * as mustache from 'mustache-express';
import {AppSettings} from '../settings';


export class ExpressServer {
    constructor(private app: express.Express, private port: number) {
        this.configureRoutes(app);
        this.configureEngineTemplate(app);
        this.configureStaticFiles(app);
    }

    private configureRoutes(app: express.Express) {
        app.use(routes);
    }

    private configureEngineTemplate(app: express.Express) {
        app.engine('html', mustache());
        app.set('views', AppSettings.VIEWSFOLDER);
        app.set('view engine', 'mustache');
    }

    private configureStaticFiles(app: express.Express){
        app.use(express.static(AppSettings.STATICFOLDERS));
    }

    public run() {
        console.log(`Starting app on port ${this.port}`);
        this.app.listen(this.port);
    }
}
