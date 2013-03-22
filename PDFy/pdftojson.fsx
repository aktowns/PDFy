#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System
open PDFy

let filename = 
    let args = System.Environment.GetCommandLineArgs() 
    if args.Length < 5 then failwith (sprintf "%s requires 1 argument a pdf file" __SOURCE_FILE__)
    args.[4]

cocoaInit()

let toJson = openDocument >> JSONUtils.documentToJSON
printfn "%s" (toJson filename)

