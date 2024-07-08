namespace DBFConverter.Core.Objects;

public class Function : BFObject {
	private Action _action;

	public Function(MemoryHandler memory, Action action) : base(memory) {
		_action = action;
	}

	public T Arg<T>(int index) {
		throw new NotImplementedException();
	}

	public void Return<T>(T value) {
		throw new NotImplementedException();
	}
}