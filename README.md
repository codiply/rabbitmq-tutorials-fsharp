# .NET F# code for RabbitMQ tutorials

See [corresponding C# tutorials](http://www.rabbitmq.com/getstarted.html) for more information.

### Requirements
To run these tutorials yourself, you'll first need to download and install the [RabbitMQ Server](http://www.rabbitmq.com/download.html). The [client library package](http://www.rabbitmq.com/dotnet.html) is also needed, and you'll need to place `RabbitMQ.Client.dll` inside the top folder (or modify the references within the scripts). Finally, you will need to have `fsi` in your path. In my version of Windows and Visual Studio, the path to `fsi` is `C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0`.

### Running the code

The tutorials have been written as scripts in F#, and can be run with `fsi` (F# Interactive). There is no explicit compilation step, as `fsi` will attempt to compile the code before running it. In each tutorial, you'll need to use multiple command line windows, one for each producer/consumer.

#### [Tutorial one: "Hello World!"](http://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)

    >fsi Send.fsx
	
	>fsi Receive.fsx
	
#### [Tutorial two: Work Queues](http://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html)

    >fsi NewTask.fsx First message.
    >fsi NewTask.fsx Second message..
    >fsi NewTask.fsx Third message...
    >fsi NewTask.fsx Fourth message....
    >fsi NewTask.fsx Fifth message.....
    
    >fsi Worker.fsx

    >fsi Worker.fsx

#### [Tutorial three: Publish/Subscribe](http://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html)

    >fsi ReceiveLogs.fsx > logs_from_rabbit.log

    >fsi ReceiveLogs

    >fsi EmitLog.fsx
    >fsi EmitLog.fsx "error: Run. Run. Or it will explode."

#### [Tutorial four: Routing](http://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html)

    >fsi ReceiveLogsDirect.fsx warning error > logs_from_rabbit.log

    >fsi ReceiveLogsDirect.fsx info warning error

    >fsi EmitLogDirect.exe error "Run. Run. Or it will explode."

#### [Tutorial five: Topics](http://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html)

    >fsi ReceiveLogsTopic.fsx "#"
    
    >fsi ReceiveLogsTopic.fsx "kern.*" 

    >fsi ReceiveLogsTopic.fsx "*.critical"

    >fsi ReceiveLogsTopic.fsx "kern.*" "*.critical"

    >fsi EmitLogTopic.fsx "kern.critical" "A critical kernel error"

#### [Tutorial six: RPC](http://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html)

    >fsi RPCServer.fsx

    >fsi RPCClient.fsx