#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System
open System.IO
open PDFy

let filename, output = 
    let args = System.Environment.GetCommandLineArgs() 
    if args.Length < 6 then failwith (sprintf "%s requires 2 arguments a json file and an output file" __SOURCE_FILE__)
    (args.[4], args.[5])

cocoaInit()

let json = File.ReadAllText(filename)
let doc = JSONUtils.documentFromJSON json
saveDocument output doc