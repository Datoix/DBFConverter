# DBFConverter

Converts C# code to brainfuck

## Example

```csharp
BFCompiler compiler = new();

var code = compiler.Execute(bf => {
	var a = bf.Var();
	a.Value = 65;
	bf.Print(a); // output: A    (ascii table)

	var b = bf.Var(4);
	var c = bf.Var(5);

	var e1 = bf.Var(1);
	var e2 = bf.Var(2);

	bf.Condition(
		bf.Equal(e1, e2),
		() => bf.Print(a),
		() => bf.Print(b * c + a)
	);
});

Console.WriteLine(code);
```

## Implemented

- Built-in `BF` interpreter (`BFInterpreter` class)

```csharp
BFInterpreter interpreter = new(bufferSize: 1000);
interpreter.Execute("<BF program>");
```

- Variables (`bf.Var`)
- Conditions (`bf.Condition`)
- Logical expressions (`bf.Equal`)
- Arithmetic operations (`+`, `-`, `*`, `/`)

## Future plans
- Loops
- Functions