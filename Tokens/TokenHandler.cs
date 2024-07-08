namespace DBFConverter.Tokens;

public class TokenHandler {
	private Token[] _tokens = [
		Token.Add,
		Token.Sub,
		Token.Next,
		Token.Prev,
		Token.LBeg,
		Token.LEnd,
		Token.In,
		Token.Out
	];

	private char[] _chars = [
		'+',
		'-',
		'>',
		'<',
		'[',
		']',
		',',
		'.'
	];


	public Token? FromChar(char ch) {
		int index = Array.IndexOf(_chars, ch);
		if (index == -1) return null;

		return _tokens[index];
	}

	public char FromToken(Token token) {
		int index = Array.IndexOf(_tokens, token);
		if (index == -1) return ' ';
		
		return _chars[index];
	}
}