// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

void Foo(Dictionary<string, int> args) {
    Console.WriteLine($"FOO {args["a"]}");
}

var d = new Dictionary<string, int>() { { "a", 5 } };

Foo(new(){{ "a", 5 }});
Console.ReadLine();
