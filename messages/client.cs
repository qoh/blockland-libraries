if (!isObject(MessageOverlay))
{
    new GuiControl(MessageOverlay)
    {
        profile = GuiDefaultProfile;
    };

    PlayGUI.add(MessageOverlay);
}

function clientCmdMessage_Add(%id)
{
    if (isObject(MessageOverlay.message[%id]))
        MessageOverlay.message[%id].delete();

    %swatch = new GuiSwatchCtrl()
    {
        profile = GuiDefaultProfile;

        minExtent = "0 0";
        extent = "0 0";
        color = "0 0 0 0";

        id = %id;
        margin = "0 0";

        new GuiMLTextCtrl()
        {
            profile = BlockChatTextSize4Profile;

            minExtent = "0 0";
            extent = "0 0";

            allowColorChars = 1;
        };
    };

    %swatch.text = %swatch.getObject(0);

    MessageOverlay.add(%swatch);
    MessageOverlay.message[%id] = %swatch;
}

function clientCmdMessage_Remove(%id)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
    {
        %swatch.delete();
        MessageOverlay.message[%id] = "";
    }
}

function clientCmdMessage_Resize(%id, %position, %extent)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
    {
        %swatch.resize(
            getWord(%position, 0), getWord(%position, 1),
            getWord(%extent, 0), getWord(%extent, 1));

        clientCmdMessage_SetMargin(%id, %swatch.margin);
    }
}

function clientCmdMessage_SetMargin(%id, %margin)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
    {
        %swatch.margin = %margin;

        %x = getWord(%margin, 0);
        %y = getWord(%margin, 1);

        %swatch.text.resize(%x, %y,
            getWord(%swatch.extent, 0) - %x,
            getWord(%swatch.extent, 1) - %y);

        if (Canvas.isMember(PlayGui))
            %swatch.text.forceReflow();
    }
}

function clientCmdMessage_SetVisible(%id, %visible)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
        %swatch.setVisible(%visible);
}

function clientCmdMessage_SetAlpha(%id, %alpha)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
        %swatch.text.setAlpha(%alpha);
}

function clientCmdMessage_SetBackground(%id, %background)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
        %swatch.setColor(%background);
}

function clientCmdMessage_SetLineSpacing(%id, %lineSpacing)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
    {
        %swatch.text.lineSpacing = %lineSpacing;

        if (Canvas.isMember(PlayGui))
            %swatch.text.forceReflow();
    }
}

function clientCmdMessage_SetBitmapHeight(%id, %bitmapHeight)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
    {
        %swatch.text.maxBitmapHeight = %bitmapHeight;

        if (Canvas.isMember(PlayGui))
            %swatch.text.forceReflow();
    }
}

function clientCmdMessage_SetText(%id, %text)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
        %swatch.text.setText(%text);
}

function clientCmdMessage_AddText(%id, %text)
{
    %swatch = MessageOverlay.message[%id];

    if (isObject(%swatch))
        %swatch.text.setText(%swatch.text.getText() @ %text);
}

package MessageClientPackage
{
    function PlayGUI::onRender(%this)
    {
        Parent::onRender(%this);
        MessageOverlay.resize(0, 0, getWord(%this.extent, 0), getWord(%this.extent, 1));

        if (isObject(ServerConnection))
            commandToServer('ClientResolution', getRes());
    }

    function GameConnection::onConnectionAccepted(%this)
    {
        Parent::onConnectionAccepted(%this);

        commandToServer('ClientHasMessages');
        // commandToServer('ClientResolution', getRes());
    }

    function disconnectedCleanup(%rejoin)
    {
        if (isObject(MessageOverlay))
        {
            %count = MessageOverlay.getCount();

            for (%i = 0; %i < %count; %i++)
                MessageOverlay.message[MessageOverlay.getObject(%i).id] = "";

            MessageOverlay.deleteAll();
        }

        Parent::disconnectedCleanup(%rejoin);
    }
};

activatePackage("MessageClientPackage");
