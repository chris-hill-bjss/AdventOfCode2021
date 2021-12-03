namespace AdventOfCode2021.Solutions

module DayTwo = 

    open AdventOfCode2021.Http
    
    type SubState = {
        X : int
        Y : int
        Aim : int
    }

    let testInput = [|
        "forward", 5
        "down", 5
        "forward", 8
        "up", 3
        "down", 8
        "forward", 2
    |]

    let input = 
        AdventClient.getInputForDay 2
        |> Seq.map (fun s -> (s.Split(' ')[0], System.Int32.Parse(s.Split(' ')[1])))
        |> Seq.toArray

    let partOne (input:(string * int)[]) = 
        let mutable state = { X = 0; Y = 0; Aim = 0 }

        for direction, distance in input do
            match direction with
            | "forward" -> state <- { X = state.X + distance; Y = state.Y; Aim = state.Aim }
            | "up" -> state <- { X = state.X; Y = state.Y - distance; Aim = state.Aim }
            | "down" -> state <- { X = state.X; Y = state.Y + distance; Aim = state.Aim }
            | _ -> ()

        state.X * state.Y

    let partTwo (input:(string * int)[]) = 
        let mutable state = { X = 0; Y = 0; Aim = 0 }

        for direction, distance in input do
            match direction with
            | "forward" -> state <- { X = state.X + distance; Y = state.Y + (state.Aim * distance); Aim = state.Aim }
            | "up" -> state <- { X = state.X; Y = state.Y; Aim = state.Aim - distance }
            | "down" -> state <- { X = state.X; Y = state.Y; Aim = state.Aim + distance }
            | _ -> ()

        state.X * state.Y