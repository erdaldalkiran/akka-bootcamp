using System;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            //YOU NEED TO FILL IN HERE
            // make consoleWriterActor using these props: Props.Create(() => new ConsoleWriterActor())
            // make consoleReaderActor using these props: Props.Create(() => new ConsoleReaderActor(consoleWriterActor))
            var consoleWriterActor = MyActorSystem.ActorOf(Props.Create<ConsoleWriterActor>(), "ConsoleWriterActor");
            var tailCoordinatorActor = MyActorSystem.ActorOf(Props.Create<TailCoordinatorActor>(), "TailCoordinatorActor");
            var fileValidatorActor = MyActorSystem.ActorOf(Props.Create<FileValidatorActor>(consoleWriterActor, tailCoordinatorActor), "FileValidatorActor");
            var consoleReaderActor = MyActorSystem.ActorOf(Props.Create<ConsoleReaderActor>(fileValidatorActor), "ConsoleReaderActor");


            // tell console reader to begin
            //YOU NEED TO FILL IN HERE
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
