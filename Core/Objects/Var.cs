using DBFConverter.Core.Expressions;

namespace DBFConverter.Core.Objects;

public class Var : Expression {
	public Var(MemoryHandler memory, byte value = 0)
		: base(memory, memory.Alloc(1)) {
			
		Value = value;
	}

	public Var(Var variable)
		: base(variable._memory, variable._memory.Alloc(1)) {
		
		_value = variable.Value;
		_memory.CopyValue(variable.Address, Address);
	}
}