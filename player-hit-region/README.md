# Player Region Detection

Provides a single function which will determine which part of a player corresponds to a certain position. This is mainly useful for changing damage dealt by weapons based on where the projectile/raycast hit, or perhaps clicking on body parts to view health, etc.

### Usage

Just call `Player::getRegion` on the player, passing the position to it. It will return the name of a player part in a string, e.g. `%part = %player.getRegion(%position);`

##### `string Player::getRegion(Point3F position, bool clamp=false)`

Determine what part corresponds to a world space position on the player. If `clamp` is true, positions below or above the player will be snapped onto it.

Here's all the possible part names/return values:

* `"head"` - Anywhere on the player's head
* `"chest"` - Front or back of chest
* `"hip"` - Pants/skirt top
* `"larm"` - Left arm
* `"rarm"` - Right arm
* `"lleg"` - Left leg
* `"rleg"` - Right leg
* `""` - Unable to determine part. This will only be returned if the player is crouching - the library does not yet support part detection on crouched players, as the bounding box in no way corresponds to the model - the hip or anything above it wouldn't be detected.
