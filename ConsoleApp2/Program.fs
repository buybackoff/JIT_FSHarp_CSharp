// Learn more about F# at http://fsharp.org

open System

open System.Threading
open Spreads.Utils
open System.Runtime.CompilerServices

type Worker() =
  [<MethodImplAttribute(MethodImplOptions.AggressiveInlining)>]
  member inline this.Work<'T>(x: 'T) : 'T =
    if obj.ReferenceEquals(typeof<'T>, typeof<uint64>) then
      let d : uint64 = unbox(box(x)) // unbox(box(x))
      unbox(box(d))
    else 
      x

[<EntryPoint>]
let main argv =
  let worker = Worker()
  for round in 0..20 do
    let bench1 = 
      use b1 = Benchmark.Run("F# Ulong", 10000000, true)
      let mutable output : uint64 = Unchecked.defaultof<_>
      for i = 0 to 10000000 do
        output <- output + (worker.Work(uint64 i));
      output

    let bench2 = 
      use b2 = Benchmark.Run("F# Long", 10000000, true)
      let mutable output : int64 = Unchecked.defaultof<_>
      for i = 0 to 10000000 do
        output <- output + (worker.Work(int64 i));
      output

    if int64(bench1) + bench2 < 0L then failwith ""

  Benchmark.Dump("F#")

  CSharpTest.Program.Main()

  Console.ReadLine() |> ignore
  0 // return an integer exit code
