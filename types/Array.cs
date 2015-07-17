// Largely untested

if (!isObject(ArraySortTextList) && isFunction("GuiTextListCtrl", "sort"))
    new GuiTextListCtrl(ArraySortTextList);

function emptyArray()
{
    return new ScriptObject()
    {
        class = "Array";
        length = 0;
    };
}

function splitArray(%text, %separator)
{
    if (%separator $= "")
        %separator = " ";

    %array = new ScriptObject()
    {
        class = "Array";
    };

    %max = -1;

    while (%text !$= "")
    {
        %text = nextToken(%text, "value", %separator);
        %array.value[%max++] = %value;
    }

    %array.length = %max + 1;
    return %array;
}

function Array::clear(%this)
{
    for (%i = 0; %i < %this.length; %i++)
        %this.value[%i] = "";

    %this.length = 0;
}

function Array::push(%this, %value)
{
    %this.value[%this.length] = %value;
    %this.length++;
}

function Array::pushAll(%this, %array)
{
    for (%i = 0; %i < %array.length; %i++)
    {
        %this.value[%this.length] = %array.value[%i];
        %this.length++;
    }
}

function Array::insert(%this, %index, %value)
{
    if (mFloor(%index) !$= %index || %index < 0 || %index > %this.length)
        return;

    for (%i = %this.length; %i > %index; %i++)
        %this.value[%i] = %this.value[%i - 1];

    %this.value[%index] = %value;
    %this.length++;
}

function Array::insertSwap(%this, %index, %value)
{
    if (mFloor(%index) !$= %index || %index < 0 || %index > %this.length)
        return;

    %this.value[%this.length] = %this.value[%index];
    %this.value[%index] = %value;
    %this.length++;
}

function Array::remove(%this, %index)
{
    if (mFloor(%index) !$= %index || %index < 0 || %index >= %this.length)
        return;

    %this.length--;

    for (%i = %index; %i < %this.length; %i++)
        %this.value[%i] = %this.value[%i + 1];

    %this.value[%this.length] = "";
}

function Array::removeSwap(%this, %index)
{
    if (mFloor(%index) !$= %index || %index < 0 || %index >= %this.length)
        return;

    %this.length--;
    %this.value[%index] = %this.value[%this.length];
    %this.value[%this.length] = "";
}

function Array::map(%this, %func)
{
    for (%i = 0; %i < %this.length; %i++)
        %this.value[%i] = call(%func, %this.value[%i]);
}

function Array::apply(%this, %action)
{
    for (%i = 0; %i < %this.length; %i++)
        call(%action, %this.value[%i]);
}

function Array::fold(%this, %initial, %func)
{
    for (%i = 0; %i < %this.length; %i++)
        %initial = call(%func, %initial, %this.value[%i]);

    return %initial;
}

function Array::filter(%this, %predicate)
{
    %i = 0;
    %j = 0;

    while (%i < %this.length)
    {
		if (call(%predicate, %this.value[%i + %j]))
        {
			%this.value[%i] = %this.value[%i + %j];
            %i++;
		}
        else
        {
			%j++;
			%this.length--;
		}
	}

	for (%i = 0; %i < %j; %i++)
		%this.value[%this.length + %i] = "";
}

function Array::reverse(%this)
{
    %max = mFloor((%this.length - 2) / 2);

    for (%i = 0; %i < %max; %i++)
    {
        %temp = %this.value[%i];
        %this.value[%i] = %this.value[%this.length - 1 - %i];
        %this.value[%this.length - 1 - %i] = %temp;
    }
}

function Array::shuffle(%this)
{
    for (%i = 0; %i < %this.length; %i++)
    {
        %j = getRandom(%i, %this.length - 1);
        %temp = %this.value[%i];
        %this.value[%i] = %this.value[%j];
        %this.value[%j] = %temp;
    }
}

// Ignores anything after the first tab in each value
function Array::guiSort(%this, %ascending)
{
    for (%i = 0; %i < %this.length; %i++)
        ArraySortTextList.addRow(%i, %this.value[%i]);

    ArraySortTextList.sort(0, %ascending);

    for (%i = 0; %i < %this.length; %i++)
        %this.value[%i] = ArraySortTextList.getRowText(%i);

    ArraySortTextList.clear();
}

function Array::join(%this, %separator)
{
    if (%this.length > 0)
        %result = %this.value0;

    for (%i = 1; %i < %this.length; %i++)
        %result = %result @ %separator @ %this.value[%i];

    return %result;
}
