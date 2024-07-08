using DBFConverter.Core.Objects;

namespace DBFConverter.Core.Expressions;

public abstract class Expression : BFObject {
    public int Address { get; }

	protected byte _value;
	
	public byte Value {
		internal get => _value;
		set {
			_memory.MoveTo(Address);
			_memory.SetValue(value);
			_memory.MoveLast();
			_value = value;
		}
	}

	protected Expression(MemoryHandler memory, int address) : base(memory) {
		Address = address;
    }

	public static Expression operator +(Expression e1, Expression e2) {
		var memory = e1._memory;

		var newVar = new Var(memory);
		
		memory.AddValue(e1.Address, newVar.Address);
		memory.AddValue(e2.Address, newVar.Address);
		
		newVar._value = (byte)(e1.Value + e2.Value);
		
		return newVar;
	}

	public static Var operator -(Expression e1, Expression e2) {
		var memory = e1._memory;

		var newVar = new Var(memory);

		memory.AddValue(e1.Address, newVar.Address);
		memory.SubValue(e2.Address, newVar.Address);
		
		newVar._value = (byte)(e1.Value - e2.Value);
		
		return newVar;
	}

	public static Var operator *(Expression e1, Expression e2) {
		var memory = e1._memory;

		var newVar = new Var(memory);

		memory.MultiplyValue(e1.Address, e2.Address, newVar.Address);
		
		newVar._value = (byte)(e1.Value * e2.Value);
		
		return newVar;
	}

	public static Var operator /(Expression e1, Expression e2) {
		var memory = e1._memory;

		var newVar = new Var(memory);

		memory.DivideValue(e1.Address, e2.Address, newVar.Address);
		
		newVar._value = (byte)(e1.Value / e2.Value);
		
		return newVar;
	}
}
