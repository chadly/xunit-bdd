# xUnit.Net BDD Extensions

Extends xUnit.Net with Behavior Driven Development style fixtures.

Some of the design goals include:
 * Use natural C# constructs to control things such as BDD contexts and concerns. For example, the context of the specification is defined in the class constructor and the concern for the fixture is defined by its namespace.

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

	protected override void Observe()
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

This is a contrived example, but the idea is to run the code you're observing in the `Observe` method and have one or more `Observation`s on what you observed. This way, when a test (`Observation`) fails, the language of the class and method (the scenario) should be granular enough to let you know exactly what failed.

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
