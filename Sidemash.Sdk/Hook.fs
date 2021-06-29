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

module rec Hook = 
    type T = HttpCall of {| Method: HttpMethod.T; Url: string |}
             | WsCall of {| Method: HttpMethod.T; Url: string |} with 
        override self.ToString() = Hook.toString(self)
        member self.RemoteType() = Hook.remoteType(self)
        member self.ToJson() = Hook.toJson(self)

    let toString = function
        | HttpCall h -> "Hook.HttpCall(Method=" + h.Method.ToString() + ", Url=" + h.Url + ")"
        | WsCall w -> "Hook.WsCall(Method=" + w.Method.ToString() + ", Url=" + w.Url + ")"

    let toJson = function 
            | HttpCall hook -> JsonValue.Record [| ("_type", JsonValue.String "Hook.HttpCall")
                                                   ("method", hook.Method.ToJson())
                                                   ("url", JsonValue.String hook.Url) |]
            | WsCall hook -> JsonValue.Record [| ("_type", JsonValue.String "Hook.WsCall")
                                                 ("method", hook.Method.ToJson())
                                                 ("url", JsonValue.String hook.Url) |]

    let remoteType = function
        | HttpCall _ -> "Hook.HttpCall" 
        | WsCall _ -> "Hook.WsCall" 

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let _type = (Map.find "_type" map).AsString()
            match _type with
            | "Hook.HttpCall" -> 
                let method = HttpMethod.fromJson(Map.find "method" map)
                let url = (Map.find "url" map).AsString()
                HttpCall {| Method=method; Url=url |}
            | "Hook.WsCall" -> 
                let method = HttpMethod.fromJson(Map.find "method" map)
                let url = (Map.find "url" map).AsString()
                WsCall {| Method=method; Url=url |}
            | _ -> failwith ("Invalid _type submitted for Hook. Got: " + _type) 
        | _ -> failwith "Invalid Json submitted for Hook" 