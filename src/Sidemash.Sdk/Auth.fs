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

module rec Auth =
    type T = { Token: string  
               SecretKey: string } with 
        override self.ToString() = Auth.toString(self)
        member self.ToJson() = Auth.toJson(self)

    let make token secretKey =
        { Token=token; SecretKey=secretKey }

    let toJson (auth:Auth.T) =
        JsonValue.Record [| ("token", JsonValue.String auth.Token)
                            ("secretKey", JsonValue.String auth.SecretKey) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let token = (Map.find "token" map).AsString()
            let secretKey = (Map.find "secretKey" map).AsString()
            { Token=token; SecretKey=secretKey }
        | _ -> failwith "Invalid Json submitted for Auth" 

    let toString (auth:Auth.T) =
        "Auth(Token=" + auth.Token +
             ", SecretKey=******" + ")"