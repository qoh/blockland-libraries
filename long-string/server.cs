// Example (sending as the server):
//     %longA = %client.sendLongString("this is my very long string");
//     %longB = %client.sendLongString("this thing is not as long");
//     commandToClient(%client, 'MyFirstCommand', %longA, %longB, "whatever");
//
// Example (receiving as the server):
//     function serverCmdMySecondCommand(%client, %theData, %otherStuff) {
//         %theData = %client.getLongString(%theData);
//         echo(%theData);
//     }

$Server::NextLongString = -1;

function GameConnection::sendLongString(%this, %data)
{
    %tag = ($Server::NextLongString = ($Server::NextLongString + 1) | 0);
    %len = strlen(%data);

    for (%i = 0; %i < %len; %i += 255)
    {
        commandToClient(%this, 'SendLongString', %tag, getSubStr(%data, %i, 255));
    }

    return %tag;
}

function GameConnection::getLongString(%this, %tag)
{
    return %this.longStringBuffer[%tag];
}

function serverCmdSendLongString(%client, %tag, %part)
{
    %client.longStringBuffer[%tag] = %client.longStringBuffer[%tag] @ %part;
}
