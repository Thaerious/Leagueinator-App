// See https://aka.ms/new-console-template for more information
using System.Reflection;

Console.WriteLine("Hello, World!");

string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
foreach(string name in names) Console.WriteLine(name);

Console.ReadLine();
