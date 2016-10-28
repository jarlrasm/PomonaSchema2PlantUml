
open PS2Plant.SchemaInterpretor
open System.Net

let schemaUri baseuri= baseuri+"/schemas"

[<EntryPoint>]
let main argv = 
    let uri=(Array.head argv)
    let ignoredClasses= (Array.skip 1 argv)|>Array.toList
    let config={IgnoredClasses =  ignoredClasses;}
    let req = WebRequest.Create(System.Uri(schemaUri uri)) 
    use resp = req.GetResponse() 
    use stream = resp.GetResponseStream() 
    let contentreader=new System.IO.StreamReader(stream)
    let content=contentreader.ReadToEnd()
    printfn  "%s" (parse config content)
    0
