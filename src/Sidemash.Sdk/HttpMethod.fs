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

module rec HttpMethod =
    type T = GET
             | POST
             | PUT
             | DELETE
             | PATCH with 
        member self.ToJson() = HttpMethod.toJson(self)

    let toString (httpMethod:HttpMethod.T) =
        httpMethod.ToString()

    let toJson (httpMethod:HttpMethod.T) =
        JsonValue.String(httpMethod.ToString())

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.String s -> fromString s
        | _ -> failwith "Invalid Json submitted for HttpMethod" 

    let fromString (httpMethod:string) =
        match httpMethod with 
        | "GET" -> HttpMethod.GET 
        | "POST" -> HttpMethod.POST 
        | "PUT" -> HttpMethod.PUT 
        | "DELETE" -> HttpMethod.DELETE 
        | "PATCH" -> HttpMethod.PATCH 
        | _  -> failwith ("Invalid valid value submitted for HttpMethod. Got: " + httpMethod)

    let make (httpMethod:string) = fromString httpMethod