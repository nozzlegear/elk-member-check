module Http
open System.Net.Http

let private readBody (msg: HttpResponseMessage) = 
    msg.Content.ReadAsStringAsync()
    |> Async.AwaitTask
    |> Async.map (fun str -> str, msg)

let private toResult<'t> (body: string, msg: HttpResponseMessage) = 
    if not msg.IsSuccessStatusCode then 
        sprintf "Request to %A failed with status code%A: %s" msg.RequestMessage.RequestUri msg.StatusCode body 
        |> Result.Error 
    else 
        Newtonsoft.Json.JsonConvert.DeserializeObject<'t> body
        |> Result.Ok

/// Makes a request to the given URL and deserializes the body to a JSON object
let getJson<'t> (url: string) = 
    use client = new HttpClient()
    use requestMessage = new HttpRequestMessage(HttpMethod.Get, url)

    client.SendAsync requestMessage 
    |> Async.AwaitTask 
    |> Async.bind readBody
    |> Async.map toResult<'t>