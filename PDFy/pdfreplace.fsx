#light (*
    exec fsharpi --debug --exec $0 --quiet $@
*)

#I "./bin/Debug/"
#r "MonoMac.dll"
#r "PDFy.dll"

open System

let args = System.Environment.GetCommandLineArgs() 
if args.Length < 6 then 
    failwith (sprintf "%s requires 4 arguments, pdffile, outputfile, search string and a replace string" __SOURCE_FILE__)

let filename, output, searchterm, replacewith = (args.[4], args.[5], args.[6], args.[7])

PDFy.cocoaInit()

PDFy.getPDFAnnotations filename
|> PDFy.findLinkAnnotationsNamed searchterm
|> List.map(PDFy.renameAnnotationDestination replacewith)
|> List.map(fun x ->
    printfn "Replaced: %s with %s on page %i of %s" searchterm replacewith (PDFy.indexForPage x.Page) filename
    PDFy.documentForAnnotation x)
|> Seq.distinct
|> Seq.head
|> PDFy.setPDF output
