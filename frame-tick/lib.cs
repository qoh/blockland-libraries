// by Port
//
// Call addFrameTick with a function name to call on every frame
// This function should return true when it wants to stop running (false otherwise)

function addFrameTick(%func)
{
    $FrameTickEntry[$FrameTickMax++] = %func;
}

function doFrameTick()
{
    for (%i = $FrameTickMax; %i >= 1; %i--)
    {
        if (call($FrameTickEntry[%i]))
        {
            $FrameTickEntry[%i] = "";
            $FrameTickMax--;
        }
    }

    return "";
}

if (!isObject(FrameTickCtrl))
{
    new GuiControl(FrameTickGui)
    {
        profile = "GuiDefaultProfile";

        new GuiConsoleTextCtrl(FrameTickCtrl)
        {
            profile = "GuiDefaultProfile";
            position = "-1 -1";
            minExtent = "0 0";
            extent = "0 0";

            expression = "doFrameTick()";
        };
    };

    if (isObject(Canvas))
    {
        Canvas.add(FrameTickGui);
        Canvas.bringToFront(FrameTickGui);
    }
}

package FrameTickPackage
{
    function Canvas::setContent(%this, %control)
    {
        Parent::setContent(%this, %control);

        if (!isObject(%control) || %control.getID() != FrameTickGui.getID())
        {
            %this.add(FrameTickGui);
            %this.bringToFront(FrameTickGui);
        }
    }
};

activatePackage("FrameTickPackage");
