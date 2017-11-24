using System;
using System.Collections.Generic;

namespace Billmorro.Infrastruktur
{
    public abstract class CommandHandler
    {

        private readonly EventStore _eventstore;
        private readonly Dictionary<Type,Action<Command, UnitOfWork>> _commandhandlers = new Dictionary<Type,Action<Command, UnitOfWork>>();

        protected CommandHandler(EventStore eventstore)
        {
            _eventstore = eventstore;
        }

        protected void Register_Command<TCommand>(Action<TCommand, UnitOfWork> handler){
            _commandhandlers.Add(typeof(TCommand), (cmd,uow)=>handler((TCommand)cmd, uow));
        }
    
        public void Execute(params Command[] commands)
        {
            var uow = new EventSourced_UnitOfWork(_eventstore);

            foreach (var command in commands)
            {
                _commandhandlers[command.GetType()](command, uow);
            }

            uow.Commit();
        }



    }
}