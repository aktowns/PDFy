module PDFy

open System
open System.Text.RegularExpressions
open System.Threading
open System.Drawing
open MonoMac
open MonoMac.AppKit
open MonoMac.Foundation
open MonoMac.PdfKit

let cocoaInit() =
    NSApplication.Init()
    let thread = new Thread(new ThreadStart(fun () -> NSRunLoop.Current.Run() ))
    thread.Start()

let getPDF (file: string) : PdfDocument =
    new PdfDocument(NSUrl.FromFilename(file))

let setPDF (file: string) (data: NSData) : bool = 
    (new PdfDocument(data)).Write(file)

let pdfPages (doc: PdfDocument) : List<PdfPage> = 
    List.init doc.PageCount (doc.GetPage)

let getPDFPages = getPDF >> pdfPages

let pdfAnnotations (page: PdfPage) : List<PdfAnnotation>  =  
    page.Annotations |> List.ofArray

let getPDFAnnotations (file: string) = 
    getPDFPages file
    |> List.map(pdfAnnotations)
    |> List.concat

let tryDowncastLinkAnnotation (anno: PdfAnnotation) : option<PdfAnnotationLink> =
    try Some((anno :?> PdfAnnotationLink)) with | _ -> None

let findLinkAnnotations (annos: List<PdfAnnotation>) : List<PdfAnnotationLink> =
    annos |> List.choose (tryDowncastLinkAnnotation)

let renameAnnotationDestination (newname: string) (annotation: PdfAnnotationLink) = 
    annotation.Url <- new NSUrl(newname)
    annotation

let annotationsNamed name (annos: List<PdfAnnotationLink>) : List<PdfAnnotationLink> =
    annos |> List.filter(fun annotation -> annotation.Url.AbsoluteString = name)

let annotationsStartingWith name (annos: List<PdfAnnotationLink>) : List<PdfAnnotationLink> =
    annos |> List.filter(fun annotation -> annotation.Url.AbsoluteString.StartsWith(name))

let annotationsMatchingRegex (regex: Regex) (annos: List<PdfAnnotationLink>) : List<PdfAnnotationLink> =
    annos |> List.filter(fun annotation -> regex.IsMatch(annotation.Url.AbsoluteString))
    
let findLinkAnnotationsNamed search = 
    findLinkAnnotations >> annotationsNamed search

let findLinkAnnotationsStartingWith term = 
    findLinkAnnotations >> annotationsStartingWith term

let findLinkAnnotationsMatchingRegex regex =
    findLinkAnnotations >> annotationsMatchingRegex regex

let findLinkAnnotationsMatchingRegex' regexStr =
    findLinkAnnotations >> annotationsMatchingRegex (new Regex(regexStr))

let createAnnotation (content: string) rect =
    let x, y, width, height = rect
    let url = new NSUrl(content)
    let annotation = new PdfAnnotationLink()
    annotation.Bounds <- new RectangleF(x, y, width, height)
    annotation.Url <- url
    annotation

let addAnnotation (annotation: PdfAnnotationLink) (page: PdfPage) =
    page.AddAnnotation(annotation)

let removeAnnotation (page: PdfPage) annotation =
    page.RemoveAnnotation(annotation)

let indexForPage (page: PdfPage) = page.Document.GetPageIndex(page)