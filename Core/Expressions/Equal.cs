using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace DBFConverter.Core.Expressions;

public class Equal : LogicalExpression {
    public Equal(
		MemoryHandler memory,
		Expression first,
		Expression second) : base(memory) {
		
		Execute(first, second);
	}

	public void Execute(Expression first, Expression second) {
		int tmpFirst = _memory.Alloc(1);
		int tmpSecond = _memory.Alloc(1);

		_memory.CopyValue(first.Address, tmpFirst);
		_memory.CopyValue(second.Address, tmpSecond);

		_memory.SubValue(second.Address, tmpFirst);
		
		_memory.IfNonZero(
			tmpFirst,
			ifTrue: () => {
				MarkValue(false);
			},
			ifFalse: () => {
				_memory.SubValue(first.Address, tmpSecond);
				
				_memory.IfNonZero(
					tmpSecond,
					ifTrue: () => {			
						MarkValue(false);
					},
					ifFalse: () => {
						MarkValue(true);
					}
				);
			}
		);
	}
}