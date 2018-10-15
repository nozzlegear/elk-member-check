module Async 

let map fn computation = async {
    let! result = computation 

    return fn result
}

let bind fn computation = async {
    let! computationResult = computation 
    let! bindResult = fn computationResult

    return bindResult
}

let wrap value = async { return value }