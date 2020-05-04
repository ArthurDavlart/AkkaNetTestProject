using Akka.Actor;
using AkkaNetProject.classes;
using System;

namespace AkkaNetProject
{
    public class ComparatorActor : ReceiveActor
    {

        public ComparatorActor()
        {
            Receive<AkkaMsg>(akkaMsg =>
            {
                Console.WriteLine($"Получил сообщение {Self}. Сравниваю {akkaMsg.MinElement.Number} и" +
                        $" {akkaMsg.NewElement.Number}");
                var result =
                    akkaMsg.MinElement.Number < akkaMsg.NewElement.Number ? akkaMsg.MinElement : akkaMsg.NewElement;
                Sender.Tell(result);
                //Context.Stop(Self);
            });
        }
        //protected override void OnReceive(object message)
        //{
        //    switch (message)
        //    {
        //        case AkkaMsg akkaMsg:
                    
        //            break;
        //    }
        //}
    }
}
