using DBFConverter.Core;
using DBFConverter.Interpreter;

BFInterpreter interpreter = new(bufferSize: 1000);
BFCompiler compiler = new();


// Testing

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
interpreter.Execute(code);