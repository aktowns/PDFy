namespace Tests.PDFy

open System
open PDFy
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Given a pdf file with annotations`` () = 
    let pdfdoc = PDFy.PDFDocument.openDocument "Test1.pdf"
    
    [<Test>] 
    member x.``when i try to open an existing file it returns a pdfDocument`` ()=
        pdfdoc.GetType() = typeof<MonoMac.PdfKit.PdfDocument> |> should be True
    
    [<Test>]
    member x.``a document should contain at least one page`` () =
        pdfdoc.PageCount |> should be (greaterThanOrEqualTo 1)
