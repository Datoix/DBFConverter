using DBFConverter.Core.Expressions;

namespace DBFConverter.Core.Objects;

public class StdOut : BFObject {
	public StdOut(MemoryHandler memory) : base(memory) {
		
	}

	public void Print(Expression expression) {
		_memory.MoveTo(expression.Address);
		_memory.Out();
		_memory.MoveLast();
	}
}