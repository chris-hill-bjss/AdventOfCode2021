namespace AdventOfCode2021.Solutions

module DayOne = 

    open AdventOfCode2021.Http
    
    let getTestInput () = [|
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

    let getInput () = 
        AdventClient.getInputForDay 1 
        |> Seq.map System.Int32.Parse 
        |> Seq.toArray

    let partOne (input:int[]) = 
        [| for x, y in Array.zip input[..input.Length - 2] input[1..] -> if y > x then 1 else 0 |] 
        |> Array.sum

    let partTwo (input:int[]) = 
        [| for x, y, z in Array.zip3 input[..input.Length - 3] input[1..input.Length - 2] input[2..] -> x + y + z |]
        |> partOne