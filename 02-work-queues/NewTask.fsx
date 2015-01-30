#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

let getMessage (args : string array) =
    if Array.length args > 2
    then String.Join(" ", args |> Seq.skip 2)
    else "Hello World!"

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    let durable = true
    channel.QueueDeclare("task_queue", durable, false, false, null) |> ignore
    
    let message = getMessage (Environment.GetCommandLineArgs())
    let body = Encoding.UTF8.GetBytes(message)

    let properties = channel.CreateBasicProperties()
    properties.DeliveryMode <- 2uy
    properties.SetPersistent(true)

    channel.BasicPublish("", "task_queue", properties, body)
    printfn " [x] Sent %s" message
)
