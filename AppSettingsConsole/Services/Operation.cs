using System;
using System.Collections.Generic;
using System.Text;

namespace AppSettingsConsole.Services
{
    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton, IOperationSingletonInstance
    {
        Guid _guid; 
        public Guid OperationId => _guid;

        public Operation() : this(Guid.NewGuid())
        {

        }

        public Operation(Guid guid)
        {
            _guid = guid;
        }
    }
}
