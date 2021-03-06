﻿using System;
using System.Runtime.CompilerServices;
using Spreads.Utils;

namespace CSharpTest
{

    class Worker
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Work<T>(T x)
        {
            if (typeof(T) == typeof(ulong))
            {
                var u = (ulong)(object)x;
                u = u + 1;
                return (T)(object)u;
            }
            else
            {
                return x;
            }
        }
    }
    public class Program
    {
        public static void Main()
        {
            var worker = new Worker();
            for (int round = 0; round < 20; round++)
            {
                var sum1 = 0UL;
                using (Benchmark.Run("C# Ulong", 100000000, true))
                {
                    
                    for (var i = 0; i < 100000000; i++)
                    {
                        sum1 += worker.Work((ulong)i);
                    }
                }
                var sum2 = 0L;
                using (Benchmark.Run("C# Long", 100000000, true))
                {
                    for (var i = 0; i < 100000000; i++)
                    {
                        sum2 += worker.Work((long)i);
                    }
                }

                if (sum1 + (ulong)sum2 <= 10000)
                {
                    throw new Exception();
                }
            }

            Benchmark.Dump("C#");
            // Console.ReadLine();
        }
    }
}
