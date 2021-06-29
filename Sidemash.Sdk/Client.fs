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

module rec Client =
    type T = { Auth: Auth.T  
               StreamSquare: StreamSquareService.T } with 
        override self.ToString() = Client.toString(self)
        member self.ToJson() = Client.toJson(self)

    let make auth = 
        { Auth = auth
          StreamSquare = StreamSquareService.make(auth) }

    let toJson (client:Client.T) =
        JsonValue.Record [| ("auth", client.Auth.ToJson())
                            ("streamSquare", client.StreamSquare.ToJson()) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let auth = Auth.fromJson(Map.find "auth" map)
            let streamSquare = StreamSquareService.fromJson(Map.find "streamSquare" map)
            { Auth=auth; StreamSquare=streamSquare }
        | _ -> failwith "Invalid Json submitted for Client" 

    let toString (client:Client.T) =
        "Client(Auth=" + client.Auth.ToString() +
               ", StreamSquare=" + client.StreamSquare.ToString() + ")"