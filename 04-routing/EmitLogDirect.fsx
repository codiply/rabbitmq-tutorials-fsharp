#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    channel.ExchangeDeclare("direct_logs", "direct")

    let args = Environment.GetCommandLineArgs()
    let severity = if args.Length > 2 then args.[2] else "info"
    let message = if args.Length > 3 
                  then String.Join(" ", args |> Seq.skip 3)
                  else "Hello World!"
    let body = Encoding.UTF8.GetBytes(message)
    channel.BasicPublish("direct_logs", severity, null, body)
    printfn " [x] Sent '%s':'%s'" severity message
)
