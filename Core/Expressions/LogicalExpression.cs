namespace DBFConverter.Core.Expressions;

public abstract class LogicalExpression : Expression {
    protected LogicalExpression(MemoryHandler memory) : base(memory, memory.Alloc(1)) {
	}

	public void MarkValue(bool value) {
		Value = Convert.ToByte(value);
	}
}