#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System

let args = System.Environment.GetCommandLineArgs() 
if args.Length < 6 then failwith (sprintf "%s requires 2 arguments, pdffile and search string" __SOURCE_FILE__)

let filename, searchterm = (args.[4], args.[5])

PDFy.cocoaInit()

let result = 
    (PDFy.getPDF >> PDFy.pdfPages) filename
    |> List.map(PDFy.pdfAnnotations)
    |> List.concat
    |> PDFy.findLinkAnnotations
    |> PDFy.findAnnotationsNamed searchterm
    |> List.map(fun x -> 
        (PDFy.indexForPage x.Page, x.Url.AbsoluteString, filename))

result 
|> List.iter(fun (pagenum, hotspot, filename) -> printfn "Found: Page: %i with %s on %s" pagenum hotspot filename)
