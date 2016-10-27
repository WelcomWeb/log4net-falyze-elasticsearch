using System;
using System.Runtime.Serialization;

namespace log4net.Falyze.ElasticSearch.Models
{
    [DataContract]
    public class MetricEntryProperty
    {
        public enum MetricEntryType
        {
            MemorySize,
            LoadTime,
        }

        [DataMember(Name = "type")]
        public string Type { get; }

        [DataMember(Name = "value")]
        public int Value { get; }

        private string _message = null;

        public MetricEntryProperty(MetricEntryType type, int value, string message = null)
        {
            Type = Enum.GetName(typeof(MetricEntryType), type);
            Value = value;

            _message = message;
        }

        public string GetDescription()
        {
            return _message;
        }
    }
}
