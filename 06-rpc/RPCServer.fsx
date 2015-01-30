#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    channel.QueueDeclare("rpc_queue", false, false, false, null) |> ignore
    channel.BasicQos(0u, 1us, false)
    let consumer = new QueueingBasicConsumer(channel)
    channel.BasicConsume("rpc_queue", false, consumer) |> ignore
    printfn " [x] Awaiting RPC requests"

    let rec fib (n : int) =
        match n with
        | 0 | 1 -> n
        | _ -> fib (n - 1) + fib (n - 2)

    let rec loop () : unit =
        let ea = consumer.Queue.Dequeue()
        
        let body = ea.Body
        let props = ea.BasicProperties
        let replyProps = channel.CreateBasicProperties()
        replyProps.CorrelationId <- props.CorrelationId

        let response = 
            try
                let message = Encoding.UTF8.GetString(body)
                let n = Int32.Parse(message)
                printfn " [.] fib(%s)" message
                fib(n).ToString()
            with
            | exn ->
                printfn " [.] %s" exn.Message
                ""
        let responseBytes = Encoding.UTF8.GetBytes(response)
        channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes)
        channel.BasicAck(ea.DeliveryTag, false)

        loop()

    loop ()
)
