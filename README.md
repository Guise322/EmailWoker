# EmailWorker
This project is a service worker that allows users to work with emails on the background.

## Motivation
I came up with this idea when I needed a pet project and some automation to work with emails. I also discovered there were no obvious way to work with
a big number of emails by means of the email services.

## Built With
-.NET 6.0
-MimeKit
-ardalis/GuardClauses

## Features
Below given a template of work tasks for the service. At the time, it works with Yandex and Gmail boxes.

### Reading All The Unread Messages In An Email Box
In this task the service marks as seen all the unread messages in a given email box. It marks up to 1000 messages at a time. The next 1000 will be marked
in 5 minuts and so on until the end. The first block of the template shows the way a task is wrote.

### Searching An Email Box For A Request Message And Sending The PC's Current IP In Response
In this task the service searches a given email box for a request message. If a request is searched, it sends the PC's current IP in response. The service
searches an unread message from the email given in the property \_searchedEmail of PublicIPGetterService.cs of the source code.The second block of the
template shows the way a task is wrote.

### JSON Template For A Work Task
The json file should be placed in the same directory as the executable file.
```json
[
    {
        "MailServer" : "imap.yandex.com",
        "Port" : 993,
        "Ssl" : true,
        "Login" : "yourEmail@ya.ru",
        "Password" : "yourPassword",
        "DedicatedWork" : "MarkAsSeen"
    },
    {
        "MailServer" : "imap.gmail.com",
        "Port" : 993,
        "Ssl" : true,
        "Login" : "yourEmail@gmail.com",
        "Password" : "yourPassword",
        "DedicatedWork" : "SearchRequest"
    }
]
```
## Installing
Install the service as a regular service worker for your OS. Executables are stored in the bin folder of the project.
