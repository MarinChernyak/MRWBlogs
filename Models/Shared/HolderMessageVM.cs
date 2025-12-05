using Microsoft.Extensions.Diagnostics.HealthChecks;
using static MRWBlogs.Models.Shared.HolderMessageVM;

namespace MRWBlogs.Models.Shared
{
    public class MessageContainer
    {
        public MessageType MsgType { get; protected set; } = MessageType.None;
        public string Message { get; protected set; } = string.Empty;
        public MessageContainer(string message, MessageType typeMessage) 
        {
            MsgType = typeMessage;
            Message = message;
        }
        public MessageContainer(MessageContainer container)
        {
            MsgType = container.MsgType;
            Message = container.Message;
        }
        public MessageContainer()
        {
            MsgType = MessageType.None;
            Message =  string.Empty;
        }
    }
    public class HolderMessageVM
    {
        public enum MessageType
        {
            None=0,
            Success=1,
            Info=2,
            Warning=3,
            Error= 4
        }
        public bool IsProcesing { get; set; } = false;
        public MessageContainer MessageContainer { get; set; } = new();

        public void SetMessage(string message, MessageType typeMessage)
        {
            MessageContainer = new MessageContainer(message, typeMessage);
        }
        public string getMessage ()
        {
            string msg = string.Empty;
            if (MessageContainer != null)
            {
                msg = MessageContainer.Message;
            }
            return msg;
        }
        public string getClassMsg ()
        {
            string classmsg = string.Empty;
            if (MessageContainer != null)
            {
                switch (MessageContainer.MsgType)
                {
                    case MessageType.Success:
                        classmsg = "alert alert-success";
                        break;
                    case MessageType.Info:
                        classmsg = "alert alert-info";
                        break;
                    case MessageType.Warning:
                        classmsg = "alert alert-warning";
                        break;
                    case MessageType.Error:
                        classmsg = "alert alert-danger";
                        break;
                    default:
                        classmsg = "alert alert-secondary";
                        break;
                }
            }
            return classmsg;
        }

    }
}
