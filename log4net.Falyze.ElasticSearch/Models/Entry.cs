using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace log4net.Falyze.ElasticSearch.Models
{
    [DataContract]
    public class Entry
    {
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "log_service_group")]
        public string LogServiceGroup { get; set; }

        [DataMember(Name = "service_name")]
        public string ServiceName { get; set; }

        [DataMember(Name = "level")]
        public string Level { get; set; }

        [DataMember(Name = "message")]
        public MessageEntryProperty Message { get; set; }

        [DataMember(Name = "metric")]
        public MetricEntryProperty Metric { get; set; }

        [DataMember(Name = "custom")]
        public IDictionary<string, string> Custom { get; set; }

        [DataMember(Name = "exception")]
        public ExceptionEntryProperty Exception { get; set; }
    }
}
