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

module rec UpdateStreamSquareForm =
    type T = { Id: string  
               Set: Set<Edit.T>  
               Remove: Set<RemovableField.T> } with 
        override self.ToString() = UpdateStreamSquareForm.toString(self)
        member self.ToJson() = UpdateStreamSquareForm.toJson(self)
        member self.ToBody() = Some(UpdateStreamSquareForm.toJson(self).ToString())

    module RemovableField =
        type T = DescriptionField
                 | ForeignDataField with 
            member self.ToJson() = RemovableField.toJson(self)

        let toString (removableField:RemovableField.T) =
            removableField.ToString()

        let toJson (removableField:RemovableField.T) =
            JsonValue.String(removableField.ToString())

        let fromJson (json:JsonValue) =
            match json with
            | JsonValue.String s -> fromString s
            | _ -> failwith "Invalid Json submitted for RemovableField" 

        let fromString (removableField:string) =
            match removableField with 
            | "DescriptionField" -> RemovableField.DescriptionField 
            | "ForeignDataField" -> RemovableField.ForeignDataField 
            | _  -> failwith ("Invalid valid value submitted for RemovableField. Got: " + removableField)

        let make (removableField:string) = fromString removableField

    module EditableField =
        type T = IsElasticField
                 | SizeField
                 | HookField
                 | DescriptionField
                 | ForeignDataField with 
            member self.ToJson() = EditableField.toJson(self)

        let toString (editableField:EditableField.T) =
            editableField.ToString()

        let toJson (editableField:EditableField.T) =
            JsonValue.String(editableField.ToString())

        let fromJson (json:JsonValue) =
            match json with
            | JsonValue.String s -> fromString s
            | _ -> failwith "Invalid Json submitted for EditableField" 

        let fromString (editableField:string) =
            match editableField with 
            | "IsElasticField" -> EditableField.IsElasticField 
            | "SizeField" -> EditableField.SizeField 
            | "HookField" -> EditableField.HookField 
            | "DescriptionField" -> EditableField.DescriptionField 
            | "ForeignDataField" -> EditableField.ForeignDataField 
            | _  -> failwith ("Invalid valid value submitted for EditableField. Got: " + editableField)

        let make (editableField:string) = fromString editableField

    module Edit = 
        type T = SetIsElastic of {| NewValue: bool |}
                 | SetSize of {| NewValue: StreamSquare.Size.T |}
                 | SetHook of {| NewValue: Hook.T |}
                 | SetDescription of {| NewValue: string |}
                 | SetForeignData of {| NewValue: string |} with 
            override self.ToString() = Edit.toString(self)
            member self.ToJson() = Edit.toJson(self)

        let toString = function
            | SetIsElastic s -> "Edit.SetIsElastic(NewValue=" + s.NewValue.ToString() + ")"
            | SetSize s -> "Edit.SetSize(NewValue=" + s.NewValue.ToString() + ")"
            | SetHook s -> "Edit.SetHook(NewValue=" + s.NewValue.ToString() + ")"
            | SetDescription s -> "Edit.SetDescription(NewValue=" + s.NewValue + ")"
            | SetForeignData s -> "Edit.SetForeignData(NewValue=" + s.NewValue + ")"

        let toJson = function 
            | SetIsElastic s -> JsonValue.Record [| ("isElastic", JsonValue.Boolean s.NewValue) |] 
            | SetSize s -> JsonValue.Record [| ("size", JsonValue.String(s.NewValue.ToString())) |] 
            | SetHook s -> JsonValue.Record [| ("hook", s.NewValue.ToJson()) |] 
            | SetDescription s -> JsonValue.Record [| ("description", JsonValue.String s.NewValue) |] 
            | SetForeignData s -> JsonValue.Record [| ("foreignData", JsonValue.String s.NewValue) |] 

    let make id set remove =
        { Id=id; Set=set; Remove=remove }

    let toString (form:UpdateStreamSquareForm.T) =
        "UpdateStreamSquareForm(Id=" + form.Id +
                               ", Set=" + form.Set.ToString() +
                               ", Remove=" + form.Remove.ToString() + ")"

    let toJson (form:UpdateStreamSquareForm.T) =
        JsonValue.Record [| ("id", JsonValue.String form.Id)
                            ("set", JsonValue.Array (form.Set |> Set.map(fun s -> s.ToJson()) |> Set.toArray))
                            ("remove", JsonValue.Array (form.Remove |> Set.map(fun r -> JsonValue.String(r.ToString())) |> Set.toArray)) |]