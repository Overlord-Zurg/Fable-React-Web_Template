let newSolutionName = "New solution name goes here (no extension)"
let newProjectName = "New project name goes here (no extension)"

let originalSolutionName = "_Fable_Solution_"
let originalProjectName = "_Fable_Project_"

open System.IO
try Directory.GetFiles(__SOURCE_DIRECTORY__, "*", SearchOption.AllDirectories)
    |> Array.filter (fun x ->
        not(x.ToLower().Contains(@"\.git\")) &&
        Path.GetFileName(x).ToLower() <> "renameproject.fsx" &&
        //not(Path.GetFileName(x).ToLower().Contains("_fable_solution_")) &&
        true)
    |> Array.iter (fun x ->
        let getContainingFolderName file = file |> Path.GetDirectoryName |> Path.GetFileName
        if not (["bin";"obj"] |> List.exists ((=) (getContainingFolderName x))) then
            let fileText = File.ReadAllText x
            let newText = fileText.Replace(originalSolutionName, newSolutionName).Replace(originalProjectName, newProjectName)
            let fileName = Path.GetFileName(x).ToLower()
            if fileText <> newText then
                printfn "Modifying file %s" fileName
                File.WriteAllText(x, newText)
            else
                printfn "NOT modifying file %s" fileName
            if fileName.Contains(originalSolutionName.ToLower()) || fileName.Contains(originalProjectName.ToLower()) then
                let newFileName = fileName.Replace(originalSolutionName.ToLower(), newSolutionName).Replace(originalProjectName.ToLower(), newProjectName)
                Directory.SetCurrentDirectory(Path.GetDirectoryName x)
                printfn "in folder %s, renaming file %s to %s" (Directory.GetCurrentDirectory()) fileName newFileName
                File.Move(fileName, newFileName)
            else printfn "in folder %s, NOT renaming file %s" (Directory.GetCurrentDirectory()) fileName)

    Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)
    Directory.Move(originalProjectName, newProjectName)
    if (Directory.Exists(Path.Combine(Directory.GetParent(__SOURCE_DIRECTORY__).FullName, originalSolutionName))) then
        Directory.SetCurrentDirectory(Directory.GetParent(__SOURCE_DIRECTORY__).FullName)
        Directory.Move(originalSolutionName, newSolutionName)

//    Array.append (Directory.GetDirectories(__SOURCE_DIRECTORY__, "*", SearchOption.AllDirectories)) [| __SOURCE_DIRECTORY__ |]
//    |> Array.filter (fun x -> not (x.Contains(".git")))
//    |> Array.sortBy(fun x -> x.Length)
//    |> Array.rev
//    |> Array.iter (fun x ->
//        let directoryName = Path.GetFileName(x).ToLower()
//        if directoryName.Contains(originalSolutionName.ToLower()) || directoryName.Contains(originalProjectName.ToLower()) then
//            let newDirName = directoryName.Replace(originalSolutionName.ToLower(), newSolutionName).Replace(originalProjectName.ToLower(), newProjectName)
//            Directory.SetCurrentDirectory  (Directory.GetParent(x).FullName)
//            printfn "in folder %s, renaming directory %s to %s" (Directory.GetCurrentDirectory()) directoryName newDirName
//            Directory.Move (directoryName, newDirName)
//        else printfn "in folder %s, NOT renaming directory %s" (Directory.GetCurrentDirectory()) directoryName)

with e -> failwithf "Failed to do something while trying to modify all the files. %A" e