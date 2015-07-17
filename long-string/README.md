# Long String Networking

Provides methods for framing strings longer than 255 characters and sending them over the network.

### Usage

Sending two long string and arbitrary other data from the server to a client:

```csharp
%longA = %client.sendLongString("hello world abc");
%longB = %client.sendLongString("bacon spam rate");

commandToClient(%client, 'MyCommand', %longA, "hey", 3, %foo, %longB);
```

As you can see, you use a method to "prepare" a long string. It returns a unique handle which you can then use as ordinary data in your `commandToClient` call.

Then, to receive your data on the client:

```csharp
function clientCmdMyCommand(%longA, %x, %y, %z, %longB)
{
  %stringA = getLongString(%longA);
  %stringB = getLongString(%longB);
}
```

Your data can be retrieved by passing the long string handles to another function (this time on the client).

The other way around (client to server) is the same process. Call `sendLongString` with your data, then send the returned handle to the server. On the server, call `%client.getLongString` with the handle to get your data again.

### API

#### Server

##### `int GameConnection::sendLongString(string data)`

Prepares a unique handle for your data and sends it to the client. The returned handle can then be used to access `data` on the client with `getLongString`.

##### `string GameConnection::getLongString(int handle)`

Retrieves the long string data sent by a client. `handle` is the value returned by `sendLongString` on the other end.

#### Client

##### `int sendLongString(string data)`

Prepares a unique handle for your data and sends it to the server. The returned handle can then be used to access `data` on the server with `GameConnection::getLongString`.

##### `string getLongString(int handle)`

Retrieves the long string data sent by a server. `handle` is the value returned by `GameConnection::sendLongString` on the other end.
