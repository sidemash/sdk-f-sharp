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

module rec CreateStreamSquareForm =
    type T = { IsElastic: bool  
               Size: StreamSquare.Size.T  
               Hook: Hook.T  
               Description: option<string>  
               ForeignData: option<string>  
               PlayDomainName: option<string>  
               PublishDomainName: option<string> } with 
        override self.ToString() = CreateStreamSquareForm.toString(self)
        member self.ToJson() = CreateStreamSquareForm.toJson(self)
        member self.ToBody() = Some(CreateStreamSquareForm.toJson(self).ToString())

    let make isElastic size hook description foreignData playDomainName publishDomainName =
        { IsElastic=isElastic
          Size=size
          Hook=hook
          Description=description
          ForeignData=foreignData
          PlayDomainName=playDomainName
          PublishDomainName=publishDomainName }

    let toJson (form:CreateStreamSquareForm.T) =
        JsonValue.Record (Array.concat [ [| ("isElastic", JsonValue.Boolean form.IsElastic) |] 
                                         [| ("size", JsonValue.String(form.Size.ToString())) |] 
                                         [| ("hook", form.Hook.ToJson()) |] 
                                         form.Description |> Option.toArray |> Array.map (fun d -> ("description", JsonValue.String d))
                                         form.ForeignData |> Option.toArray |> Array.map (fun f -> ("foreignData", JsonValue.String f))
                                         form.PlayDomainName |> Option.toArray |> Array.map (fun p -> ("playDomainName", JsonValue.String p))
                                         form.PublishDomainName |> Option.toArray |> Array.map (fun p -> ("publishDomainName", JsonValue.String p)) ])

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let isElastic = (Map.find "isElastic" map).AsBoolean()
            let size = StreamSquare.Size.fromJson(Map.find "size" map)
            let hook = Hook.fromJson(Map.find "hook" map)
            let description = (Map.tryFind "description" map) |> Option.map (fun j -> j.AsString())
            let foreignData = (Map.tryFind "foreignData" map) |> Option.map (fun j -> j.AsString())
            let playDomainName = (Map.tryFind "playDomainName" map) |> Option.map (fun j -> j.AsString())
            let publishDomainName = (Map.tryFind "publishDomainName" map) |> Option.map (fun j -> j.AsString())
            { IsElastic=isElastic
              Size=size
              Hook=hook
              Description=description
              ForeignData=foreignData
              PlayDomainName=playDomainName
              PublishDomainName=publishDomainName }
        | _ -> failwith "Invalid Json submitted for CreateStreamSquareForm" 

    let toString (form:CreateStreamSquareForm.T) =
        "CreateStreamSquareForm(IsElastic=" + form.IsElastic.ToString() +
                               ", Size=" + form.Size.ToString() +
                               ", Hook=" + form.Hook.ToString() +
                               ", Description=" + form.Description.ToString() +
                               ", ForeignData=" + form.ForeignData.ToString() +
                               ", PlayDomainName=" + form.PlayDomainName.ToString() +
                               ", PublishDomainName=" + form.PublishDomainName.ToString() + ")"