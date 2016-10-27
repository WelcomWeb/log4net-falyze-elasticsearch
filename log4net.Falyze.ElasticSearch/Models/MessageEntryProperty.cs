using System.Runtime.Serialization;

namespace log4net.Falyze.ElasticSearch.Models
{
    [DataContract]
    public class MessageEntryProperty
    {
        [DataMember(Name = "message_group")]
        public string GroupName { get; }

        [DataMember(Name = "description")]
        public string Description { get; }

        public MessageEntryProperty(string groupName, string description)
        {
            GroupName = groupName;
            Description = description;
        }
    }
}
