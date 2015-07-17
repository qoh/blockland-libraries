# Field Access

Provides `eval`-free read/write access to arbitrary object fields.

##### `SimObject::getField(string name)`

Returns the value assigned to field `name` on the object, e.g. `%foo.getField("bar")` is equivalent to `%foo.bar`.

##### `SimObject::setField(string name, value)`

Assigns `value` to field `name` on the object, e.g. `%foo.setField("bar", 3)` is equivalent to `%foo.bar = 3`.
