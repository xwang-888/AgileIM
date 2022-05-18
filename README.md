项目结构介绍

AgileIM.Client ，客户端项目

AgileM.IM  类库，IM的核心类

AgileIM.Service，IM的接口层，集成**IdentityServer4**鉴权与授权

AgileIM.Shared，IM共享层，存放公用的一些方法和模型

AgileIM.Web，IM Web项目

项目介绍：

​	使用WebSocket+redis 做即时通讯服务，客户端用websocket协议，webApi 负责业务，调用AgileIM.IM中的发送消息以及创建群组的方法，并将数据推送到Redis, Redis使用发布订阅形式，Redis5.0 新增Stream方式追加消息，这里就要科普一下Redis5.0的Stream了，Stream是一个强大的支持多播的可持久化的消息队列，它借鉴了Kafka的设计，Redis Stream 它有一个消息链表，将所有加入的消息都串起来，每个消息都有一个唯一的 ID 和对应的内容。消息是持久化的，Redis 重启后，内容还在，消费者内部会有个状态变量，它记录了当前已经被客户端读取的消息，但是还没有 ack。如果客户端没有 ack，这个变量里面的消息 ID 会越来越多，一 旦某个消息被 ack，它就开始减少。这个变量在 Redis 官方被称之为**PEL**，也就是 **Pending Entries List**，这是一个很核心的数据结构，它用来确保客户端至少消费了消息一 次，而不会在网络传输的中途丢失了没处理，所以使用Redis做即时通讯是可以进行尝试的。

​	优点：

1. 分离IM和业务，让IM更纯粹，只负责维护连接，消息推送与转发
2. 使用websocket协议保证多端协议统一
3. 支持集群，

​    进展：

​	目前客户端项目界面已经差不多结束，再优化优化界面就进行下一步操作。

​    下一步计划：

- [ ] 数据库数据表设计

- [ ] webApi业务接口编写

- [ ] WPF客户端数据交互

- [ ] Web端 界面设计以及数据交互 考虑使用（ VUE，Blazor，MVC）的其中一种

  

  *tips:因为真正编码时间没有想象的那么多，所以进度可能不会很快。*

------

AgileIM.Client (WPF) 运行界面示例

![20220518133047](https://user-images.githubusercontent.com/56998197/168964109-115149d1-0296-4020-bea6-63ee217ffe16.png)
![image-20220420091836346](https://user-images.githubusercontent.com/56998197/164133004-dc707b16-7721-45b9-8f50-8bf00f45d3a0.png)
![image-20220420091922888](https://user-images.githubusercontent.com/56998197/164133013-f8d0433a-f06a-4528-a629-747b1d8c6cee.png)
![image-20220420092021553](https://user-images.githubusercontent.com/56998197/164133033-11d0222b-4f7d-4e03-9db7-065fb67e5124.png)
![image-20220420092047740](https://user-images.githubusercontent.com/56998197/164133041-7ef2bb56-09f7-4361-9ba2-3cfd6d9d7529.png)
![image-20220420092107408](https://user-images.githubusercontent.com/56998197/164133052-f9618e52-f7f2-4fac-b324-91a6b97669f7.png)
![image-20220420092141415](https://user-images.githubusercontent.com/56998197/164133064-53f8a9e5-f046-4d90-96e2-8f5d32921eba.png)


