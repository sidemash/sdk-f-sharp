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

module rec InstanceStatus =
    type T = Initializing
             | Running
             | Restarting
             | Updating
             | Maintaining
             | PartiallyAvailable
             | NotAvailable
             | Terminating
             | Terminated with 
        member self.ToJson() = InstanceStatus.toJson(self)

    let toString (instanceStatus:InstanceStatus.T) =
        instanceStatus.ToString()

    let toJson (instanceStatus:InstanceStatus.T) =
        JsonValue.String(instanceStatus.ToString())

    let fromJson (json:JsonValue) =
        match json with
        | JsonValue.String s -> fromString s
        | _ -> failwith "Invalid Json submitted for InstanceStatus" 

    let fromString (instanceStatus:string) =
        match instanceStatus with 
        | "Initializing" -> InstanceStatus.Initializing 
        | "Running" -> InstanceStatus.Running 
        | "Restarting" -> InstanceStatus.Restarting 
        | "Updating" -> InstanceStatus.Updating 
        | "Maintaining" -> InstanceStatus.Maintaining 
        | "PartiallyAvailable" -> InstanceStatus.PartiallyAvailable 
        | "NotAvailable" -> InstanceStatus.NotAvailable 
        | "Terminating" -> InstanceStatus.Terminating 
        | "Terminated" -> InstanceStatus.Terminated 
        | _  -> failwith ("Invalid valid value submitted for InstanceStatus. Got: " + instanceStatus)

    let make (instanceStatus:string) = fromString instanceStatus