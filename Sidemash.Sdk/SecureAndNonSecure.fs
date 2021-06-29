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

module rec SecureAndNonSecure =
    type T = { Secure: string  
               NonSecureOn80: string  
               NonSecure: string } with 
        override self.ToString() = SecureAndNonSecure.toString(self)
        member self.ToJson() = SecureAndNonSecure.toJson(self)

    let make secure nonSecureOn80 nonSecure =
        { Secure=secure; NonSecureOn80=nonSecureOn80; NonSecure=nonSecure }

    let toJson (secure:SecureAndNonSecure.T) =
        JsonValue.Record [| ("secure", JsonValue.String secure.Secure)
                            ("nonSecureOn80", JsonValue.String secure.NonSecureOn80)
                            ("nonSecure", JsonValue.String secure.NonSecure) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let secure = (Map.find "secure" map).AsString()
            let nonSecureOn80 = (Map.find "nonSecureOn80" map).AsString()
            let nonSecure = (Map.find "nonSecure" map).AsString()
            { Secure=secure; NonSecureOn80=nonSecureOn80; NonSecure=nonSecure }
        | _ -> failwith "Invalid Json submitted for SecureAndNonSecure" 

    let toString (secure:SecureAndNonSecure.T) =
        "SecureAndNonSecure(Secure=" + secure.Secure +
                           ", NonSecureOn80=" + secure.NonSecureOn80 +
                           ", NonSecure=" + secure.NonSecure + ")"