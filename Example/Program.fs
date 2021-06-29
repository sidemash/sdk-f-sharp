open Sidemash.Sdk
          
[<EntryPoint>]
let main argv =
    let auth = Auth.make "1342" "****"
    let sdm = Client.make auth

    // let domain = sdm.Domain.Create(CreateDomainForm.make "example695.com" Domain.Play (Some "My example domain name") None)

    // printfn "%s" (domain.ToString())
    // printfn "%s" (Some(Domain.Play).ToString())
    // printfn "%s" (Some(Domain.Play).ToString())
    printfn "%s" (auth.ToString())
    0 // return an integer exit code
