
using System;

Console.Write("Expression: ");
string input = Console.ReadLine();

char[] operators = { '+', '-', '*', '/' };

foreach (char op in operators)
{
    if (input.Contains(op))
    {
        string[] parts = input.Split(op);

        double a = double.Parse(parts[0]);
        double b = double.Parse(parts[1]);

        double result = op switch
        {
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            '/' => a / b
        };

        Console.WriteLine($"Résultat: {result}");
        return;
    }
}
Console.WriteLine("Aucun opérateur reconnu"); 

