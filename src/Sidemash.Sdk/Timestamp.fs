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

module rec Timestamp =
    type T = { Seconds: int64 } with 
        override self.ToString() = Timestamp.toString(self)
        member self.ToJson() = Timestamp.toJson(self)

    let make seconds =
        { Seconds=seconds }

    let toJson (timestamp:Timestamp.T) =
        JsonValue.Record [| ("seconds", JsonValue.Number(decimal(timestamp.Seconds))) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let seconds = (Map.find "seconds" map).AsInteger64()
            { Seconds=seconds }
        | _ -> failwith "Invalid Json submitted for Timestamp" 

    let toString (timestamp:Timestamp.T) =
        "Timestamp(Seconds=" + timestamp.Seconds.ToString() + ")"