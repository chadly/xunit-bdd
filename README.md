# xUnit.Net BDD Extensions

Extends xUnit.Net with Behavior Driven Development style fixtures.

Some of the design goals include:
 * Use natural C# constructs to control things such as BDD contexts and concerns. For example, the context of the specification is defined in the class constructor and the concern for the fixture is defined by its namespace.
 * Async tests are a first class citizen.

See here for a [full introduction](https://www.chadly.net/bdd-with-xunit-net/)

## How to Use

Install via [nuget](https://www.nuget.org/packages/xUnit.BDD/)

```
dotnet add package xUnit.BDD
```

### Write a Scenario

```cs
using Xunit;

public class Calculator
{
	public int Add(int x, int y) => x + y;
}

public class when_adding_two_numbers : Specification
{
	readonly Calculator calc;
	int result;

	public when_adding_two_numbers()
	{
		calc = new Calculator();
	}

	protected override Task ObserveAsync()
	{
		result = calc.Add(1, 2);
		return Task.CompletedTask;
	}

	[Observation]
	public void should_return_correct_result()
	{
		result.ShouldEqual(3);
	}
}
```

This is a contrived example, but the idea is to run the code you're observing in the `ObserveAsync` method and have one or more `Observation`s on what you observed. This way, when a test (`Observation`) fails, the language of the class and method (the scenario) should be granular enough to let you know exactly what failed.

### Async Scenarios

The `Specification` is async by default (e.g. the `ObserveAsync` method is expected to run async) since this is a brave new async world we live in. Since C# does not allow async constructors or async `Dispose` methods, if you need to do any async setup or teardown for the test, you should override `InitializeAsync` and/or `DisposeAsync`.

```cs
public class when_doing_some_async_thing : Specification
{
	string result;

	protected override async Task InitializeAsync()
	{
		await myThing.DoAThingAsync();
	}

	protected override async Task ObserveAsync()
	{
		result = await myThing.DoTheThing();
	}

	[Observation]
	public async Task should_return_correct_result()
	{
		string otherThing = await someOtherThing.DoAnotherThing();
		result.ShouldEqual(otherThing);
	}
}
```

You should explicitly **avoid** implementing Xunit's `IAsyncLifetime` since that is what `Specification` does internally to do its magic. Use the hooks on the `Specification` class.

## Building Locally

After cloning, run:

```
dotnet restore
dotnet build
```

To run the test cases:

```
cd test
dotnet test
```
