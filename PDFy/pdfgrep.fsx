#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System
open System.Text.RegularExpressions
open PDFy

let filename, searchterm = 
    let args = System.Environment.GetCommandLineArgs() 
    if args.Length < 6 then failwith (sprintf "%s requires 2 arguments, pdffile and search regex" __SOURCE_FILE__)
    (args.[4], args.[5])

cocoaInit()

(openDocument >> searchForAnnotationsWithRegex (new Regex(searchterm))) filename
|> List.iter (fun x -> printfn "Found: Page: %i with %s on %s." (x.index) (x.hotspot) filename)