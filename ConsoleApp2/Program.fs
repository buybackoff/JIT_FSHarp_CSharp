// Learn more about F# at http://fsharp.org

open System

open System.Threading
open Spreads.Utils
open System.Runtime.CompilerServices

type Worker() =
  [<MethodImplAttribute(MethodImplOptions.AggressiveInlining)>]
  member this.Work<'T>(x: 'T) : 'T =
    if typeof<'T> = typeof<uint64> then
      let d : uint64 = unbox(box(x)) // unbox(box(x))
      unbox(box(d))
    else 
      x

[<EntryPoint>]
let main argv =
  let worker = Worker()
  for round in 0..20 do
    let bench1 = 
      use b1 = Benchmark.Run("F# Ulong", 1000000, true)
      let mutable output : uint64 = Unchecked.defaultof<_>
      for i in 0UL..1000000UL do
        output <- output + (worker.Work(i));
      output

    let bench2 = 
      use b2 = Benchmark.Run("F# Long", 1000000, true)
      let mutable output : int64 = Unchecked.defaultof<_>
      for i in 0L..1000000L do
        output <- output + (worker.Work(i));
      output

    if int64(bench1) + bench2 < 0L then failwith ""

  Benchmark.Dump("F#")

  CSharpTest.Program.Main()

  Console.ReadLine() |> ignore
  0 // return an integer exit code
