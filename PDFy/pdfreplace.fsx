#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System
open PDFy

let filename, output, searchterm, replacewith = 
    let args = System.Environment.GetCommandLineArgs() 
    if args.Length < 6 then 
        failwith (sprintf "%s requires 4 arguments, pdffile, outputfile, search string and a replace string" __SOURCE_FILE__)
    (args.[4], args.[5], args.[6], args.[7])

cocoaInit()

(openDocument >> getAnnotationsFromDocument) filename
|> List.filter(PDFAnnotation.isHotspot)
|> List.filter(PDFAnnotation.hotspotNamed searchterm)
|> List.map(fun annotation -> 
    annotation.hotspot <- replacewith
    printfn "Replaced: %s with %s on page %i of %s" searchterm replacewith annotation.index filename
    annotation.document)
|> List.head
|> saveDocument output