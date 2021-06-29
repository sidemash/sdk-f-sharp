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

module rec Pagination =
    type T = { SelfUrl: string  
               PrevUrl: option<string>  
               NextUrl: option<string>  
               StartedTime: UTCDateTime.T  
               NbItemsOnThisPage: int32  
               Page: int32  
               NbItemsPerPage: int32 } with 
        override self.ToString() = Pagination.toString(self)
        member self.ToJson() = Pagination.toJson(self)

    let make selfUrl prevUrl nextUrl startedTime nbItemsOnThisPage page nbItemsPerPage =
        { SelfUrl=selfUrl
          PrevUrl=prevUrl
          NextUrl=nextUrl
          StartedTime=startedTime
          NbItemsOnThisPage=nbItemsOnThisPage
          Page=page
          NbItemsPerPage=nbItemsPerPage }

    let toJson (pagination:Pagination.T) =
        JsonValue.Record (Array.concat [ [| ("selfUrl", JsonValue.String pagination.SelfUrl) |] 
                                         pagination.PrevUrl |> Option.toArray |> Array.map (fun p -> ("prevUrl", JsonValue.String p))
                                         pagination.NextUrl |> Option.toArray |> Array.map (fun n -> ("nextUrl", JsonValue.String n))
                                         [| ("startedTime", pagination.StartedTime.ToJson()) |] 
                                         [| ("nbItemsOnThisPage", JsonValue.Number(decimal(pagination.NbItemsOnThisPage))) |] 
                                         [| ("page", JsonValue.Number(decimal(pagination.Page))) |] 
                                         [| ("nbItemsPerPage", JsonValue.Number(decimal(pagination.NbItemsPerPage))) |]  ])

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let selfUrl = (Map.find "selfUrl" map).AsString()
            let prevUrl = (Map.tryFind "prevUrl" map) |> Option.map (fun j -> j.AsString())
            let nextUrl = (Map.tryFind "nextUrl" map) |> Option.map (fun j -> j.AsString())
            let startedTime = UTCDateTime.fromJson(Map.find "startedTime" map)
            let nbItemsOnThisPage = (Map.find "nbItemsOnThisPage" map).AsInteger()
            let page = (Map.find "page" map).AsInteger()
            let nbItemsPerPage = (Map.find "nbItemsPerPage" map).AsInteger()
            { SelfUrl=selfUrl
              PrevUrl=prevUrl
              NextUrl=nextUrl
              StartedTime=startedTime
              NbItemsOnThisPage=nbItemsOnThisPage
              Page=page
              NbItemsPerPage=nbItemsPerPage }
        | _ -> failwith "Invalid Json submitted for Pagination" 

    let toString (pagination:Pagination.T) =
        "Pagination(SelfUrl=" + pagination.SelfUrl +
                   ", PrevUrl=" + pagination.PrevUrl.ToString() +
                   ", NextUrl=" + pagination.NextUrl.ToString() +
                   ", StartedTime=" + pagination.StartedTime.ToString() +
                   ", NbItemsOnThisPage=" + pagination.NbItemsOnThisPage.ToString() +
                   ", Page=" + pagination.Page.ToString() +
                   ", NbItemsPerPage=" + pagination.NbItemsPerPage.ToString() + ")"