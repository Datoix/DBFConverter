namespace DBFConverter.Core.Objects;

public abstract class BFObject {
	protected readonly MemoryHandler _memory;

	public BFObject(MemoryHandler memory) {
		_memory = memory;
	}
}