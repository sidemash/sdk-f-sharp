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

module rec UTCDateTime =
    type T = { Iso8601: string  
               Timestamp: Timestamp.T } with 
        override self.ToString() = UTCDateTime.toString(self)
        member self.ToJson() = UTCDateTime.toJson(self)

    let make iso8601 timestamp =
        { Iso8601=iso8601; Timestamp=timestamp }

    let toJson (dateTime:UTCDateTime.T) =
        JsonValue.Record [| ("iso8601", JsonValue.String dateTime.Iso8601)
                            ("timestamp", dateTime.Timestamp.ToJson()) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let iso8601 = (Map.find "iso8601" map).AsString()
            let timestamp = Timestamp.fromJson(Map.find "timestamp" map)
            { Iso8601=iso8601; Timestamp=timestamp }
        | _ -> failwith "Invalid Json submitted for UTCDateTime" 

    let toString (dateTime:UTCDateTime.T) =
        "UTCDateTime(Iso8601=" + dateTime.Iso8601 +
                    ", Timestamp=" + dateTime.Timestamp.ToString() + ")"