using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur
{
    public abstract class CommandHandler
    {

        private readonly EventStore _eventstore;
        private readonly Action<Exception> _onError;
        private readonly Dictionary<Type,Action<Command, UnitOfWork>> _commandhandlers = new Dictionary<Type,Action<Command, UnitOfWork>>();

        protected CommandHandler(EventStore eventstore, Action<Exception> on_error)
        {
            _eventstore = eventstore;
            _onError = on_error;
        }

        protected void Register_Command<TCommand>(Action<TCommand, UnitOfWork> handler){
            _commandhandlers.Add(typeof(TCommand), (cmd,uow)=>handler((TCommand)cmd, uow));
        }
    
        public void Execute(params Command[] commands)
        {
            var uow = new EventSourced_UnitOfWork(_eventstore);

            try
            {
                foreach (var command in commands)
                {
                    _commandhandlers[command.GetType()](command, uow);
                }

                uow.Commit();
            }
            catch (Error error)
            {
                Console.WriteLine($"Ein Vorgang konnte nicht ausgeführt werden: {error.Message}.");
                //uow.Commit(error.Details); -- error.Details ist ein Event, dass ggf. persistiert und veröffentlicht werden kann.
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}.");
                _onError(ex);
            }
        }



    }
}