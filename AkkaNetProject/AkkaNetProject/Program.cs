using Akka.Actor;
using AkkaNetProject.classes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AkkaNetProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var system = ActorSystem.Create("System"))
            {
                // Create top level supervisor
                var mainActor = system.ActorOf<MainActor>("MainActor");
                mainActor.Tell("Run");
                // Exit the system after ENTER is pressed
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }

    class MainActor : ReceiveActor
    {
        private readonly int[] _array = new int[]{ 9, 4, 3, 2, 5};
        private int _index;
        private int _quantityActorsInSystem;
        private Element _element;
        private List<IActorRef> _actorlist;
        public MainActor()
        {
            _index = 0;
            _quantityActorsInSystem = 0;
            _actorlist = new List<IActorRef>();
            Receive<string>(message =>
            {
                Console.WriteLine("Start mainActor");
                _element = new Element(_index, _array[_index]);
                Run();
            });

            Receive<Element>(result =>
            {                
                _element = _element.Number < result.Number ? _element : result;
                Console.WriteLine($"Result: {_element.Number}");
                _quantityActorsInSystem--;                
                if (_quantityActorsInSystem == 0)
                {
                    
                    Console.WriteLine($"Min element: {_element.Number}");
                    var buf = _array[_index];
                    _array[_index] = _element.Number;
                    _array[_element.Index] = buf;
                    _index++;
                    _element = new Element(_index, _array[_index]);

                    if (_index == _array.Length - 1)
                    {
                        Console.Write("Finish...\n Result: ");
                        foreach (var el in _array)
                        {
                            Console.Write($"{el}, ");
                        }

                        return;
                    }

                    Tell();
                }
            });
        }

        private void Run()
        {
            var minElement = new Element(_index, _array[_index]);
            for (int i = _index + 1; i < _array.Length; i++)
            {                
                IActorRef actorRef = Context.ActorOf<ComparatorActor>("ComparatorActor" + i);
                _actorlist.Add(actorRef);
                Console.WriteLine($"Start Actor: {actorRef}");
                _quantityActorsInSystem++;
                actorRef.Tell(new AkkaMsg(minElement, new Element(i, _array[i])));
            }
        }

        private void Tell()
        {
            var minElement = new Element(_index, _array[_index]);
            var quantity = 0;
            for (int i = _index + 1; i < _array.Length; i++)
            {
                IActorRef actorRef = _actorlist[quantity];
                quantity++;
                Console.WriteLine($"Start Actor: {actorRef}");
                _quantityActorsInSystem++;
                actorRef.Tell(new AkkaMsg(minElement, new Element(i, _array[i])));
            }
        }

    }
}
