using crud.api.core.enums;
using crud.api.core.interfaces;
using System.Collections.Generic;

namespace jwt.consumer
{
    internal class HandleMessageAbs : IHandleMessage
    {
        public HandleMessageAbs()
        {

        }

        public string MessageType { get; set; }

        public string Message { get; set; }

        public HandlesCode Code { get; set; }

        public List<string> StackTrace { get; set; }
    }
}