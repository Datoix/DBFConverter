
using DBFConverter.Tokens;

namespace DBFConverter.Core;

public class BFCompiler {
	private readonly MemoryHandler _memoryHandler;
	private readonly SyntaxProvider _syntaxProvider;
	private readonly TokenHandler _tokenHandler;

	private string _output = "";

	public BFCompiler() {
		_memoryHandler = new MemoryHandler(Append);	
		_syntaxProvider = new SyntaxProvider(_memoryHandler);
		_tokenHandler = new TokenHandler();
	}

	// TODO: cmd parameters
	public string Execute(Action<SyntaxProvider> action) {
		_output = "";
		action.Invoke(_syntaxProvider);		
		
		return _output;
	}

	private void Append(Token token) {
		_output += _tokenHandler.FromToken(token);
	}
  
}