open SchemaInterpretor


[<EntryPoint>]
let main argv = 
    let uri=(Array.head argv)
    let ignoredClasses= (Array.skip 1 argv)|>Array.toList
    let config={IgnoredClasses =  ignoredClasses; Uri = uri}
    printfn  "%s" (loadPomona config)
    0
