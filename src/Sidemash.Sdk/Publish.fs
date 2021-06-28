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

module rec Publish =
    type T = { Rtmp: PublishRtmp.T } with 
        override self.ToString() = Publish.toString(self)
        member self.ToJson() = Publish.toJson(self)

    let make rtmp =
        { Rtmp=rtmp }

    let toJson (publish:Publish.T) =
        JsonValue.Record [| ("rtmp", publish.Rtmp.ToJson()) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let rtmp = PublishRtmp.fromJson(Map.find "rtmp" map)
            { Rtmp=rtmp }
        | _ -> failwith "Invalid Json submitted for Publish" 

    let toString (publish:Publish.T) =
        "Publish(Rtmp=" + publish.Rtmp.ToString() + ")"