#r "../../node_modules/fable-core/Fable.Core.dll"
#r "../../node_modules/fable-react/Fable.React.dll"

open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core
open Fable.Import.React

ReactDom.render(
    div [] [ str "If you can see this, Fable and React are both working. Hooray!" ],
    Browser.document.getElementById "root")