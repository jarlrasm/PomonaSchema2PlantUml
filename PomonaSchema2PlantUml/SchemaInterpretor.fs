module SchemaInterpretor
open FSharp.Data
open System.Net
open System
open System.IO

let schemaUri baseuri= baseuri+"/schemas"

let propertyType (prop:JsonValue) = 
    let propertyName=(prop.GetProperty "type").AsString() 
    match propertyName with
    | "array" -> (Array.last ((prop.GetProperty "items").AsArray())).GetProperty("type").AsString() + "[]" //Jaysus
    | _ -> propertyName

let toPlantProperty ((name:string), (schemaProp:JsonValue))=
    "+" +
    name +
    ":" +
    (propertyType schemaProp)

let toPlantProperties (schemaProp:JsonValue)=
    schemaProp.Properties() |> Array.map toPlantProperty |> String.concat "\n"

let nameIfDifferentFromTypeName (name:string) (typename:string) = 
    if(name.ToLower().Equals(typename.ToLower())) then "" else " : "+name

let propertyName (prop:JsonValue) = (propertyType prop).Replace("[]","") 
let toPlantRelation ((name:string), (schemaProp:JsonValue)) typename=
    typename +
    " --> " +
    (propertyName schemaProp) +
    nameIfDifferentFromTypeName name (propertyName schemaProp) 
    
let isDefined ((name:string), (schemaProp:JsonValue)) (allTypenames:string[])=
    Array.exists (fun x->x.Equals (propertyType schemaProp) || (x+"[]").Equals (propertyType schemaProp)) allTypenames//Ughh

let toPlantRelations (schemaProp:JsonValue) allTypenames typename=
    schemaProp.Properties()|> Array.filter (fun x->isDefined x allTypenames)|> Array.map (fun x->toPlantRelation x typename) |> String.concat "\n"
    
let name (t:JsonValue)=t.GetProperty("name").AsString()

let toPlantAggregation typename (schemaProp:JsonValue) =
     schemaProp.AsString() + " o-- " + typename

let toPlantAggregations schemaProp typename=
    let toAggregation= toPlantAggregation typename
    match schemaProp with
    | Some extendProperty -> toAggregation extendProperty
    | _ -> ""

let toPlantClass (schemaClass:JsonValue) allTypenames=
    let classname=name schemaClass
    "class " +
        classname +
        "{\n"+ 
        toPlantProperties (schemaClass.GetProperty "properties") +
        "\n}\n"+
        toPlantRelations (schemaClass.GetProperty "properties") allTypenames classname +
        "\n\n" +
        toPlantAggregations (schemaClass.TryGetProperty "extends")  classname
        

let data pomonaUri= 
    let req = WebRequest.Create(Uri(schemaUri pomonaUri)) 
    use resp = req.GetResponse() 
    use stream = resp.GetResponseStream() 
    JsonValue.Load(stream)

let allTypenames (types:JsonValue[])=
    Array.map name  (types)

let toPlantUml  types=
    Array.map (fun x-> toPlantClass x (allTypenames  types)) types|> String.concat "\n"
    
let isIgnored (schemaClass:JsonValue) ignoredClasses=
    Array.exists (fun x->x.Equals(name schemaClass)) ignoredClasses

let loadPomona pomonaUri (ignoredClasses:string[])=
    "@startuml\n" +
    ((data pomonaUri).GetProperty("types").AsArray() |> Array.filter (fun x->not (isIgnored x ignoredClasses)) |> toPlantUml) +
    "\n@enduml\n"

