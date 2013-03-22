# PDFy
WIP, some small utils for dealing with pdfs in f# using PDFKit   

### Example

to read all annotations, filter hotspots named `Title1` and print  
```fsharp
cocoaInit()

(openDocument >> getAnnotationsFromDocument) "test.pdf"
|> List.filter(PDFAnnotation.isHotspot) 
|> List.filter(PDFAnnotation.hotspotRegex (new Regex("^Title1$")))
|> List.iter (fun x -> printfn "Found: Page: %i with %s on test.pdf." (x.index) (x.hotspot))
```

to rename a set of hotspots 
```fsharp
(openDocument >> searchForAnnotations searchterm) "test.pdf"
|> List.map(fun annotation -> 
    annotation.hotspot <- replacewith
    printfn "Replaced: %s with %s on page %i of test.pdf" searchterm replacewith annotation.index
    annotation.document)
|> List.head
|> saveDocument output
```

### Scripts
inside the PDFy directory there is four scripts, `pdfgrep.fsx`, `pdfreplace.fsx`, `pdftojson.fsx` and `pdffromjson.fsx`

`pdftojson.fsx` and `pdffromjson.fsx` serialize the annotations on a pdf, and are able to create new pdf's from the exported json.


`pdfgrep` accepts a filename and regex and returns the results per page of hits, ie
```shell
PDFy/pdfgrep.fsx ~/Desktop/wah.pdf ".*"
Found: Page: 0 with OtherStoryImage1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage2 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage3 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage4 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage5 on /Users/ashleyis/Desktop/wah.pdf
...
```

`pdfreplace` accepts a input filename, output filename, a search term and the replacement string and returns the results of replaced items, ie
```shell
PDFy/pdfreplace.fsx ~/Desktop/wah.pdf test.pdf OtherStoryTitle1 OtherStoryBigTitle1
Replaced: OtherStoryTitle1 with OtherStoryBigTitle1 on page 0 of /Users/ashleyis/Desktop/wah.pdf
```
