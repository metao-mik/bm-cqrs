using System;
using System.Collections.Generic;

namespace Tests {
    public abstract class CommandHandler {
        
        private Dictionary<Type,Action<ICommand>> _commandhandlers = new Dictionary<Type,Action<ICommand>>();
        protected void Register_Command<TCommand>(Action<TCommand> handler){
            _commandhandlers.Add(typeof(TCommand), cmd=>handler((TCommand)cmd));
        }
    
        public void Execute(ICommand command){
            // check ob das wirklich existiert...
            _commandhandlers[command.GetType()](command);
        }
    }

    internal class VerkaufCommandHandler : CommandHandler {

                
        public VerkaufCommandHandler(Func<Guid,Guid> tablet_zu_bon_repository){

            Register_Command<PositionHinzufuegenCommand>(Handle);
        }
        private void Handle(PositionHinzufuegenCommand cmd)
        {
            throw new NotImplementedException();
        }        
    }
}