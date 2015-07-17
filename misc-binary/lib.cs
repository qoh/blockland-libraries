function u31_add(%a, %b) { return ((%a & 0x7fffffff) + (%b & 0x7fffffff)) | 0; }
function u31_sub(%a, %b) { return ((%a & 0x7fffffff) - (%b & 0x7fffffff)) | 0; }
function u31_mul(%a, %b) { return ((%a & 0x7fffffff) * (%b & 0x7fffffff)) | 0; }
function u31_div(%a, %b) { return ((%a & 0x7fffffff) / (%b & 0x7fffffff)) | 0; }

function s32_add(%a, %b) { return ((%a | 0) + (%b | 0)) | 0; }
function s32_sub(%a, %b) { return ((%a | 0) - (%b | 0)) | 0; }
function s32_mul(%a, %b) { return ((%a | 0) * (%b | 0)) | 0; }
function s32_div(%a, %b) { return ((%a | 0) / (%b | 0)) | 0; }

// TODO: u32_* math ops

function u32_lt(%a, %b)
{
	%i = %a & 0x80000000;
	%j = %b & 0x80000000;
	if (%i != %j)
		return %j;
	return (%a ^ %i) < (%b ^ %j);
}

function u32_gt(%a, %b)
{
	%i = %a & 0x80000000;
	%j = %b & 0x80000000;
	if (%i != %j)
		return %i;
	return (%a ^ %i) > (%b ^ %j);
}

function u32_le(%a, %b)
{
	return !u32_gt(%a, %b);
}

function u32_ge(%a, %b)
{
	return !u32_lt(%a, %b);
}

function bin(%n)
{
    while (%n != 0)
    {
        %result = (%n & 1) @ %result;
        %n >>= 1;
    }

    return %result;
}

function oct(%n)
{
    while (%n != 0)
    {
        %result = getSubStr("0123456789abcdef", %n & 7, 1) @ %result;
        %n >>= 3;
    }

    return %result;
}

function hex(%n)
{
    while (%n != 0)
    {
        %result = getSubStr("0123456789abcdef", %n & 15, 1) @ %result;
        %n >>= 4;
    }

    return %result;
}

function chr(%i)
{
    return $_byte_map[%i];
}

function ord(%c)
{
    return 1 + strpos($_byte_list, %c);
}

if ($_byte_list $= "")
{
    for ($_i = 1; $_i < 256; $_i++)
    {
        $_byte_map[$_i] = collapseEscape("\\x" @
            getSubStr("0123456789abcdef", $_i >> 4, 1) @
            getSubStr("0123456789abcdef", $_i & 15, 1));
        $_byte_list = $_byte_list @ $_byte_map[$_i];
    }
}
