#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

let getMessage (args : string array) =
    if Array.length args > 2
    then String.Join(" ", args |> Seq.skip 2)
    else "info: Hello World!"

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    channel.ExchangeDeclare("logs", "fanout")

    let message = getMessage (Environment.GetCommandLineArgs())
    let body = Encoding.UTF8.GetBytes(message)

    channel.BasicPublish("logs", "", null, body)
    printfn " [x] Sent %s" message
)
