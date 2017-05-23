using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        private readonly IActorRef _validatorActor;

        public ConsoleReaderActor(IActorRef validatorActor)
        {
            _validatorActor = validatorActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
                GetInput();
            }
            else if (message is Messages.ContinueProcessing)
            {
                GetInput();
            }

            Unhandled(message);
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }

        private void GetInput()
        {
            var message = Console.ReadLine();
            if (String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the entire actor system (allows the process to exit)
                Context.System.Terminate();
                return;
            }

            _validatorActor.Tell(message);
        }
    }
}