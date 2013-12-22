# xUnit.Net BDD Extensions

Extends xUnit.Net with Behavior Driven Development style fixtures.

Some of the design goals include:
 * Use natural C# constructs to control things such as BDD contexts and concerns. For example, the context of the specification is defined in the class constructor and the concern for the fixture is defined by its namespace.
 * Don't force me to inherit from any base class to get BDD style tests working.There is an interface `ISpecification` with one method `Observe()` to accomplish this.  A `Specification` base class is also provided for convenience.

See here for a [full introduction](http://chadly.net/2009/04/bdd-with-xunit-net/)
