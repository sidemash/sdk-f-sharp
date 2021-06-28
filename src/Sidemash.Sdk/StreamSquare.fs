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

module rec StreamSquare =
    type T = { Id: string  
               Url: string  
               Status: InstanceStatus.T  
               IsElastic: bool  
               Size: StreamSquare.Size.T  
               PlayDomainName: option<string>  
               PublishDomainName: option<string>  
               Publish: Publish.T  
               Hook: Hook.T  
               Description: option<string>  
               ForeignData: option<string> } with 
        override self.ToString() = StreamSquare.toString(self)
        member self.ToJson() = StreamSquare.toJson(self)

    let make id url status isElastic size playDomainName publishDomainName publish hook description foreignData =
        { Id=id
          Url=url
          Status=status
          IsElastic=isElastic
          Size=size
          PlayDomainName=playDomainName
          PublishDomainName=publishDomainName
          Publish=publish
          Hook=hook
          Description=description
          ForeignData=foreignData }

    module Size =
        type T = S
                 | M
                 | L
                 | XL
                 | XXL with 
            member self.ToJson() = Size.toJson(self)

        let toString (size:Size.T) =
            size.ToString()

        let toJson (size:Size.T) =
            JsonValue.String(size.ToString())

        let fromJson (json:JsonValue) =
            match json with
            | JsonValue.String s -> fromString s
            | _ -> failwith "Invalid Json submitted for Size" 

        let fromString (size:string) =
            match size with 
            | "S" -> Size.S 
            | "M" -> Size.M 
            | "L" -> Size.L 
            | "XL" -> Size.XL 
            | "XXL" -> Size.XXL 
            | _  -> failwith ("Invalid valid value submitted for Size. Got: " + size)

        let make (size:string) = fromString size

    let toJson (streamSquare:StreamSquare.T) =
        JsonValue.Record (Array.concat [ [| ("id", JsonValue.String streamSquare.Id) |] 
                                         [| ("url", JsonValue.String streamSquare.Url) |] 
                                         [| ("_type", JsonValue.String "StreamSquare" ) |] 
                                         [| ("status", streamSquare.Status.ToJson()) |] 
                                         [| ("isElastic", JsonValue.Boolean streamSquare.IsElastic) |] 
                                         [| ("size", JsonValue.String(streamSquare.Size.ToString())) |] 
                                         streamSquare.PlayDomainName |> Option.toArray |> Array.map (fun p -> ("playDomainName", JsonValue.String p))
                                         streamSquare.PublishDomainName |> Option.toArray |> Array.map (fun p -> ("publishDomainName", JsonValue.String p))
                                         [| ("publish", streamSquare.Publish.ToJson()) |] 
                                         [| ("hook", streamSquare.Hook.ToJson()) |] 
                                         streamSquare.Description |> Option.toArray |> Array.map (fun d -> ("description", JsonValue.String d))
                                         streamSquare.ForeignData |> Option.toArray |> Array.map (fun f -> ("foreignData", JsonValue.String f)) ])

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let id = (Map.find "id" map).AsString()
            let url = (Map.find "url" map).AsString()
            let status = InstanceStatus.fromJson(Map.find "status" map)
            let isElastic = (Map.find "isElastic" map).AsBoolean()
            let size = StreamSquare.Size.fromJson(Map.find "size" map)
            let playDomainName = (Map.tryFind "playDomainName" map) |> Option.map (fun j -> j.AsString())
            let publishDomainName = (Map.tryFind "publishDomainName" map) |> Option.map (fun j -> j.AsString())
            let publish = Publish.fromJson(Map.find "publish" map)
            let hook = Hook.fromJson(Map.find "hook" map)
            let description = (Map.tryFind "description" map) |> Option.map (fun j -> j.AsString())
            let foreignData = (Map.tryFind "foreignData" map) |> Option.map (fun j -> j.AsString())
            { Id=id
              Url=url
              Status=status
              IsElastic=isElastic
              Size=size
              PlayDomainName=playDomainName
              PublishDomainName=publishDomainName
              Publish=publish
              Hook=hook
              Description=description
              ForeignData=foreignData }
        | _ -> failwith "Invalid Json submitted for StreamSquare" 

    let toString (streamSquare:StreamSquare.T) =
        "StreamSquare(Id=" + streamSquare.Id +
                     ", Url=" + streamSquare.Url +
                     ", Status=" + streamSquare.Status.ToString() +
                     ", IsElastic=" + streamSquare.IsElastic.ToString() +
                     ", Size=" + streamSquare.Size.ToString() +
                     ", PlayDomainName=" + streamSquare.PlayDomainName.ToString() +
                     ", PublishDomainName=" + streamSquare.PublishDomainName.ToString() +
                     ", Publish=" + streamSquare.Publish.ToString() +
                     ", Hook=" + streamSquare.Hook.ToString() +
                     ", Description=" + streamSquare.Description.ToString() +
                     ", ForeignData=" + streamSquare.ForeignData.ToString() + ")"