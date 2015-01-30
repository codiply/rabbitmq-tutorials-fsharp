#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open System.Text

type RPCClient() =
    let factory = new ConnectionFactory(HostName = "localhost")
    let connection = factory.CreateConnection()
    let channel = connection.CreateModel()
    let replyQueueName = channel.QueueDeclare().QueueName
    let consumer = new QueueingBasicConsumer(channel)
    do channel.BasicConsume(replyQueueName, true, consumer) |> ignore
    member this.Call (message : string) = 
        let corrId = Guid.NewGuid().ToString()
        let props = channel.CreateBasicProperties()
        props.ReplyTo <- replyQueueName
        props.CorrelationId <- corrId

        let messageBytes = Encoding.UTF8.GetBytes(message)
        channel.BasicPublish("", "rpc_queue", props, messageBytes)

        let rec loop () =
            let ea = consumer.Queue.Dequeue()
            if ea.BasicProperties.CorrelationId = corrId
            then Encoding.UTF8.GetString(ea.Body)
            else loop ()

        loop ()

    member this.Close () =
        connection.Close ()

let rpcClient = new RPCClient()

printfn " [x] Requesting fib(30)"
let response = rpcClient.Call("30")
printfn " [.] Got '%s'" response

rpcClient.Close()