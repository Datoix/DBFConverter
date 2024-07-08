using DBFConverter.Core.Expressions;

namespace DBFConverter.Core.Objects;

public class Condition : BFObject {
	private readonly LogicalExpression _logicalExpression;
    private readonly Action _ifTrue;
	private readonly Action _ifFalse;

	public Condition(
		MemoryHandler memory,
		LogicalExpression logicalExpression,
		Action ifTrue,
		Action ifFalse
	) : base(memory) {
		_logicalExpression = logicalExpression;

		_ifTrue = ifTrue;
		_ifFalse = ifFalse;
	}

	public void Execute() {
		_memory.IfNonZero(_logicalExpression.Address, _ifTrue, _ifFalse);
	}
}