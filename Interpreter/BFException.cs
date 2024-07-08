namespace DBFConverter.Interpreter;

public class BFException {
    private readonly string _message;

    public BFException(string message) {
		_message = message;
	}

	public void Raise() {
		Console.WriteLine($"[Error] {_message}");
		Environment.Exit(-1);
	}
}