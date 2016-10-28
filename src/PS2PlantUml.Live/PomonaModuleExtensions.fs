namespace PS2Plant
[<System.Runtime.CompilerServices.Extension>]   
module PomonaExtensions =
    open SchemaInterpretor
    let pomonaSchema(pomonaModule:Pomona.PomonaModule)=
            let schduleRoute = pomonaModule.Routes|> Seq.filter (fun x-> x.Description.Name="Pomona.Metadata.JsonSchema") |> Seq.head
            let result=schduleRoute.Invoke(Nancy.DynamicDictionary(),System.Threading.CancellationToken.None).Result :?> Nancy.Response
            use stream = new System.IO.MemoryStream()
            result.Contents.Invoke(stream)
            stream.Position<-int64(0)
            let streamreader = new System.IO.StreamReader(stream)
            streamreader.ReadToEnd()

    [<System.Runtime.CompilerServices.Extension>] 
    let PlantUml(pomonaModule:Pomona.PomonaModule, route:string, configuration:Config) =
        pomonaModule.Get.[route] <- fun _ ->
            let schema = pomonaSchema pomonaModule
            parse configuration schema:>obj

            


