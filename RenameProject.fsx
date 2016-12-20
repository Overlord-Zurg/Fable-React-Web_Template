let newSolutionName = "New solution name goes here (no extension)"
let newProjectName = "New project name goes here (no extension)"

let originalSolutionName = "_Fable_Solution_"
let originalProjectName = "_Fable_Project_"

open System.IO
try Directory.GetFiles(__SOURCE_DIRECTORY__)
    |> Array.iter (fun x ->
        let fileText = File.ReadAllText x
        let newText = fileText.Replace(originalSolutionName, newSolutionName).Replace(originalProjectName, newProjectName)
        if fileText <> newText then File.WriteAllText(x, newText)
        let fileName = Path.GetFileName(x).ToLower()
        if fileName.Contains(originalSolutionName.ToLower()) || fileName.Contains(originalProjectName.ToLower()) then
            let newFileName = fileName.Replace(originalSolutionName.ToLower(), newSolutionName).Replace(originalProjectName.ToLower(), newProjectName)
            Directory.SetCurrentDirectory(Path.GetDirectoryName x)
            File.Move(fileName, newFileName))
    Array.append (Directory.GetDirectories(__SOURCE_DIRECTORY__)) [| __SOURCE_DIRECTORY__ |]
    |> Array.filter (fun x -> Path.GetDirectoryName(x).ToLower() <> ".git")
    |> Array.sortByDescending(fun x -> x.Length)
    |> Array.iter (fun x ->
        let directoryName = Path.GetDirectoryName(x).ToLower()
        if directoryName.Contains(originalSolutionName.ToLower()) || directoryName.Contains(originalProjectName.ToLower()) then
            let newDirName = directoryName.Replace(originalSolutionName.ToLower(), newSolutionName).Replace(originalProjectName.ToLower(), newProjectName)
            Directory.SetCurrentDirectory  (Directory.GetParent(x).FullName)
            Directory.Move (directoryName, newDirName))
with e -> failwithf "Failed to do something while trying to modify all the files. %A" e