namespace Tests.PDFy

open System
open PDFy
open NUnit.Framework
open FsUnit

[<SetUpFixture>]
type Config () =
    [<SetUp>]
    member x.Setup () =
        cocoaInit() 
    
    [<TearDown>]
    member x.TearDown () =
        ()

[<TestFixture>]
type ``Given a pdf file without annotations`` () =     
    let pdfdoc = PDFy.PDFDocument.openDocument "TestAssets/Test1.pdf"

    [<Test>] 
    member x.``when I try to open an existing file it returns a pdfDocument`` () =
        pdfdoc.GetType() = typeof<MonoMac.PdfKit.PdfDocument> |> should be True
    
    [<Test>]
    member x.``a document should contain at least one page`` () =
        pdfdoc.PageCount |> should be (greaterThanOrEqualTo 1)

    [<Test>]
    member x.``Annotations should return a List<PDFAnnotation> of zero`` () =
        (getAnnotationsFromDocument pdfdoc).GetType() = typeof<List<PDFAnnotation>> 
        |> should be True
        
        List.length (getAnnotationsFromDocument pdfdoc)
        |> should equal 0

[<TestFixture>]
type ``Given a pdf file with some annotations`` () =     
    let pdfdoc = PDFy.PDFDocument.openDocument "TestAssets/Test2.pdf"
    
    [<Test>]
    member x.``when I try to open an existing file it returns a pdfDocument`` () = 
        pdfdoc.GetType() = typeof<MonoMac.PdfKit.PdfDocument> |> should be True

    [<Test>]
    member x.``a document should contain at least one page`` () =
        pdfdoc.PageCount |> should be (greaterThanOrEqualTo 1)

    [<Test>]
    member x.``Annotations should return a List<PDFAnnotation> of annotations`` () =
        (getAnnotationsFromDocument pdfdoc).GetType() = typeof<List<PDFAnnotation>> 
        |> should be True
        
        List.length (getAnnotationsFromDocument pdfdoc)
        |> should be (greaterThanOrEqualTo 1)
    
    [<Test>]
    member x.``I should be able to filter annotations by a string`` () =
        (getAnnotationsFromDocument pdfdoc) 
        |> List.filter(PDFAnnotation.hotspotNamed "TestHotspot")
        |> List.length
        |> should be (greaterThanOrEqualTo 1)        
        
    [<Test>]
    member x.``I should be able to filter annotations by a regex`` () =
        (getAnnotationsFromDocument pdfdoc) 
        |> List.filter(PDFAnnotation.hotspotRegex (new System.Text.RegularExpressions.Regex("^TestHotspot$")))
        |> List.length
        |> should be (greaterThanOrEqualTo 1) 
    
    [<Test>]
    member x.``I Should be able to rename a hotspot`` () = 
        let annotation = 
            (getAnnotationsFromDocument pdfdoc) 
            |> List.find(PDFAnnotation.hotspotNamed "TestHotspot")
        annotation.hotspot <- "NewHotspot"
        
        let result = 
            (getAnnotationsFromDocument pdfdoc) 
            |> List.tryFind(PDFAnnotation.hotspotNamed "NewHotspot")
        result.IsSome |> should be True
        