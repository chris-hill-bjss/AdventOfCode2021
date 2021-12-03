namespace AdventOfCode2021.Http

module AdventClient =

    open Hopac
    open HttpFs.Client

    let readFile fileName =
        System.IO.File.ReadAllText(fileName)
        |> String.split '\n'

    let writeFile fileName content =
        if not(System.IO.Directory.Exists("./input")) then
            System.IO.Directory.CreateDirectory("./input") |> ignore
            
        System.IO.File.WriteAllText (fileName, content)

    let downloadInputForDay (day:int) =
        Request.createUrl Get $"https://adventofcode.com/2021/day/{day}/input"
        |> Request.cookie (Cookie.create("session","<yourcookiegoesvaluehere>"))
        |> Request.responseAsString
        |> run
        |> String.trim
        
    let getInputForDay (day:int) =
        if not(System.IO.File.Exists $"./input/day{day}.txt") then
            downloadInputForDay day 
            |> writeFile $"./input/day{day}.txt"

        readFile $"./input/day{day}.txt"
