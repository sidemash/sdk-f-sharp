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

module rec StreamMetaData =
    type T = { Todo: int32 } with 
        override self.ToString() = StreamMetaData.toString(self)
        member self.ToJson() = StreamMetaData.toJson(self)

    let make todo =
        { Todo=todo }

    let toJson (data:StreamMetaData.T) =
        JsonValue.Record [| ("todo", JsonValue.Number(decimal(data.Todo))) |]

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let todo = (Map.find "todo" map).AsInteger()
            { Todo=todo }
        | _ -> failwith "Invalid Json submitted for StreamMetaData" 

    let toString (data:StreamMetaData.T) =
        "StreamMetaData(Todo=" + data.Todo.ToString() + ")"