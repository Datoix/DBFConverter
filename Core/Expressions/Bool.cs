namespace DBFConverter.Core.Expressions;

public class Bool : LogicalExpression {
    public Bool(MemoryHandler memory, bool value) : base(memory) {
		MarkValue(value);
	}
}