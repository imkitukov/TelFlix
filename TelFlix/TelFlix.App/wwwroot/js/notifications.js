/// <reference path="../lib/signalr/dist/browser/signalr.js" />

$(() => {
    console.log('Hello')

    let baseUrl = 'http://localhost:50183/'

    let connectionBuilder = new signalR
        .HubConnectionBuilder()
        .withUrl(baseUrl + 'notifications')

    let connection = connectionBuilder.build()

    connection
        .start()
        .then(() => {
            connection.invoke('SendMessage', 'Robert', 'Welcome to SignalR!')
            connection.on("getMessage", (username, content) => {
                console.log('Message from: ' + username)
                console.log('Content: ' + content)
            })
        })
})