namespace AdventOfCode2021.Solutions

module DayTwo = 

    open AdventOfCode2021.Http
    
    type SubState = {
        X : int
        Y : int
        Aim : int
    }

    let getTestInput () = [
        "forward", 5
        "down", 5
        "forward", 8
        "up", 3
        "down", 8
        "forward", 2
    ]

    let getInput () = 
        AdventClient.getInputForDay 2
        |> Seq.map (fun s -> (s.Split(' ')[0], System.Int32.Parse(s.Split(' ')[1])))
        |> Seq.toList

    let partOneCommandProcessor direction distance state =
        match direction with
        | "forward" -> { X = state.X + distance; Y = state.Y; Aim = state.Aim }
        | "up" -> { X = state.X; Y = state.Y - distance; Aim = state.Aim }
        | "down" -> { X = state.X; Y = state.Y + distance; Aim = state.Aim }
        | _ -> state

    let partTwoCommandProcessor direction distance state =
        match direction with
        | "forward" -> { X = state.X + distance; Y = state.Y + (state.Aim * distance); Aim = state.Aim }
        | "up" -> { X = state.X; Y = state.Y; Aim = state.Aim - distance }
        | "down" -> { X = state.X; Y = state.Y; Aim = state.Aim + distance }
        | _ -> state

    let solve (commandProcessor:string->int->SubState->SubState) (input:(string * int)list) =

        let rec processInput input state =
            match input with
            | [] -> state
            | (direction, distance) :: tail -> processInput tail (commandProcessor direction distance state)

        let finalState = processInput input {X=0;Y=0;Aim=0}

        finalState.X * finalState.Y