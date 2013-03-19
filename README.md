# PDFy
WIP, some small utils for dealing with pdfs in f# using PDFKit   

### Scripts
inside the PDFy directory theres two scripts, pdfgrep.fsx and pdfreplace.fsx   

pdfgrep accepts a filename and regex and returns the results per page of hits, ie
```shell
Ξ Projects/PDFy git:(master) ▶ PDFy/pdfgrep.fsx ~/Desktop/wah.pdf ".*"
Found: Page: 0 with OtherStoryImage1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage2 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage3 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage4 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage5 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryImage6 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory2 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle2 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory3 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle3 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory4 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle4 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory5 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryCategory6 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle5 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with OtherStoryTitle6 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with VIPS1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with Title1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with ad1 on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with goto,section+1,transition=SlideCoverFromRight on /Users/ashleyis/Desktop/wah.pdf
Found: Page: 0 with goto,section+2,transition=SlideCoverFromRight on /Users/ashleyis/Desktop/wah.pdf
```


pdfreplace accepts a input filename, output filename, a search term and the replacement string and returns the results of replaced items, ie
```shell
Ξ Projects/PDFy git:(master) ▶ PDFy/pdfreplace.fsx ~/Desktop/wah.pdf test.pdf OtherStoryTitle1 OtherStoryBigTitle1
Replaced: OtherStoryTitle1 with OtherStoryBigTitle1 on page 0 of /Users/ashleyis/Desktop/wah.pdf
```
