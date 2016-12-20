namespace _Fable_Project_

open System.Web.Http
open System

type StatusGetResponse = {
    status: string
    time: DateTime }

type StatusController () =
    inherit ApiController()

    member this.Get () =
        this.Ok {
            status = "A-OK!"
            time = DateTime.Now }