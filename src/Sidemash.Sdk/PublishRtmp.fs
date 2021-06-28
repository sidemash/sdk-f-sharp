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

module rec PublishRtmp =
    type T = { StreamKeyPrefix: string  
               Ip: SecureAndNonSecure.T  
               Domain: SecureAndNonSecure.T } with 
        override self.ToString() = PublishRtmp.toString(self)
        member self.ToJson() = PublishRtmp.toJson(self)

    let make streamKeyPrefix ip domain =
        { StreamKeyPrefix=streamKeyPrefix; Ip=ip; Domain=domain }

    let toJson (publishRtmp:PublishRtmp.T) =
        JsonValue.Record [| ("streamKeyPrefix", JsonValue.String publishRtmp.StreamKeyPrefix)
                            ("ip", publishRtmp.Ip.ToJson())
                            ("domain", publishRtmp.Domain.ToJson()) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let streamKeyPrefix = (Map.find "streamKeyPrefix" map).AsString()
            let ip = SecureAndNonSecure.fromJson(Map.find "ip" map)
            let domain = SecureAndNonSecure.fromJson(Map.find "domain" map)
            { StreamKeyPrefix=streamKeyPrefix; Ip=ip; Domain=domain }
        | _ -> failwith "Invalid Json submitted for PublishRtmp" 

    let toString (publishRtmp:PublishRtmp.T) =
        "PublishRtmp(StreamKeyPrefix=" + publishRtmp.StreamKeyPrefix +
                    ", Ip=" + publishRtmp.Ip.ToString() +
                    ", Domain=" + publishRtmp.Domain.ToString() + ")"