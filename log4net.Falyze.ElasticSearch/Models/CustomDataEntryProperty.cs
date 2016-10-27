using System.Collections.Generic;

namespace log4net.Falyze.ElasticSearch.Models
{
    internal class CustomDataEntryProperty
    {
        private IDictionary<string, string> _data;
        private MessageEntryProperty _message;

        public CustomDataEntryProperty(IDictionary<string, string> data, MessageEntryProperty message)
        {
            _data = data;
            _message = message;
        }

        public IDictionary<string, string> Data
        {
            get
            {
                return _data;
            }
        }

        public MessageEntryProperty Message
        {
            get
            {
                return _message;
            }
        }
    }
}
