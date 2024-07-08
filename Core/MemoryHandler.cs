using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using DBFConverter.Tokens;

namespace DBFConverter.Core;

public class MemoryHandler {
	public int Ptr { get; private set; }
	private int _nextFree = 0;
	private int _lastPtr;

	private Action<Token> _append;

	public MemoryHandler(Action<Token> append) {
		_append = append;
	}

	public void MoveTo(int ptr) {
		int diff = ptr - Ptr;

		Action action = diff > 0 ? Next : Prev;

		Repeat(action, Math.Abs(diff));

		_lastPtr = Ptr;	
		Ptr = ptr;
	}

	public void SetValue(byte value, byte? currentValue = null) {
		if (currentValue == null) {
			Erase();
		}
		int diff = value - (currentValue ?? 0);
		Action action = diff > 0 ? Add : Sub;
		
		Repeat(action, Math.Abs(diff));
	}

	public void MoveValue(int first, int second) {
		int diff = second - first;
		string c = diff > 0 ? ">" : "<";

		MoveTo(first);
		LBegin();	
			MoveTo(second);
			Add();
			
			MoveTo(first);
			Sub();
		LEnd();
	}

	public void CopyValue(int first, int second) {
		int tmp = Alloc(1);
		
		MoveTo(first);
		LBegin();
			MoveTo(second);
			Add();
			
			MoveTo(tmp);
			Add();
			
			MoveTo(first);
			Sub();	
		LEnd();
		// third -> first
		
		MoveValue(tmp, first);
		Free(tmp);
	}

	public void AddValue(int valuePtr, int targetPtr) {
		int tmp = Alloc(1);
		
		CopyValue(valuePtr, tmp);
		MoveValue(tmp, targetPtr);
		
		Free(tmp);
	}

	public void SubValue(int valuePtr, int targetPtr) {
		int tmp = Alloc(1);
		
		CopyValue(valuePtr, tmp);

		MoveTo(tmp);

		LBegin();
			MoveTo(targetPtr);
			Sub();

			MoveTo(tmp);

			IfNonZero(targetPtr,
				() => {},
				() => {
					MoveTo(tmp);
					SetValue(1);
				}
			);
			
			MoveTo(tmp);
			Sub();
		LEnd();

		Free(tmp);
	}

	public void MultiplyValue(int firstPtr, int secondPtr, int targetPtr) {
		int tmp = Alloc(1);
		
		CopyValue(firstPtr, tmp);

		MoveTo(tmp);
		
		LBegin();
			AddValue(secondPtr, targetPtr);

			MoveTo(tmp);
			Sub();
		LEnd();

		Free(tmp);
	}

	public void DivideValue(int firstPtr, int secondPtr, int targetPtr) {
		int tmp = Alloc(1);

		CopyValue(firstPtr, tmp);
		
		MoveTo(tmp);

		LBegin();
			SubValue(secondPtr, tmp);
			
			MoveTo(targetPtr);
			Add();

			MoveTo(tmp);
		LEnd();
		
		Free(tmp);
	}

	public void IfNonZero(int ptr, Action ifTrue, Action ifFalse) {
		int elsePtr = Alloc(1);
	
		MoveTo(elsePtr);
		Add();

		int tmp = Alloc(1);
		
		CopyValue(ptr, tmp);
		MoveTo(tmp);

		LBegin();
			ifTrue.Invoke();

			MoveTo(elsePtr);
			Sub();

			MoveTo(tmp);
			Erase();
		LEnd();

		MoveTo(elsePtr);
	
		LBegin();
			ifFalse.Invoke();
			
			MoveTo(elsePtr);
			Sub();
		LEnd();

		Free(elsePtr);
		Free(tmp);
	}

	public int Alloc(int size) {
		int tmp = _nextFree;
		_nextFree += size;

		return tmp;
	}
	
	public void Clear() {
		LBegin();
		Sub();
		LEnd();
	}

	public void Erase() {
		Clear();
	}

	public void Free(int ptr) {
		MoveTo(ptr);
		Erase();
		
		if (_nextFree - ptr <= 1) { // if not in the middle
			_nextFree = ptr;
		}

		MoveLast();
	}

	public void MoveLast() {
		MoveTo(_lastPtr);
	}
	
	public void DebugAddress(int address, byte border = 0) {
		// TODO: ptr saving method A->B
		// Currently inconsequence in moving last or not
		// Every method should go back?

		int prevPtr = Ptr;

		int tmp = Alloc(1);
		CopyValue(address, tmp);

		if (border != 0) {
			int borderTmp = Alloc(1);
			MoveTo(borderTmp);
			SetValue(border);

			AddValue(borderTmp, tmp);
			Free(borderTmp);
		}

		MoveTo(tmp);
		Out();

		MoveTo(prevPtr);
	}

	public void DebugValue(byte value) {
		int prevPtr = Ptr;

		int tmp = Alloc(1);
		MoveTo(tmp);
		SetValue(value);
		Out();
		Free(tmp);

		MoveTo(prevPtr);

	}

	public void DebugChar(char ch) {
		DebugValue((byte)ch);
	}

	// tokens methods:

	public void Add() => _append(Token.Add);
	public void Sub() => _append(Token.Sub);
	public void Next() => _append(Token.Next);
	public void Prev() => _append(Token.Prev);
	public void LBegin() => _append(Token.LBeg);
	public void LEnd() => _append(Token.LEnd);
	public void In() => _append(Token.In);
	public void Out() => _append(Token.Out);
	
	public void Empty() => _append(Token.EMPTY);
 
	public void Repeat(Action action, int n) {
		for (int i = 0; i < n; ++i) {
			action.Invoke();
		}
	}
}