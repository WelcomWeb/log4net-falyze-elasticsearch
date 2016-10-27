using System;
using System.Collections.Generic;

using log4net.Falyze.ElasticSearch.Models;

namespace log4net.Falyze.ElasticSearch
{
    public static class LogExtensions
    {
        public static void Debug(this ILog log, string messageGroup, string description)
        {
            log.Debug(new MessageEntryProperty(messageGroup, description));
        }
        public static void Error(this ILog log, string messageGroup, string description)
        {
            log.Error(new MessageEntryProperty(messageGroup, description));
        }
        public static void Fatal(this ILog log, string messageGroup, string description)
        {
            log.Fatal(new MessageEntryProperty(messageGroup, description));
        }
        public static void Info(this ILog log, string messageGroup, string description)
        {
            log.Info(new MessageEntryProperty(messageGroup, description));
        }
        public static void Warn(this ILog log, string messageGroup, string description)
        {
            log.Warn(new MessageEntryProperty(messageGroup, description));
        }

        public static void Debug(this ILog log, IDictionary<string, string> data, string description)
        {
            log.Debug(new CustomDataEntryProperty(data, new MessageEntryProperty(null, description)));
        }
        public static void Debug(this ILog log, IDictionary<string, string> data, string messageGroup, string description)
        {
            log.Debug(new CustomDataEntryProperty(data, new MessageEntryProperty(messageGroup, description)));
        }
        public static void Error(this ILog log, IDictionary<string, string> data, string description)
        {
            log.Error(new CustomDataEntryProperty(data, new MessageEntryProperty(null, description)));
        }
        public static void Error(this ILog log, IDictionary<string, string> data, string messageGroup, string description)
        {
            log.Error(new CustomDataEntryProperty(data, new MessageEntryProperty(messageGroup, description)));
        }
        public static void Fatal(this ILog log, IDictionary<string, string> data, string description)
        {
            log.Fatal(new CustomDataEntryProperty(data, new MessageEntryProperty(null, description)));
        }
        public static void Fatal(this ILog log, IDictionary<string, string> data, string messageGroup, string description)
        {
            log.Fatal(new CustomDataEntryProperty(data, new MessageEntryProperty(messageGroup, description)));
        }
        public static void Info(this ILog log, IDictionary<string, string> data, string description)
        {
            log.Info(new CustomDataEntryProperty(data, new MessageEntryProperty(null, description)));
        }
        public static void Info(this ILog log, IDictionary<string, string> data, string messageGroup, string description)
        {
            log.Info(new CustomDataEntryProperty(data, new MessageEntryProperty(messageGroup, description)));
        }
        public static void Warn(this ILog log, IDictionary<string, string> data, string description)
        {
            log.Warn(new CustomDataEntryProperty(data, new MessageEntryProperty(null, description)));
        }
        public static void Warn(this ILog log, IDictionary<string, string> data, string messageGroup, string description)
        {
            log.Warn(new CustomDataEntryProperty(data, new MessageEntryProperty(messageGroup, description)));
        }

        public static void Debug(this ILog log, MetricEntryProperty.MetricEntryType metricEntryType, int value, string message = null)
        {
            log.Debug(new MetricEntryProperty(metricEntryType, value, message));
        }
        public static void Error(this ILog log, MetricEntryProperty.MetricEntryType metricEntryType, int value, string message = null)
        {
            log.Error(new MetricEntryProperty(metricEntryType, value, message));
        }
        public static void Fatal(this ILog log, MetricEntryProperty.MetricEntryType metricEntryType, int value, string message = null)
        {
            log.Fatal(new MetricEntryProperty(metricEntryType, value, message));
        }
        public static void Info(this ILog log, MetricEntryProperty.MetricEntryType metricEntryType, int value, string message = null)
        {
            log.Info(new MetricEntryProperty(metricEntryType, value, message));
        }
        public static void Warn(this ILog log, MetricEntryProperty.MetricEntryType metricEntryType, int value, string message = null)
        {
            log.Warn(new MetricEntryProperty(metricEntryType, value, message));
        }

        public static void Debug(this ILog log, string messageGroup, string description, Exception e)
        {
            log.Debug(new MessageEntryProperty(messageGroup, description), e);
        }
        public static void Error(this ILog log, string messageGroup, string description, Exception e)
        {
            log.Error(new MessageEntryProperty(messageGroup, description), e);
        }
        public static void Fatal(this ILog log, string messageGroup, string description, Exception e)
        {
            log.Fatal(new MessageEntryProperty(messageGroup, description), e);
        }
        public static void Info(this ILog log, string messageGroup, string description, Exception e)
        {
            log.Info(new MessageEntryProperty(messageGroup, description), e);
        }
        public static void Warn(this ILog log, string messageGroup, string description, Exception e)
        {
            log.Warn(new MessageEntryProperty(messageGroup, description), e);
        }
    }
}
