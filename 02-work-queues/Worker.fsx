#r "../RabbitMQ.Client.dll"

open System
open System.Threading
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    let durable = true
    channel.QueueDeclare("task_queue", durable, false, false, null) |> ignore

    channel.BasicQos(0u, 1us, false)
    let consumer = new QueueingBasicConsumer(channel)
    channel.BasicConsume("task_queue", false, consumer) |> ignore

    printfn " [*] Waiting for messages. To exit press CTRL+C"
    
    let rec loop () : unit =
        let ea = consumer.Queue.Dequeue();

        let body = ea.Body
        let message = Encoding.UTF8.GetString(body)
        printfn " [x] Received %s" message
        
        let dots = message.Split('.').Length - 1
        Thread.Sleep(dots * 1000)

        printfn " [x] Done"

        channel.BasicAck(ea.DeliveryTag, false)

        loop ()

    loop ()
)
