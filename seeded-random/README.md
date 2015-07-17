# Seeded Random

Implements separate RNG contexts that can be seeded arbitrarily.

Normally, all `getRandom` calls share the same context, which makes replicable seeded random number generation impractical. This library provides a `SeededRandom` class which provides exactly the same results as `getRandom` with the exception that it works in a separate context.

### Usage

Create a new context as follows:

```csharp
%context = SeededRandom();
```

`%context` will now mirror all future values provided by `getRandom` (but not skip them). This allows it to generate numbers without other scripts messing with it.

You can call `%context.get(10) == getRandom()` as many times as you want and it'll always be true (until one or the other is called out of order).

Optionally, you can pass an initial seed value to `SeededRandom(...)` in order to start with a fixed seed instead (or get/set the `seed` attribute on the returned object later). This lets you generate the same sequence of random numbers regardless of how other add-ons are using `getRandom`. `%context.get(...)` will behave identically to `getRandom(...)` but in a separate context.

* `%context.get()` - random float in `[0.0, 1.0]`
* `%context.get(n)` - random integer in `[0, n]`
* `%context.get(i, j)` - random integer in `[i, j]`

If you need to generate many random numbers in one place, it may be better to call `%context.push()` and use `getRandom` as you would normally, followed by `%context.pop()`. `::push()` and `::pop()` will temporarily modify the global `getRandom` to work in the same context as the `::get()` of this generator.

### API

##### `SeededRandom([int seed]) -> SeededRandom`

Create a new, separate context.

##### `SeededRandom::get([int i], [int j]) -> number`

Generate a random number. See documentation for `getRandom` for specifics.

##### `SeededRandom::push()`

Modify the global `getRandom` to provide numbers from the context of this generator.

##### `SeededRandom::pop()`

Restore the global `getRandom` and update this generator to the new state.
