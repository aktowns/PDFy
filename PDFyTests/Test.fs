namespace Tests.PDFy

open System
open PDFy
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Given a pdf file without annotations`` () = 
    do
        cocoaInit()
    
    let pdfdoc = PDFy.PDFDocument.openDocument "Test1.pdf"

    [<Test>] 
    member x.``when i try to open an existing file it returns a pdfDocument`` () =
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