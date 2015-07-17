function GameConnection::checkClientModules(%this)
{
    return "";
}

package ClientModuleServerPackage
{
    function GameConnection::onConnectRequest(
        %this, %address, %lanName, %netName, %prefix, %suffix,
        %rtb, %nonce, %modules, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k
    )
    {
        %result = Parent::onConnectRequest(
            %this, %address, %lanName, %netName, %prefix, %suffix,
            %rtb, %nonce, %modules, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k
        );

        if (%result !$= "")
            return %result;

        %count = getFieldCount(%modules);

        for (%i = 0; %i < %count; %i++)
        {
            %field = getField(%modules, %i);

            if (%field !$= "")
                %this.moduleVersion[getWord(%field, 0)] = getWord(%field, 1);
        }

        return %this.checkClientModules();
    }
};

activatePackage("ClientModuleServerPackage");
