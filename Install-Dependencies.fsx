open System.Diagnostics
open System.IO

open System.Security.Principal
let isElevated =
    let identity = WindowsIdentity.GetCurrent()
    let principal = new WindowsPrincipal(identity)
    principal.IsInRole(WindowsBuiltInRole.Administrator)

if isElevated then
    failwith "Install-Dependencies should be run with administrator privileges, just to be on the safe side."
else
    let npmInstall name = async {
        let proc = Process.Start("npm", sprintf "install %s" name)
        do proc.WaitForExit()
        if proc.ExitCode = 0 then
            printfn "Successfully installed %s" name
        else
            printfn "Exit code %i while attempting to install %s" proc.ExitCode name
        return proc.ExitCode = 0 }

    Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)

    let proc = Process.Start("npm", "list -g fable-compiler")
    proc.WaitForExit()
    match proc.ExitCode with
    | 0 -> ()
    | 1 -> 
        let proc = Process.Start("npm", "install fable-compiler")
        proc.WaitForExit()
        printfn "Fable installation exit code (zero is probably good): %i" proc.ExitCode
    | x -> printfn "I don'w know what exit code %i means" x

    if not (Directory.Exists "node_modules") then
        [   npmInstall "fable-core"
            npmInstall "fable-powerpack"
            npmInstall "fable-react"
            npmInstall "react"
            npmInstall "react-dom" ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> Array.iter (ignore)

    if not (Directory.Exists "_Fable_Project_/libs") then
        Directory.CreateDirectory("_Fable_Project_/libs") |> ignore    
        File.Copy("node_modules/react/dist/react.js", "_Fable_Project_/libs/react.js")
        File.Copy("node_modules/react-dom/dist/react-dom.js", "_Fable_Project_/libs/react-dom.js")