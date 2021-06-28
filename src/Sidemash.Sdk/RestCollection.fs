namespace Sidemash.Sdk
open FSharp.Data

module rec RestCollection =
    type T<'a> = { Url: string  
                   Pagination: Pagination.T  
                   Items: List<'a> } with 
        override self.ToString() = RestCollection.toString(self)
        member self.ToJson(converter: 'a -> JsonValue) = RestCollection.toJson converter self

    let make url pagination items =
        { Url=url; Pagination=pagination; Items=items }

    let toJson (converter: 'a -> JsonValue) restCollection =
        JsonValue.Record [| ("url", JsonValue.String restCollection.Url)
                            ("pagination", restCollection.Pagination.ToJson())
                            ("items", JsonValue.Array(restCollection.Items |> List.map(converter) |> List.toArray)) |]

    let fromJson converter = function 
        | JsonValue.Record array ->
            let map = Map.ofArray array
            let url = (Map.find "url" map).AsString()
            let pagination = Pagination.fromJson(Map.find "pagination" map)
            let items =
                match (Map.find "items" map) with
                | JsonValue.Array array -> array |> Array.map(converter) |> Array.toList
                | _ -> failwith "Invalid 'items' submitted for RestCollection" 
            { Url=url; Pagination=pagination; Items=items }
        | _ -> failwith "Invalid Json submitted for RestCollection" 

    let toString (restCollection:RestCollection.T<'a>) =
        "RestCollection(Url=" + restCollection.Url +
                       ", Pagination=" + restCollection.Pagination.ToString() +
                       ", Items=" + restCollection.Items.ToString() + ")"