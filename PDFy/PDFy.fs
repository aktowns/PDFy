module PDFy

open Microsoft.FSharp.Reflection
open System
open System.Text.RegularExpressions
open System.Threading
open System.Drawing
open MonoMac
open MonoMac.AppKit
open MonoMac.Foundation
open MonoMac.PdfKit

// So we're able to use the PDFKit methods
let cocoaInit() =
    NSApplication.Init()
    let thread = new Thread(new ThreadStart(fun () -> NSRunLoop.Current.Run() ))
    thread.Start()

type HotspotSize = { x: float32; y: float32; width: float32; height: float32 }

type PDFAnnotation =
    | Annotation of PdfAnnotation
    | ButtonAnnotation of PdfAnnotationButtonWidget
    | ChoiceAnnotation of PdfAnnotationChoiceWidget
    | CircleAnnotation of PdfAnnotationCircle
    | FreeTextAnnotation of PdfAnnotationFreeText
    | InkAnnotation of PdfAnnotationInk
    | LineAnnotation of PdfAnnotationLine
    | LinkAnnotation of PdfAnnotationLink
    | MarkupAnnotation of PdfAnnotationMarkup
    | PopupAnnotation of PdfAnnotationPopup
    | SquareAnnotation of PdfAnnotationSquare
    | StampAnnotation of PdfAnnotationStamp
    | TextAnnotation of PdfAnnotationText
    | TextWidgetAnnotation of PdfAnnotationTextWidget
    with
        member this.tryDowncastTo<'T when 'T :> PdfAnnotation> () = 
            match this with
            | Annotation pdfannotation -> PDFAnnotation.tryDowncastTo<'T> (pdfannotation)
            | _ -> None
            
        static member tryDowncastTo<'T when 'T :> PdfAnnotation> (annotation: PdfAnnotation) =
            try Some(annotation :?> 'T) with _ -> None
            
        member this.getBaseType : PdfAnnotation =
            match this with
            | Annotation(x) -> x 
            | ButtonAnnotation(x) -> x :> PdfAnnotation
            | ChoiceAnnotation(x) -> x :> PdfAnnotation 
            | CircleAnnotation(x) -> x :> PdfAnnotation
            | FreeTextAnnotation(x) -> x :> PdfAnnotation
            | InkAnnotation(x) -> x :> PdfAnnotation
            | LineAnnotation(x) -> x :> PdfAnnotation
            | LinkAnnotation(x) -> x :> PdfAnnotation
            | MarkupAnnotation(x) -> x :> PdfAnnotation
            | PopupAnnotation(x) -> x :> PdfAnnotation
            | SquareAnnotation(x) -> x :> PdfAnnotation
            | StampAnnotation(x) -> x :> PdfAnnotation
            | TextAnnotation(x) -> x :> PdfAnnotation
            | TextWidgetAnnotation(x) -> x :> PdfAnnotation
        
        static member inferFromObject (item: obj when 'T :> PdfAnnotation) =
            match item with 
            | :? PdfAnnotationButtonWidget as x -> ButtonAnnotation(x)
            | :? PdfAnnotationChoiceWidget as x -> ChoiceAnnotation(x)
            | :? PdfAnnotationCircle as x -> CircleAnnotation(x)
            | :? PdfAnnotationFreeText as x -> FreeTextAnnotation(x)
            | :? PdfAnnotationInk as x -> InkAnnotation(x)
            | :? PdfAnnotationLine as x -> LineAnnotation(x)
            | :? PdfAnnotationLink as x -> LinkAnnotation(x)
            | :? PdfAnnotationMarkup as x -> MarkupAnnotation(x)
            | :? PdfAnnotationPopup as x -> PopupAnnotation(x)
            | :? PdfAnnotationSquare as x -> SquareAnnotation(x)
            | :? PdfAnnotationStamp as x -> StampAnnotation(x)
            | :? PdfAnnotationText as x -> TextAnnotation(x)
            | :? PdfAnnotationTextWidget as x -> TextWidgetAnnotation(x)
            | :? PdfAnnotation as x -> Annotation(x)
            | _ -> failwithf "failed to infer type of %A as an annotation" item
        
        static member isHotspot = function
            | LinkAnnotation _ -> true | _ -> false
        
        member this.isHotspot' = PDFAnnotation.isHotspot this
        
        member this.size
            with get () = 
                let bounds = this.getBaseType.Bounds
                { x = bounds.X; y = bounds.Y; width = bounds.Width; height = bounds.Height}
            and set (value) = 
                this.getBaseType.Bounds <- new RectangleF(value.x, value.y, value.width, value.height)
        
        member this.hotspot
            with get () = 
                match this with
                | LinkAnnotation(x) -> x.Url.AbsoluteString
                | _ -> failwithf "unable to retrieve the name of annotation of type %A" (this.GetType())
            and set (value: string) =
                match this with
                | LinkAnnotation(x) -> x.Url <- new NSUrl(value)
                | _ -> failwithf "unable to change the name of annotation of type %A" (this.GetType())
        
        static member createHotspot rect text =
            let annotation = LinkAnnotation(new PdfAnnotationLink())
            annotation.hotspot <- text
            annotation.size <- rect
            annotation
            
        member this.page = this.getBaseType.Page
        member this.document = this.page.Document
        member this.index = this.document.GetPageIndex(this.page)
        
        member this.hotspotNamed' name = PDFAnnotation.hotspotNamed name this
        member this.hotspotRegex' regex = PDFAnnotation.hotspotRegex regex this
        
        static member hotspotNamed (name: string) (annotation: PDFAnnotation) = annotation.hotspot = name
        static member hotspotRegex (regex: Regex) (annotation: PDFAnnotation) = regex.IsMatch(annotation.hotspot)
        
            
            
[<AutoOpen>]
module PDFPage =
    let getAnnotationsFromPage (page: PdfPage) = 
        page.Annotations 
        |> List.ofArray 
        |> List.map(PDFAnnotation.inferFromObject)

    let indexForPage (page: PdfPage) = page.Document.GetPageIndex(page)

    let addAnnotation (annotation: PdfAnnotationLink) (page: PdfPage) = page.AddAnnotation(annotation)
        
    let removeAnnotation (page: PdfPage) annotation = page.RemoveAnnotation(annotation)

[<AutoOpen>]
module PDFDocument =                                                                    
    let openDocument (file: string) = 
        if IO.File.Exists(file) then 
            new PdfDocument(NSUrl.FromFilename(file)) 
        else failwithf "Failed to open file %s" file
    
    let saveDocument (file: string) (document: PdfDocument) : bool =
        (new PdfDocument(document.GetDataRepresentation())).Write(file)    

    let getPages (document: PdfDocument) =
        List.init document.PageCount (document.GetPage)
        
    let getAnnotationsFromDocument (document: PdfDocument) = 
        getPages document 
        |> List.map(getAnnotationsFromPage)
        |> List.concat
        
    let searchForAnnotations (str: string) (doc: PdfDocument) =
        let search = PDFAnnotation.hotspotNamed str
        getAnnotationsFromDocument doc 
        |> List.filter(PDFAnnotation.isHotspot)
        |> List.filter(search)
    
    let searchForAnnotationsWithRegex (regex: Regex) (doc: PdfDocument) =
        let search = PDFAnnotation.hotspotRegex regex
        getAnnotationsFromDocument doc 
        |> List.filter(PDFAnnotation.isHotspot)
        |> List.filter(search)