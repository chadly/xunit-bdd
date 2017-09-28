# xUnit.Net BDD Extensions

Extends xUnit.Net with Behavior Driven Development style fixtures.

Some of the design goals include:
 * Use natural C# constructs to control things such as BDD contexts and concerns. For example, the context of the specification is defined in the class constructor and the concern for the fixture is defined by its namespace.
 * Don't force me to inherit from any base class to get BDD style tests working. There is an interface `ISpecification` that one can implement to accomplish this. A `Specification` & `AsyncSpecification` base classes are also provided for convenience.

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

	public override void Observe()
	{
		result = calc.Add(1, 2);
	}

	[Observation]
	public void should_return_correct_result()
	{
		result.ShouldEqual(3);
	}
}
```

This is a contrived example, but the idea is to run the code you're observing in the `Observe` method and have one or more `Observation`s on what you observed. This way, when a test (`Observation`) fails, the language of the class and method (the scenario) should be granular enough to let you know exactly what failed.

### Async Tests

If you are testing something that is async, you can use the `AsyncSpecification`:

```cs
using Xunit.Extensions;

public class Calculator
{
	public async Task<int> CalculateMagicNumber(int x, int y)
	{
		int result = await SomeAsyncCallToAnExternalService();
		return result + x + y;
	}
}

public class when_making_magic_numbers : AsyncSpecification
{
	readonly Calculator calc;
	int result;

	public when_making_magic_numbers()
	{
		calc = new Calculator();
	}

	public override async Task ObserveAsync()
	{
		result = await calc.CalculateMagicNumber(1, 2);
	}

	[Observation]
	public void should_return_correct_result()
	{
		result.ShouldEqual(69);
	}
}
```

If you have some async setup/teardown that also needs to run with the test, you can override `InitializeAsync` and/or `DisposeAsync`:

```cs
using Xunit.Extensions;

public class when_doing_more_stuff : AsyncSpecification, IAsyncLifetime
{
	public async Task InitializeAsync()
	{
		await DoSomething();
	}

	public override async Task ObserveAsync()
	{
		result = await calc.CalculateMagicNumber(1, 2);
	}

	public async Task DisposeAsync()
	{
		await DoSomethingElse();
	}

	[Observation]
	public void should_return_correct_result()
	{
		result.ShouldEqual(69);
	}
}
```

### Shared Context

When writing test scenarios like this, you "observe" one thing in the `Observe` method and make one or more `Observation`s on the results. Due to this, the test framework overrides [Xunit's default handling of shared test context](https://xunit.github.io/docs/shared-context.html). Instead of creating a new instance of the class for each `Observation` and rerunning the test setup & `Observe` method, the test harness will create the class once, run the setup & `Observe` once, and then run all of the `Observation`s in sequence.

In other words, it treats all `ISpecification` tests as [class fixtures](https://xunit.github.io/docs/shared-context.html#class-fixture) (e.g. shared object instance across tests in a single class).

If you write BDD scenarios as prescribed, this should make no difference to you. It is simply a performance optimization that you should be aware of.

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
