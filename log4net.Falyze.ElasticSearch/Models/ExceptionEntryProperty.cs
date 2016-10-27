using System.Runtime.Serialization;

namespace log4net.Falyze.ElasticSearch.Models
{
    [DataContract]
    public class ExceptionEntryProperty
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "origin")]
        public string Origin { get; set; }

        [DataMember(Name = "stack_trace")]
        public string StackTrace { get; set; }

        [DataMember(Name = "inner_exception", EmitDefaultValue = false)]
        public ExceptionEntryProperty InnerException { get; set; }
    }
}
