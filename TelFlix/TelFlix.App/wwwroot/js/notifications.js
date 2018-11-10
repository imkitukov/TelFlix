/// <reference path="../lib/signalr/dist/browser/signalr.js" />

$(() => {
    console.log('Hello')

    const baseUrl = 'http://localhost:50183/'

    const connectionBuilder = new signalR
        .HubConnectionBuilder()
        .withUrl(baseUrl + 'notifications')

    const connection = connectionBuilder.build()
    connection
        .start()
        .then(() => {
            let count = document.getElementById('messages').innerHTML

            connection
                .on('pushNotification', () => {
                    document
                        .getElementById('messages')
                        .innerHTML = `${++count}`
                })

            $('#sendMessageBtn').click(() => {
                let receiver = $('#receiver').val()
                let subject = $('#subject').val()
                let content = $('#content').val()

                connection
                    .invoke('SendMessage', receiver, subject, content)

                document.getElementById('input-form').reset()
                $('#newMessageModal').modal('toggle')

                let newRow = document.createElement('tr')

                let col1 = document.createElement('td')
                let col1text = document.createTextNode('The Admin')
                col1.appendChild(col1text)
                newRow.appendChild(col1)

                let col2 = document.createElement('td')
                let col2text = document.createTextNode('I want a movie')
                col2.appendChild(col2text)
                newRow.appendChild(col2)

                let col3 = document.createElement('td')
                let col3text = document.createTextNode('This one --> The Matrix')
                col3.appendChild(col3text)
                newRow.appendChild(col3)

                let col4 = document.createElement('td')
                let col4text = document.createTextNode('Now')
                col4.appendChild(col4text)
                newRow.appendChild(col4)

                document.getElementById('current-user-messages').appendChild(newRow)
            })
        })
})

