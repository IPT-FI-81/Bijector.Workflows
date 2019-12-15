using System;

namespace Bijector.Workflows.Messages
{
    public abstract class BaseDriveEntityMessage
    {
        public BaseDriveEntityMessage(){}

        public BaseDriveEntityMessage(int serviceId)
        {
            ServiceId = serviceId;
        }

        public int ServiceId { get; set; }
    }
}