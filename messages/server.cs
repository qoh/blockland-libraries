function MessageInstance::onAdd(%this)
{
    if (!isObject(%this.client))
    {
        error("ERROR: MessageInstance created without client");
        %this.delete();
        return;
    }

    commandToClient(%this.client, 'Message_Add', %this);

    %this.position = "0 0";
    %this.extent = "0 0";
    %this.margin = "0 0";
    %this.visible = 1;
    %this.alpha = 1;
    %this.background = "0 0 0 0";
    %this.lineSpacing = 2;
    %this.bitmapHeight = -1;
}

function MessageInstance::onRemove(%this)
{
    commandToClient(%this.client, 'Message_Remove', %this);
}

function MessageInstance::resize(%this, %position, %extent, %a, %b)
{
    if (%a !$= "" && %b !$= "")
    {
        %position = %position SPC %extent;
        %extent = %a SPC %b;
    }

    %this.position = %position;
    %this.extent = %extent;

    commandToClient(%this.client, 'Message_Resize', %this, %position, %extent);
}

function MessageInstance::setPosition(%this, %position)
{
    if (%position !$= %this.position)
        %this.resize(%position, %this.extent);
}

function MessageInstance::setExtent(%this, %extent)
{
    if (%extent !$= %this.extent)
        %this.resize(%this.position, %extent);
}

function MessageInstance::setMargin(%this, %margin)
{
    if (%margin !$= %this.margin)
    {
        %this.margin = %margin;
        commandToClient(%this.client, 'Message_SetMargin', %this, %margin);
    }
}

function MessageInstance::setVisible(%this, %visible)
{
    if ((!!%visible) != %this.visible)
    {
        %this.visible = !!%visible;
        commandToClient(%this.client, 'Message_SetVisible', %this, %this.visible);
    }
}

function MessageInstance::setAlpha(%this, %alpha)
{
    if (%alpha != %this.alpha)
    {
        %this.alpha = %alpha;
        commandToClient(%this.client, 'Message_SetAlpha', %this, mClampF(%alpha, 0, 1));
    }
}

function MessageInstance::setBackground(%this, %background)
{
    if (%background !$= %this.background)
    {
        %this.background = %background;
        commandToClient(%this.client, 'Message_SetBackground', %this, %background);
    }
}

function MessageInstance::setLineSpacing(%this, %lineSpacing)
{
    if (%lineSpacing != %this.lineSpacing)
    {
        %this.lineSpacing = %lineSpacing;
        commandToClient(%this.client, 'Message_SetLineSpacing', %this, %lineSpacing);
    }
}

function MessageInstance::setBitmapHeight(%this, %bitmapHeight)
{
    if (%bitmapHeight != %this.bitmapHeight)
    {
        %this.bitmapHeight = %bitmapHeight;
        commandToClient(%this.client, 'Message_SetBitmapHeight', %this, %bitmapHeight);
    }
}

function MessageInstance::setText(%this, %text)
{
    if (strcmp(%text, %this.text) == 0)
        return;

    %this.text = %text;
    %length = strlen(%text);

    commandToClient(%this.client, 'Message_SetText', %this, getSubStr(%text, 0, 255));

    for (%i = 255; %i < %length; %i += 255)
        commandToClient(%this.client, 'Message_AddText', %this, getSubStr(%text, %i, 255));
}

function MessageInstance::addText(%this, %text)
{
    %this.text = %this.text @ %text;
    commandToClient(%this.client, 'Message_AddText', %this, %text);
}

function GameConnection::createMessage(%this)
{
    if (!isObject(%this.messageGroup))
        return 0;

    %message = new ScriptObject()
    {
        class = "MessageInstance";
        client = %this;
    };

    %this.messageGroup.add(%message);
    return %message;
}

function GameConnection::updateMessages(%this)
{
}

function serverCmdClientHasMessages(%client)
{
    if (!isObject(%client.messageGroup))
    {
        %client.messageGroup = new ScriptGroup();

        if (%client.hasSpawnedOnce)
            %client.updateMessages();
    }
}

function serverCmdClientResolution(%client, %resolution)
{
    if (%resolution !$= %client.resolution && isObject(%client.messageGroup) && %client.hasSpawnedOnce)
        %client.updateMessages();

    %client.resolution = %resolution;
}

package MessageServerPackage
{
    function GameConnection::onClientEnterGame(%this)
    {
        Parent::onClientEnterGame(%this);

        if (isObject(%client.messageGroup))
            %this.updateMessages();
    }

    function GameConnection::autoAdminCheck(%this)
    {
        %this.messageGroup = new ScriptGroup();
        return Parent::autoAdminCheck(%this);
    }

    function GameConnection::onDrop(%this)
    {
        if (isObject(%this.messageGroup))
            %this.messageGroup.delete();

        Parent::onDrop(%this);
    }
};

activatePackage("MessageServerPackage");
