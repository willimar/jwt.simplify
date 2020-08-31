using crud.api.core.enums;
using crud.api.core.interfaces;
using System.Collections.Generic;

namespace jwt.simplify.services
{
    internal class HandleMessage : IHandleMessage
    {
        public string MessageType { get; }

        public string Message { get; }

        public HandlesCode Code { get; }

        public List<string> StackTrace { get; }

        public HandleMessage(string messageType, string message, HandlesCode code)
        {
            this.MessageType = messageType;
            this.Message = message;
            this.Code = code;
        }
    }
}