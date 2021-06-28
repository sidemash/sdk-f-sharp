namespace Sidemash.Sdk

open FSharp.Data
open System.Security.Cryptography
open System.Text
open System

module private rec Api  =
    
    type FromJson<'a> = JsonValue -> 'a
    type toJson<'a> = 'a-> JsonValue
    
    let sign (message: string) (privateKey: string) =
      use hmac = new HMACSHA512(Encoding.UTF8.GetBytes privateKey)
      hmac.ComputeHash(Encoding.UTF8.GetBytes message) |> Convert.ToBase64String

    let sha256 (message: string) =
        use sha = SHA256Managed.Create()
        sha.ComputeHash(Encoding.UTF8.GetBytes message) |> Convert.ToBase64String

    let computeSignedHeaders (body: string option) headers (auth: Auth.T) = 
        headers
        @ [
            "Accept", "application/json";
            "User-Agent", "Sdk F# " + version;
            "Authorization", "Bearer " + auth.Token
        ]
        @ (match body with | None -> [] | Some(_) -> [ "Content-Type",  "application/json" ])
 
    let call httpMethod path headers queryString body (converter : 'T FromJson) (auth: Auth.T) : 'T = 
       let url = host + path
       let signedHeaders = computeSignedHeaders body headers auth
       let bodyHash = Option.map sha256 body
       let request = Request.make signedHeaders httpMethod path queryString bodyHash 
       let allHeaders =
         signedHeaders @ [
           "X-Sdm-Nonce", request.Nonce.ToString()
           "X-Sdm-SignedHeaders", request.SignedHeaders |> List.map (fun h -> fst h) |> String.concat ", "  
           "X-Sdm-Signature", "SHA512 " + (sign (Request.toMessage request) auth.SecretKey)
         ]
       let response =
           match body with
           | None -> Http.RequestString(url, queryString, allHeaders, httpMethod)
           | Some(b) -> Http.RequestString(url, queryString, allHeaders, httpMethod, body = HttpRequestBody.TextRequest(b))
       converter(JsonValue.Parse(response))
  
    let get path headers queryString converter auth         = call "GET"    path headers queryString None converter auth 
    let list path headers queryString converter auth        = call "GET"    path headers queryString None (RestCollection.fromJson converter) auth 
    let post path headers queryString body converter auth   = call "POST"   path headers queryString body converter auth 
    let patch path headers queryString body converter auth  = call "PATCH"  path headers queryString body converter auth 
    let put path headers queryString body converter auth    = call "PUT"    path headers queryString body converter auth 
    let delete path headers queryString body converter auth = call "DELETE" path headers queryString body converter auth
    
    
    let host = "http://dev-api.sidemash.io"
    let version = "v1.0"
