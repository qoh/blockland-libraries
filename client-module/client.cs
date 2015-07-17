function registerClientModule(%id, %version)
{
    %count = getFieldCount($ClientModuleList);

    for (%i = 0; %i < %count; %i++)
    {
        %field = getField($ClientModuleList, %i);

        if (getWord(%field, 0) $= %id)
        {
            if (%version > getWord(%field, 1))
                $ClientModuleList = setField($ClientModuleList, %i, %entry);

            return;
        }
    }

    $ClientModuleList = $ClientModuleList TAB %id SPC %version;
}

package ClientModuleClientPackage
{
    function GameConnection::setConnectArgs(%this, %lanName, %netName,
        %prefix, %suffix, %rtb, %nonce, %modules,
        %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l)
    {
        if (%modules $= "")
            %modules = $ClientModuleList;
        else
            %modules = %modules TAB $ClientModuleList;

        Parent::setConnectArgs(%this, %lanName, %netName,
            %prefix, %suffix, %rtb, %nonce, %modules,
            %a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l);
    }
};

activatePackage("ClientModuleClientPackage");
