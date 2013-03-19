#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System

let args = System.Environment.GetCommandLineArgs() 
if args.Length < 6 then failwith (sprintf "%s requires 2 arguments, pdffile and search regex" __SOURCE_FILE__)

let filename, searchterm = (args.[4], args.[5])

PDFy.cocoaInit()

PDFy.getPDFAnnotations filename
|> PDFy.findLinkAnnotationsMatchingRegex' searchterm
|> List.iter(fun x -> 
    printfn "Found: Page: %i with %s on %s" (PDFy.indexForPage x.Page) (x.Url.AbsoluteString) filename)