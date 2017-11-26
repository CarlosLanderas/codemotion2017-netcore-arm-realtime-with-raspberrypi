"use strict";
exports.__esModule = true;
var WebSocketRelay = require("hyco-ws");
var settings_1 = require("../../../../settings");
var autoIncrementId = 0;
var wss = WebSocketRelay.createRelayedServer({
    server: WebSocketRelay.createRelayListenUri(settings_1.AppSettings.NAMESPACE, settings_1.AppSettings.PATH),
    token: WebSocketRelay.createRelayToken("http://" + settings_1.AppSettings.NAMESPACE, settings_1.AppSettings.KEYRULE, settings_1.AppSettings.KEY)
}, function (ws) {
    ws.id = autoIncrementId++;
    console.log('New connection accepted');
    ws.onmessage = function (event) {
        var wsMessage = event.data.toString();
        for (var i = 0; i < wss.clients.length; i++) {
            var client = wss.clients[i];
            if (client.readyState === client.OPEN
                && ws.id !== client.id) {
                client.send(wsMessage);
            }
        }
    };
    ws.on('close', function (code) {
        console.log("connection closed: " + code);
    });
});
console.log('Relay server listening');
wss.on('error', function (err) {
    console.log('error' + err);
});
