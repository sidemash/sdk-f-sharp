//   Copyright © 2020 Sidemash Cloud Services
//
//   Licensed under the Apache  License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless  required  by  applicable  law  or  agreed to in writing,
//   software  distributed  under  the  License  is distributed on an
//   "AS IS"  BASIS, WITHOUT  WARRANTIES  OR CONDITIONS OF  ANY KIND,
//   either  express  or  implied.  See the License for the  specific
//   language governing permissions and limitations under the License.


namespace Sidemash.Sdk
open FSharp.Data

module rec ListForm =
    type T = { Where: option<string>  
               Limit: option<int32>  
               OrderBy: option<string> } with 
        override self.ToString() = ListForm.toString(self)
        member self.ToJson() = ListForm.toJson(self)

    let make where limit orderBy =
        { Where=where; Limit=limit; OrderBy=orderBy }

    let empty: ListForm.T =
        { Where=None
          Limit=None
          OrderBy=None }

    let isEmpty (form: ListForm.T) =
        Option.isNone (form.Where)
        && Option.isNone (form.Limit)
        && Option.isNone (form.OrderBy)

    let toQueryString (form: ListForm.T) =
        (form.Where |> Option.map(fun w -> ("where", w)) |> Option.toList)
        @ (form.OrderBy |> Option.map(fun o -> ("orderBy", o)) |> Option.toList)
        @ (form.Limit |> Option.map(fun l -> ("limit", l.ToString())) |> Option.toList)

    let toJson (form:ListForm.T) =
        JsonValue.Record (Array.concat [ form.Where |> Option.toArray |> Array.map (fun w -> ("where", JsonValue.String w))
                                         form.Limit |> Option.toArray |> Array.map (fun l -> ("limit", JsonValue.Number(decimal(l))))
                                         form.OrderBy |> Option.toArray |> Array.map (fun o -> ("orderBy", JsonValue.String o)) ])

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let where = (Map.tryFind "where" map) |> Option.map (fun j -> j.AsString())
            let limit = (Map.tryFind "limit" map) |> Option.map (fun j -> j.AsInteger())
            let orderBy = (Map.tryFind "orderBy" map) |> Option.map (fun j -> j.AsString())
            { Where=where; Limit=limit; OrderBy=orderBy }
        | _ -> failwith "Invalid Json submitted for ListForm" 

    let toString (form:ListForm.T) =
        "ListForm(Where=" + form.Where.ToString() +
                 ", Limit=" + form.Limit.ToString() +
                 ", OrderBy=" + form.OrderBy.ToString() + ")"