open AdventOfCode2021.Solutions

[<EntryPoint>]
let Main(_) =

    DayTwo.getInput()
    |> DayTwo.solve DayTwo.partOneCommandProcessor
    |> printfn "%A"

    DayTwo.getInput()
    |> DayTwo.solve DayTwo.partTwoCommandProcessor
    |> printfn "%A"

    0