open SchemaInterpretor


[<EntryPoint>]
let main argv = 
    printfn  "%s" (loadPomona (Array.head argv) (Array.skip 1 argv))
    0
