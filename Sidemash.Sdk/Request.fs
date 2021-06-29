namespace Sidemash.Sdk

open System

type private Request =
    { Nonce: int64
      SignedHeaders: (string * string) list
      HttpMethod: string
      Path: string
      QueryString: (string * string) list
      BodyHash: string option }

module private Request =
    let currentTimeMillis =
        let now = DateTime.UtcNow

        let epoch =
            DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)

        Convert.ToInt64((now - epoch).TotalMilliseconds)

    let make signedHeaders httpMethod path queryString bodyHash =
        { Nonce = currentTimeMillis
          SignedHeaders = signedHeaders
          HttpMethod = httpMethod
          Path = path
          QueryString = queryString
          BodyHash = bodyHash }

    let toMessage req =
        req.Nonce.ToString()
        + (req.SignedHeaders
           |> List.map (fun h -> fst h + ":" + snd h)
           |> String.concat "")
        + req.HttpMethod
        + req.Path
        + (if req.QueryString.IsEmpty then ""
           else
               "?"
               + (req.QueryString
                  |> List.map (fun h -> fst h + "=" + snd h)
                  |> String.concat "&"))
        + Option.defaultValue "" req.BodyHash
