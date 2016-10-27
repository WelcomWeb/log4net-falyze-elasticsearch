using System;
using System.Collections.Generic;

using log4net.Appender;
using log4net.Core;

using log4net.Falyze.ElasticSearch.Models;

namespace log4net.Falyze.ElasticSearch
{
    public class ElasticSearchAppender : AppenderSkeleton
    {
        private string _connectionString = null;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new LogException("No connection string was provided for log4net.Falyze.ElasticSearch");
                }

                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        private string _indexPattern = "logs-*";
        public string IndexPattern
        {
            get
            {
                return _indexPattern;
            }
            set
            {
                _indexPattern = value;
            }
        }

        private string _documentType = "logentry";
        public string DocumentTypeName
        {
            get
            {
                return _documentType;
            }
            set
            {
                _documentType = value;
            }
        }
        
        public string LogServiceGroup { get; set; }
        public string Credentials { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var entry = new Entry
            {
                CreatedAt = DateTime.UtcNow,
                Level = loggingEvent.Level.Name,
                LogServiceGroup = LogServiceGroup,
                ServiceName = loggingEvent.LoggerName,
                Message = GetMessage(loggingEvent),
                Metric = GetMetric(loggingEvent),
                Custom = GetCustomData(loggingEvent),
                Exception = GetException(loggingEvent)
            };

            Uri serviceUri = null;
            if (Uri.TryCreate(GetServiceUri(), UriKind.Absolute, out serviceUri))
            {
                var client = new Services.HttpService(Credentials)
                {
                    ServiceURI = serviceUri
                };
                client.Post(entry);
            }
        }

        private MessageEntryProperty GetMessage(LoggingEvent loggingEvent)
        {
            var message = loggingEvent.MessageObject as MessageEntryProperty;
            if (message != null)
            {
                return message;
            }

            var metric = loggingEvent.MessageObject as MetricEntryProperty;
            if (metric != null)
            {
                return new MessageEntryProperty(null, metric.GetDescription());
            }

            var data = loggingEvent.MessageObject as CustomDataEntryProperty;
            if (data != null)
            {
                return data.Message;
            }

            return new MessageEntryProperty(null, loggingEvent.MessageObject as string);
        }

        private MetricEntryProperty GetMetric(LoggingEvent loggingEvent)
        {
            return loggingEvent.MessageObject as MetricEntryProperty;
        }

        private IDictionary<string, string> GetCustomData(LoggingEvent loggingEvent)
        {
            var data = loggingEvent.MessageObject as CustomDataEntryProperty;
            if (data != null && data.Data.Keys.Count > 0)
            {
                return data.Data;
            }

            return null;
        }

        private ExceptionEntryProperty GetException(LoggingEvent loggingEvent)
        {
            if (loggingEvent.ExceptionObject != null)
            {
                return GetExceptionEntry(loggingEvent.ExceptionObject, locationInfo: loggingEvent.LocationInformation);
            }

            return null;
        }

        private ExceptionEntryProperty GetExceptionEntry(Exception exception, LocationInfo locationInfo = null, string sourceInfo = null)
        {
            return new ExceptionEntryProperty()
            {
                Type = exception.GetType().Name,
                Message = exception.Message,
                Origin = locationInfo != null ? string.Format("{0}:line {1}", locationInfo.FileName, locationInfo.LineNumber) : sourceInfo,
                StackTrace = exception.StackTrace.Trim(),
                InnerException = exception.InnerException != null ? GetExceptionEntry(exception.InnerException, sourceInfo: exception.InnerException.Source) : null
            };
        }

        private string GetServiceUri()
        {
            return string.Format("{0}/{1}/{2}", ConnectionString.TrimEnd(new char[] { '/' }), GetIndexName(), DocumentTypeName);
        }

        private string GetIndexName()
        {
            if (IndexPattern.Contains("*"))
            {
                return string.Format(IndexPattern, DateTime.UtcNow.ToString("yyyy-MM-dd"));
            }

            return IndexPattern;
        }
    }
}
