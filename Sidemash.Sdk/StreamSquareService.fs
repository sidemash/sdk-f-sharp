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

module rec StreamSquareService =
    type T = { Auth: Auth.T } with 
        override self.ToString() = StreamSquareService.toString(self)
        member self.ToJson() = StreamSquareService.toJson(self)
        member self.Create (form:CreateStreamSquareForm.T) =
          Api.post ("/" + Api.version + "/stream-squares") [] [] (form.ToBody()) StreamSquare.fromJson self.Auth

        member self.Get (id:string) =
          Api.get ("/" + Api.version + "/stream-squares/" + id) [] [] StreamSquare.fromJson self.Auth

        member self.List (form:ListForm.T) =
          Api.list ("/" + Api.version + "/stream-squares") [] (ListForm.toQueryString form) StreamSquare.fromJson self.Auth

        member self.Update (form:UpdateStreamSquareForm.T) =
          Api.patch ("/" + Api.version + "/stream-squares/" + form.Id) [] [] (form.ToBody()) StreamSquare.fromJson self.Auth

        member self.Delete (id:string) =
          Api.delete ("/" + Api.version + "/stream-squares/" + id) [] [] None (fun _ -> ()) self.Auth

    let make auth =
        { Auth=auth }

    let toJson (service:StreamSquareService.T) =
        JsonValue.Record [| ("auth", service.Auth.ToJson()) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let auth = Auth.fromJson(Map.find "auth" map)
            { Auth=auth }
        | _ -> failwith "Invalid Json submitted for StreamSquareService" 

    let toString (service:StreamSquareService.T) =
        "StreamSquareService(Auth=" + service.Auth.ToString() + ")"