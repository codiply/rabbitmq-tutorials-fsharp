#r "../RabbitMQ.Client.dll"

open System
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text

let factory = new ConnectionFactory(HostName = "localhost")
(
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    
    channel.ExchangeDeclare("logs", "fanout")

    let queueName = channel.QueueDeclare().QueueName

    channel.QueueBind(queueName, "logs", "")
    let consumer = new QueueingBasicConsumer(channel)
    channel.BasicConsume(queueName, true, consumer) |> ignore

    printfn " [*] Waiting for logs. To exit press CTRL+C"
    
    let rec loop () : unit =
        let ea = consumer.Queue.Dequeue();

        let body = ea.Body
        let message = Encoding.UTF8.GetString(body)
        printfn " [x] %s" message
        
        loop ()

    loop ()
)
