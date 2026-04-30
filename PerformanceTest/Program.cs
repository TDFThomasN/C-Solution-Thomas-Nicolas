using System.Diagnostics;
using System;
using System.Threading.Tasks;

/*#########################*/

Console.WriteLine("Calcul de perf séquentiel");

var sw = Stopwatch.StartNew();

double sum = 0;

for (int i = 0; i < 50_000_000; i++)
{
    sum+=Math.Sin(i)+Math.Cos(i);
    sum+=Math.Sqrt(i);
    sum+=Math.Exp(i)+Math.Log(i+1);
    sum += Math.Pow(i % 100, 3);
    sum *= 1.000001;
}

sw.Stop();

Console.WriteLine($"{sw.ElapsedMilliseconds}ms");

/*#########################*/

Console.WriteLine("Calcul de perf parallèle (8 Threads)");

var sw2 = Stopwatch.StartNew();

double sum2 = 0;

object lockObj = new object();

var options = new ParallelOptions

{
    MaxDegreeOfParallelism = 8
};

Parallel.For(0, 50_000_000, options,

    () => 0.0,

    (i, state, localSum) =>

    {
        localSum += Math.Sin(i) + Math.Cos(i);
        localSum += Math.Sqrt(i);
        localSum += Math.Exp(i) + Math.Log(i + 1);
        localSum += Math.Pow(i % 100, 3);
        localSum *= 1.000001;
        return localSum;
    },

    localSum =>

    {
        lock (lockObj)
        {
            sum2 += localSum;
        }
    });

sw2.Stop();

Console.WriteLine($"{sw2.ElapsedMilliseconds}ms");

