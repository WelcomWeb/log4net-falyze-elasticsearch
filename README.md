# log4net.Falyze.ElasticSearch

[![Build Status](https://travis-ci.org/WelcomWeb/log4net.Falyze.ElasticSearch.svg?branch=master)](https://travis-ci.org/WelcomWeb/log4net.Falyze.ElasticSearch)
[![NuGet version](https://badge.fury.io/nu/log4net.Falyze.ElasticSearch.svg)](https://badge.fury.io/nu/log4net.Falyze.ElasticSearch)

A simple log4net appender for Elastic Search, creating readable and customizable index documents.

## Configure

### Create the configuration, either in `Web.config` or in an `app.config` file

	<configuration>
		<configSections>
			<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
		</configSections>

		<log4net>
			<appender name="ElasticSearchAppender" type="log4net.Falyze.ElasticSearch.ElasticSearchAppender, log4net.Falyze.ElasticSearch">
				<ConnectionString value="https://my-elastic-cluster.domain.com" />
				<LogServiceGroup value="My Application Name" />
				<IndexPattern value="logs-*" />
				<DocumentTypeName value="logentry" />
				<Credentials value="username:password" />
			</appender>
			<root>
				<level value="ALL"/>
				<appender-ref ref="ElasticSearchAppender"/>
			</root>
		</log4net>
	</configuration>


### Configure using a startup file (like Global.asax)

	log4net.Config.XmlConfigurator.Configure();


### Configure using assembly directive (like in Assembly.cs)

	[assembly: log4net.Config.XmlConfigurator(Watch=false)]


## Configuration settings variables

#### `ConnectionString`
The connection string to your Elastic Search instance. Required.

#### `LogServiceGroup`
A second dimension for grouping Elastic Search documents. Normally this property would contain the application name. Optional - defaults to null.

#### `IndexPattern`
The Elastic Search index pattern to use. Optional - defaults to `logs-*`.

#### `DocumentTypeName`
The Elastic Search document type to use. Optional - defaults to `logentry`.

#### `Credentials`
The Basic Authentication credentials (if any) for the Elastic Search cluster. Optional - defaults to null (no authentication).

	
## Extended log messages

log4net.Falyze.ElasticSearch has built-in support for extended log messages, giving the oppurtunity to add a grouping dimension for a log message.

    using log4net;
    using log4net.Falyze.ElasticSearch;
    
    namespace My.Caller
    {
    	public class FirstCaller
        {
        	private static readonly ILog _log = LogManager.GetLogger(typeof(FirstCaller));
            
            public FirstCaller()
            {
            	_log.Info("Callers", "FirstCaller is now instantiated");
            }
        }
        
        public class SecondCaller
        {
        	private static readonly ILog _log = LogManager.GetLogger(typeof(SecondCaller));
            
            public SecondCaller()
            {
            	_log.Info("Callers", "SecondCaller is now instantiated");
            }
        }
    }
    
These log entries will create Elastic Search documents similar to these:

	{
	  "_index": "logs-2016-10-27",
	  "_type": "logentry",
	  "_id": "AVgFewRdOErJRfxkE5Fd",
	  "_source": {
		"created_at": "2016-10-27T09:31:37.5381378Z",
		"log_service_group": "My Application Name",
		"service_name": "My.Callers.FirstCaller",
		"level": "INFO",
		"message": {
		  "message_group": "Callers",
		  "description": "FirstCaller is now instantiated"
		}
	  }
	}

	{
	  "_index": "logs-2016-10-27",
	  "_type": "logentry",
	  "_id": "AVgFewRdOErJRfxkE5Fg",
	  "_source": {
		"created_at": "2016-10-27T09:31:39.7712183Z",
		"log_service_group": "My Application Name",
		"service_name": "My.Callers.SecondCaller",
		"level": "INFO",
		"message": {
		  "message_group": "Callers",
		  "description": "SecondCaller is now instantiated"
		}
	  }
	}

As one can see, it gives a second dimension to use when grouping documents (from ex. Kibana) - for these documents it's the common nominator `message.message_group` property.


## Extended log message properties
### Metric data

When logging metrics there's always some hustle figuring out what to pick out from an Elastic Search document, and how to retrieve the exact metric value. log4net.Falyze.ElasticSearch has a custom message property type for these circumstances, a `MetricEntryProperty` model. It supports two different metric types at the moment, `MemorySize` and `LoadTime`.

	using log4net;
    using log4net.Falyze.ElasticSearch;
    using System.Diagnostics;
    
    namespace My.Services
    {
    	public class HttpService
        {
        	private static readonly ILog _log = LogManager.GetLogger(typeof(HttpService));
            
            public string GetRemoteString()
            {
            	using (var client = new WebClient())
                {
                	var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    
                    var url = "http://www.big-data.com/bigfile.txt";
                    var data = client.DownloadString(url);
                    stopwatch.Stop();
                    
                    _log.Info(MetricEntryProperty.MetricEntryType.LoadTime, stopwatch.Elasped.Seconds, "HTTP Metrics", url);
                    
                    return data;
                }
            }
        }
    }
    
This log will end up in a document similar to this:

	{
	  "_index": "logs-2016-10-27",
	  "_type": "logentry",
	  "_id": "AVgFewRdOErJRfxkE5Fa",
	  "_source": {
		"created_at": "2016-10-27T09:31:49.7712183Z",
		"log_service_group": "My Application Name",
		"service_name": "My.Services.HttpService",
		"level": "INFO",
		"message": {
		  "message_group": "HTTP Metrics",
		  "description": "http://www.big-data.com/bigfile.txt"
		},
        "metric": {
          "type": "LoadTime",
          "value": 47
        }
	  }
	}

Another property field is available at the document, `metric`, containing the metric type name (`LoadTime`) and the elapsed time in seconds. This property can now easily be used in a BI tool as Kibana - visualizing it in any preferred way.

### Custom data

There's a custom data implementation available for log4net.Falyze.ElasticSearch as well, making it easy to populate an Elastic Search document with arbitrary data. The implementation simply takes a `IDictionary<string, string>` and puts it under a field named `custom`.

    using log4net;
    using log4net.Falyze.ElasticSearch;

    namespace My.Customizations
    {
        public class AnyImplementation
        {
            private static readonly ILog _log = LogManager.GetLogger(typeof(AnyImplementation));

            public string AnyMethod()
            {
                var data = new Dictionary<string, string>()
                {
                    { "FirstKey", "First Value!" },
                    { "SecondKey", "Second Value!" }
                };
                _log.Info(data, "Custom data retrieved");
            }
        }
    }
    
The final document:

	{
	  "_index": "logs-2016-10-27",
	  "_type": "logentry",
	  "_id": "AVgFewRdOErJRfxkE5Fa",
	  "_source": {
		"created_at": "2016-10-27T09:36:49.7712183Z",
		"log_service_group": "My Application Name",
		"service_name": "My.Customizations.AnyImplementation",
		"level": "INFO",
		"message": {
		  "message_group": null,
		  "description": "Custom data retrieved"
		},
        "custom": {
          "FirstKey": "First Value!",
          "SecondKey": "Second Value!"
		}
	  }
	}

