$Pref::Server::DecalLimit = 100;
$Pref::Server::DecalTimeout = 20000;

if (!isObject(DecalSet))
    new SimSet(DecalSet);

function spawnDecal(%data, %position, %normal, %scale, %angle)
{
    if ($Pref::Server::DecalLimit < 1)
        return 0;

    %obj = new StaticShape()
    {
        datablock = %data;
    };

    if (!isObject(%obj))
        return 0;

    MissionGroup.add(%obj);
    DecalGroup.add(%obj);

    while (DecalGroup.getCount() > $Pref::Server::DecalLimit)
        DecalGroup.getObject(0).delete();

    if (%scale $= "")
        %size = %data.decalSize $= "" ? 1 : %data.decalSize;
    else
        %size = %data.decalSize $= "" ? %scale : %data.decalSize * %scale;

    // TODO: Rotate decal around normal by %angle
    %obj.setTransform(%position SPC vectorToAxis(%normal));
    %obj.setScale(%size SPC %size SPC %size);

    %fadeColor = "1 1 1 1";

    if (%data.doColorShift)
    {
        %fadeColor = %data.colorShiftColor;
        %obj.setNodeColor("ALL", %data.colorShiftColor);
    }

    if ($Pref::Server::DecalTimeout !$= "")
    {
        %obj.schedule($Pref::Server::DecalTimeout - 1000, "fadeOut");
        %obj.schedule($Pref::Server::DecalTimeout, "delete");
    }

    return %obj;
}

function spawnDecalFromRay(%ray, %data, %scale, %angle)
{
    %hit = firstWord(%ray);
    %moving = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;

    if (!isObject(%hit) || (%hit.getType() & %moving))
        return 0;

    return spawnDecal(%data, getWords(%ray, 1, 3), getWords(%ray, 4, 6), %scale, %angle);
}

function sprayDecal(%data, %scale, %angle,
    %start, %end, %mask, %exempt, %exempt2, %exempt3, %exempt4, %exempt5, %exempt6)
{
    return spawnDecalFromRay(
        containerRayCast(%start, %end, %mask, %exempt, %exempt2, %exempt3, %exempt4, %exempt5, %exempt6),
        %data, %scale, %angle);
}

function vectorToAxis(%vector)
{
    %y = mRadToDeg(mACos(getWord(%vector, 2) / vectorLen(%vector))) % 360;
    %z = mRadToDeg(mATan(getWord(%vector, 1), getWord(%vector, 0)));

    %euler = vectorScale(0 SPC %y SPC %z, $pi / 180);
    return getWords(matrixCreateFromEuler(%euler), 3, 6);
}

function StaticShape::fadeOut(%this, %color, %delta)
{
	%this.setNodeColor("ALL", getWords(%color, 0, 2) SPC getWord(%color, 3) * (%delta - 1));
	%this.schedule(50, "fadeOut", %color, %delta + 50 / 1000);
}
