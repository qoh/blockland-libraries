// by Port
//
// Call addFrameTick with a function name to call on every frame
// This function should return true when it wants to stop running (false otherwise)
// You can pass a second argument to addFrameTick which will be passed along to the function.
//
// The function will also be passed some arbitrary number. It's supposed to be the
// amount of seconds that elapsed since the last frame but it usually isn't.

function addFrameTick(%func, %data)
{
    $FrameTickFunc[$FrameTickMax++] = %func;
    $FrameTickData[$FrameTickMax] = %data;
}

function doFrameTick()
{
    %dt = $Sim::Time - $LastFrameTickTime;
    $LastFrameTickTime = $Sim::Time;

    for (%i = $FrameTickMax; %i >= 1; %i--)
    {
        if (call($FrameTickFunc[%i], $FrameTickData[%i], %dt))
        {
            $FrameTickFunc[%i] = "";
            $FrameTickData[%i] = "";
            $FrameTickMax--;
        }
    }

    return "";
}

if (!isObject(FrameTickGui))
{
    new GuiControl(FrameTickGui)
    {
        profile = "GuiModelessDialogProfile";
        noCursor = true;
    
        new GuiConsoleTextCtrl()
        {
            profile = "GuiTextProfile";
            position = "-1 -1";
            minExtent = "0 0";
            extent = "0 0";
    
            expression = "doFrameTick()";
        };
    };
}

package FrameTickPackage
{
    function Canvas::setContent(%this, %control)
    {
        Parent::setContent(%this, %control);

        if (!isObject(%control) || %control.getID() != FrameTickGui.getID())
            %this.add(FrameTickGui);
    }

    function scrollInventory(%value)
    {
        %name = "FrameTickGui";
        %id = nameToID(%name);
        %id.setName("FrameOverlayGui");
        Parent::scrollInventory(%value);
        %id.setName(%name);
    }
};

activatePackage("FrameTickPackage");
