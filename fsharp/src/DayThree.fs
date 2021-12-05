namespace AdventOfCode2021.Solutions

module DayThree = 

    open AdventOfCode2021.Http
    
    let getTestInput () = [
        "00100"
        "11110"
        "10110"
        "10111"
        "10101"
        "01111"
        "00111"
        "11100"
        "10000"
        "11001"
        "00010"
        "01010"
    ]

    let getInput () = 
        AdventClient.getInputForDay 3

    let getBitPositions (input:string list) = 
        [0..input[0].Length - 1]
        |> Seq.map (fun i -> input |> Seq.map (fun row -> row[i]) |> Seq.toList)
        |> Seq.toList

    let charArrayToString (c:char[]) =
        System.String c

    let binaryStringToInt s =
        System.Convert.ToInt32(s, 2)

    let partOne (input:string list) =
        let gamma = 
            input
            |> getBitPositions
            |> List.map (fun bits -> bits |> List.averageBy (fun c -> (if c = '1' then 1 else 0) |> float))
            |> List.map (fun v -> if v >= 0.5 then '1' else '0')
            |> List.toArray

        let epsilon = gamma |> Array.map (fun i -> if i = '1' then '0' else '1')

        let g = gamma |> charArrayToString |> binaryStringToInt
        let e = epsilon |> charArrayToString |> binaryStringToInt
        
        g * e

    let getEquipmentRating input bitSelector = 

        let rec loop (input:string list) (position:int) =
            let significantBit =
                input
                |> getBitPositions 
                |> List.item position        
                |> List.averageBy (fun c -> (if c = '1' then 1 else 0) |> float)
                |> bitSelector

            let validInputs = input |> List.filter (fun reading -> reading[position] = significantBit)
            if validInputs.Length > 1 then 
                loop validInputs (position + 1)
            else
                validInputs |> List.first

        loop input 0
        
    let partTwo input =
        let oxyGenRating = getEquipmentRating input (fun v -> if v >= 0.5 then '1' else '0')
        let c02ScrubberRating = getEquipmentRating input (fun v -> if v >= 0.5 then '0' else '1')

        // this is nasty but I'm tired :D
        let o = oxyGenRating.Value |> binaryStringToInt
        let c = c02ScrubberRating.Value |> binaryStringToInt
        
        o * c