open Hopac
open HttpFs.Client

let body =
  Request.createUrl Get "https://adventofcode.com/2021/day/1/input"
  |> Request.responseAsString
  |> run

let inputs = [|
    199
    200
    208
    210
    200
    207
    240
    269
    260
    263
|]

[<EntryPoint>]
let Main(_) =

    let increments = 
        [| for x, y in Array.zip inputs[..inputs.Length - 2] inputs[1..] -> if y > x then 1 else 0 |] 
        |> Array.sum

    printfn "%A" increments

    0