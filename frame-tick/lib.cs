// by Port
//
// Call addFrameTick with a function name to call on every frame
// This function should return true when it wants to stop running (false otherwise)
// The function will be called with the amount of seconds that elapsed since the last frame

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
