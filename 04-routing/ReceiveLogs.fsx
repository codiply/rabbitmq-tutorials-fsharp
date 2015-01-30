#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()

    channel.ExchangeDeclare("direct_logs", "direct")
    let queueName = channel.QueueDeclare().QueueName

    let args = Environment.GetCommandLineArgs()

    if args.Length < 3
    then 
         Console.Error.WriteLine("Usage: fsi ReceiveLogs.fsx [info] [warning] [error]")
         Environment.ExitCode <- 1
    else 
        let severities = args |> Seq.skip 2
        severities 
        |> Seq.iter (fun sev ->
            channel.QueueBind(queueName, "direct_logs", sev))
        
        printfn " [*] Waiting for messages. To exit press CTRL+C"

        let consumer = new QueueingBasicConsumer(channel)
        channel.BasicConsume(queueName, true, consumer) |> ignore

        let rec loop () : unit =
            let ea = consumer.Queue.Dequeue();

            let body = ea.Body
            let message = Encoding.UTF8.GetString(body)
            let routingKey = ea.RoutingKey;
            printfn " [x] Received '%s':'%s'" routingKey message
         
            loop ()

        loop ()
)
