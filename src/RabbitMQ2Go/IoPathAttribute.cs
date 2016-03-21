using System;

namespace RabbitMQ2Go
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IoPathAttribute : Attribute
    {
        public bool IsDirectory { get; set; }
    }
}
