#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    channel.QueueDeclare("hello", false, false, false, null) |> ignore
    
    let message = "Hello World!"
    let body = Encoding.UTF8.GetBytes(message)

    channel.BasicPublish("", "hello", null, body)
    printfn " [x] Sent %s" message
)
