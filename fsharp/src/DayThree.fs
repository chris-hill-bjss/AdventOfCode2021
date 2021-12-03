namespace AdventOfCode2021.Solutions

module DayThree = 

    open AdventOfCode2021.Http
    
    let getTestInput () = [|
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010"
    |]

    let getInput () = 
        AdventClient.getInputForDay 3

    let partOne (input:string[]) =
        input