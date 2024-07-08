using DBFConverter.Tokens;

namespace DBFConverter.Interpreter;

public class BFInterpreter {
    private readonly TokenHandler _tokenHandler;

	private readonly int _bufferSize;
    private byte[] _memory;
	private Stack<int> _loops = new();

	private int _ptr = 0;
	
	public BFInterpreter(int bufferSize) {
		_tokenHandler = new();
		_bufferSize = bufferSize;
		_memory = new byte[_bufferSize];
	}

	public void Execute(string program) {
		for (int i = 0; i < program.Length; ++i) {
			var token = _tokenHandler.FromChar(program[i]);

			switch (token) {	
				case Token.Next:
					Next(); break;
				case Token.Prev:
					Prev(); break;
				case Token.Out:
					Out(); break;
				case Token.In:
					In(); break;
				case Token.LBeg:
					LBegin(program, ref i); break;
				case Token.LEnd:
					LEnd(ref i); break;
				case Token.Add:
					Add(); break;
				case Token.Sub:
					Sub(); break;
			}
		}

		
	}

	private void Next() {
		if (_ptr + 1 >= _bufferSize) {
			new BFException("Above ptr range").Raise();
		}

		++_ptr;
	}

	private void Prev() {
		if (_ptr - 1 < 0) {
			new BFException("Below ptr range").Raise();
		}

		--_ptr;
	}

	private void Out() {
		Console.Write(
			Convert.ToChar(_memory[_ptr])
		);
	}

	private void In() {
		_memory[_ptr] = (byte)Console.ReadKey().KeyChar;
	}

	private void LBegin(string program, ref int i) {
		if (_memory[_ptr] > 0) {
			_loops.Push(i);
		} else {
			int loopsCount = 0;
			
			do {
				var token = _tokenHandler.FromChar(program[i]);
				
				if (token == Token.LBeg) {
					++loopsCount;
				} else if (token == Token.LEnd) {
					--loopsCount;
				}

				++i;
			} while (i < program.Length && loopsCount > 0);

			--i;
		}
		
	}

	private void LEnd(ref int i) {
		i = _loops.Pop() - 1; // for-loop adds unwanted 1
	}

	private void Add() {
		if (_memory[_ptr] + 1 <= byte.MaxValue) {
			++_memory[_ptr];
		} else {
			_memory[_ptr] = byte.MinValue;
		} 
	}

	private void Sub() {
		if (_memory[_ptr] - 1 >= byte.MinValue) {
			--_memory[_ptr];
		} else {
			_memory[_ptr] = byte.MaxValue;
		}
	}
}