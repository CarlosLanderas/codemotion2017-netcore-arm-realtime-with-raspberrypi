import * as WebSocketRelay from 'hyco-ws';
import { AppSettings } from '../../../../settings';

let autoIncrementId = 0;
let wss = WebSocketRelay.createRelayedServer(
    {
        server: WebSocketRelay.createRelayListenUri(AppSettings.NAMESPACE, AppSettings.PATH),
        token: WebSocketRelay.createRelayToken(
            `http://${AppSettings.NAMESPACE}`,
            AppSettings.KEYRULE,
            AppSettings.KEY)
    },
    (ws) => {
        (ws as any).id = autoIncrementId++;
        console.log('New connection accepted');
        ws.onmessage = function (event) {
            let wsMessage = event.data.toString();
            for (let i = 0; i < wss.clients.length; i++) {
                let client = wss.clients[i];
                if (client.readyState === client.OPEN
                    && (ws as any).id !== (client as any).id) {
                    client.send(wsMessage);
                }
            }
        };
        ws.on('close', function (code) {
            console.log(`connection closed: ${code}`);
        });
    });

console.log('Relay server listening');

wss.on('error', function (err) {
    console.log('error' + err);
});
