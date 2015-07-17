// Example (sending as the client):
//     %long = sendLongString("this text is not even that long");
//     commandToClient(%client, 'MySecondCommand', %long, 0.5326);
//
// Example (receiving as the client):
//     function clientCmdMyFirstCommand(%longA, %longB, %whatever) {
//         %longA = getLongString(%longA);
//         %longB = getLongString(%longB);
//         echo("got some whatever: " @ %whatever);
//         echo("and a few long strings" NL %longA NL %longB);
//     }

$Client::NextLongString = -1;

function sendLongString(%data)
{
    %tag = ($Client::NextLongString = ($Client::NextLongString + 1) | 0);
    %len = strlen(%data);

    for (%i = 0; %i < %len; %i += 255)
    {
        commandToServer('SendLongString', %tag, getSubStr(%data, %i, 255));
    }

    return %tag;
}

function getLongString(%tag)
{
    return ServerConnection.longStringBuffer[%tag];
}

function clientCmdSendLongString(%tag, %part)
{
    ServerConnection.longStringBuffer[%tag] =
        ServerConnection.longStringBuffer[%tag] @ %part;
}
