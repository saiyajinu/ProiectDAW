"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();
connection.start();
connection.on("ReceiveMessageHandler", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${msg}`;
});