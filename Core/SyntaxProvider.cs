using System.Reflection.Metadata;
using DBFConverter.Core.Expressions;
using DBFConverter.Core.Objects;

namespace DBFConverter.Core;

public class SyntaxProvider {
    private readonly MemoryHandler _memory;

    public SyntaxProvider(MemoryHandler memory) {
		_memory = memory;
	}

	public Var Var(byte val) => new(_memory, val);
	public Var Var(Var var) => new(var);
	public Var Var() => new(_memory, 0);
	
	public void Condition(LogicalExpression logicalExpression, Action ifTrue, Action ifFalse)
	=> new Condition(_memory, logicalExpression, ifTrue, ifFalse).Execute();
	public LogicalExpression Equal(Expression e1, Expression e2) => new Equal(_memory, e1, e2);
	public LogicalExpression Bool(bool value) => new Bool(_memory, value);

	public T Arg<T>(Function func, int index) => func.Arg<T>(index);
	public void Return<T>(Function func, T value) => func.Return(value);
	public void Print(Expression expression) => new StdOut(_memory).Print(expression);
}
