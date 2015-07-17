function SeededRandom(%seed)
{
    if (%seed $= "")
        %seed = getRandomSeed();

    return new ScriptObject()
    {
        class = "SeededRandom";
        seed = %seed;
    };
}

function SeededRandom::next(%this, %i, %j)
{
    %seed = getRandomSeed();
    setRandomSeed(%this.seed);

    if (%j $= "")
    {
        if (%i $= "")
            %value = getRandom();
        else
            %value = getRandom(%i);
    }
    else
        %value = getRandom(%i, %j);

    %this.seed = getRandomSeed();
    setRandomSeed(%seed);

    return %value;
}

function SeededRandom::push(%this)
{
    if (%this.prev !$= "")
    {
        error("ERROR: Duplicate push() call");
        return;
    }

    %this.prev = getRandomSeed();
    setRandomSeed(%this.seed);
}

function SeededRandom::pop(%this)
{
    if (%this.prev $= "")
    {
        error("ERROR: pop() called without previous push() call");
        return;
    }

    %this.seed = getRandomSeed();
    setRandomSeed(%this.prev);
    %this.prev = "";
}
