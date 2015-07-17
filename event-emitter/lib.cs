function EventEmitter::emitEvent(%this, %event,
    %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r)
{
    %max = %this.eventListenerMax[%event];

    for (%i = 1; %i <= %max; %i++)
    {
        // TODO: Object-based and scoped callbacks
        call(%this.eventListenerFunc[%event, %i],
            %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r);
    }
}

function EventEmitter::emitEventTerminal(%this, %event,
    %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r)
{
    %max = %this.eventListenerMax[%event];

    for (%i = 1; %i <= %max; %i++)
    {
        // TODO: Object-based and scoped callbacks
        %result = call(%this.eventListenerFunc[%event, %i],
            %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r);

        if (%result !$= "")
            return %result;
    }

    return "";
}

function EventEmitter::addEventListener(%this, %event, %func)
{
    %this.eventListenerFunc[%event, %this.eventListenerMax[%event]++] = %func;
}

function EventEmitter::removeEventListener(%this, %event, %func)
{
    %max = %this.eventListenerMax[%event];

    for (%i = 1; %i <= %max; %i++)
    {
        if (%this.eventListenerFunc[%event, %i] $= %func)
        {
            while (%i < %this.eventListenerMax[%event, %i])
            {
                %this.eventListenerFunc[%event, %i] = %this.eventListenerFunc[%event, %i + 1];
                %i++;
            }

            %this.eventListenerFunc[%event, %this.eventListenerMax[%event]] = "";
            %this.eventListenerMax[%event]--;

            if (%this.eventListenerMax[%event] < 1)
                %this.eventListenerMax[%event] = "";

            return 1;
        }
    }

    return 0;
}

function EventEmitter::clearEventListeners(%this, %event)
{
    %max = %this.eventListenerMax[%event];

    for (%i = 1; %i <= %max; %i++)
        %this.eventListenerFunc[%event, %i] = "";

    %this.eventListenerMax[%event] = "";
}

// Shorthands
function EventEmitter::on(%this, %event, %func)
{
    return %this.addEventListener(%event, %func);
}

function EventEmitter::emit(%this, %event,
    %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r)
{
    return %this.emit(%event,
        %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r);
}
