using System;
using System.ComponentModel;
using System.IO;

namespace RabbitMQ2Go.SingletonHelpers
{
    public interface IRabbitMqEnvironmentVariables
    {
        /// <summary>
        /// Name: RABBITMQ_BASE<para/>
        /// Default: %APPDATA%\RabbitMQ<para/>
        /// This base directory contains sub-directories for the RabbitMQ server's database and log files. 
        /// Alternatively, set RABBITMQ_MNESIA_BASE and RABBITMQ_LOG_BASE individually.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_BASE"), IoPath(IsDirectory = true)]
        string RabbitMqBase { get; set; }
        /// <summary>
        /// Name: RABBITMQ_CONFIG_FILE<para/>
        /// Default: %RABBITMQ_BASE%\rabbitmq<para/>
        /// The path to the configuration file, without the .config extension. 
        /// If the configuration file is present it is used by the server to configure RabbitMQ components. 
        /// See Configuration guide for more information.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_CONFIG_FILE"), IoPath]
        string RabbitMqConfigFile { get; set; }
        /// <summary>
        /// Name: RABBITMQ_MNESIA_BASE<para/>
        /// Default: %RABBITMQ_BASE%\db<para/>
        /// This base directory contains sub-directories for the RabbitMQ server's Mnesia database files, one for each node, unless RABBITMQ_MNESIA_DIR is set explicitly. 
        /// (In addition to Mnesia files this location also contains message storage and index files as well as schema and cluster details.)<para/>
        /// </summary>
        [DisplayName("RABBITMQ_MNESIA_BASE"), IoPath(IsDirectory = true)]
        string RabbitMqMnesiaBase { get; set; }
        /// <summary>
        /// Name: RABBITMQ_MNESIA_DIR<para/>
        /// Default: %RABBITMQ_MNESIA_BASE%\%RABBITMQ_NODENAME%<para/>
        /// The directory where this RabbitMQ node's Mnesia database files are placed. 
        /// (In addition to Mnesia files this location also contains message storage and index files as well as schema and cluster details.)<para/>
        /// </summary>
        [DisplayName("RABBITMQ_MNESIA_DIR"), IoPath(IsDirectory = true)]
        string RabbitMqMnesiaDir { get; set; }
        /// <summary>
        /// This base directory contains the RabbitMQ server's log files, unless RABBITMQ_LOGS or RABBITMQ_SASL_LOGS are set explicitly.<para/>
        /// Name: RABBITMQ_LOG_BASE<para/>
        /// Default: %RABBITMQ_BASE%\log
        /// </summary>
        [DisplayName("RABBITMQ_LOG_BASE"), IoPath(IsDirectory = true)]
        string RabbitMqLogBase { get; set; }
        /// <summary>
        /// Name: RABBITMQ_LOGS<para/>
        /// Default: %RABBITMQ_LOG_BASE%\%RABBITMQ_NODENAME%.log<para/>
        /// The path of the RabbitMQ server's Erlang log file. 
        /// This variable cannot be overridden on Windows.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_LOGS"), IoPath]
        string RabbitMqLogs { get; set; }
        /// <summary>
        /// Name: RABBITMQ_SASL_LOGS<para/>
        /// Default: %RABBITMQ_LOG_BASE%\%RABBITMQ_NODENAME%-sasl.log<para/>
        /// The path of the RabbitMQ server's Erlang SASL (System Application Support Libraries) log file. 
        /// This variable cannot be overridden on Windows.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_SASL_LOGS"), IoPath]
        string RabbitMqSaslLogs { get; set; }
        /// <summary>
        /// Name: RABBITMQ_PLUGINS_DIR<para/>
        /// Default: Installation-directory/plugins<para/>
        /// The directory in which the plugins are found.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_PLUGINS_DIR"), IoPath(IsDirectory = true)]
        string RabbitMqPluginsDir { get; set; }
        /// <summary>
        /// Name: RABBITMQ_PLUGINS_EXPAND_DIR<para/>
        /// Default: %RABBITMQ_MNESIA_BASE%\%RABBITMQ_NODENAME%-plugins-expand<para/>
        /// Working directory used to expand enabled plugins when starting the server.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_PLUGINS_EXPAND_DIR"), IoPath(IsDirectory = true)]
        string RabbitMqPluginsExpandDir { get; set; }
        /// <summary>
        /// Name: RABBITMQ_ENABLED_PLUGINS_FILE<para/>
        /// Default: %RABBITMQ_BASE%\enabled_plugins<para/>
        /// This file records explicitly enabled plugins.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_ENABLED_PLUGINS_FILE"), IoPath]
        string RabbitMqEnabledPluginsFile { get; set; }
        /// <summary>
        /// Name: RABBITMQ_PID_FILE	(Not currently supported)<para/>
        /// File in which the process id is placed for use by rabbitmqctl wait.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_PID_FILE"), IoPath]
        string RabbitMqPidFile { get; set; }

        /// <summary>
        /// Name: RABBITMQ_NODE_IP_ADDRESS<para/>
        /// Default: the empty string - meaning bind to all network interfaces.<para/>
        /// Change this if you only want to bind to one network interface. 
        /// To bind to two or more interfaces, use thetcp_listeners key inrabbitmq.config.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_NODE_IP_ADDRESS")]
        string RabbitMqNodeIpAddress { get; set; }
        /// <summary>
        /// Name: RABBITMQ_NODE_PORT<para/>
        /// Default: 5672<para/>
        /// The port for RabbitMq.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_NODE_PORT")]
        string RabbitMqNodePort { get; set; }
        /// <summary>
        /// Name: RABBITMQ_DIST_PORT<para/>
        /// Default: RABBITMQ_NODE_PORT + 20000<para/>
        /// Port to use for clustering. 
        /// Ignored if your config file setsinet_dist_listen_min orinet_dist_listen_max<para/>
        /// </summary>
        [DisplayName("RABBITMQ_DIST_PORT")]
        string RabbitMqDistPort { get; set; }
        /// <summary>
        /// Name: RABBITMQ_NODENAME<para/>
        /// Default: Windows:rabbit@%COMPUTERNAME%<para/>
        /// The node name should be unique per erlang-node-and-machine combination. 
        /// To run multiple nodes, see the clustering guide.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_NODENAME")]
        string RabbitMqNodename { get; set; }
        /// <summary>
        /// Name: RABBITMQ_USE_LONGNAME<para/>
        /// Default: None<para/>
        /// When set to true this will cause RabbitMQ to use fully qualified names to identify nodes. 
        /// This may prove useful on EC2. 
        /// Note that it is not possible to switch between using short and long names without resetting the node.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_USE_LONGNAME")]
        string RabbitMqUseLongname { get; set; }
        /// <summary>
        /// Name: RABBITMQ_SERVICENAME<para/>
        /// Default: Windows Service: RabbitMQ<para/>
        /// The name of the installed service. 
        /// This will appear in services.msc.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_SERVICENAME")]
        string RabbitMqServicename { get; set; }
        /// <summary>
        /// Name: RABBITMQ_CONSOLE_LOG<para/>
        /// Default: Windows Service:<para/>
        /// Set this variable to new or reuse to redirect console output from the server to a file named %RABBITMQ_SERVICENAME%.debug in the default RABBITMQ_BASEdirectory.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_CONSOLE_LOG")]
        string RabbitMqConsoleLog { get; set; }
        /// <summary>
        /// Name: RABBITMQ_SERVER_ERL_ARGS<para/>
        /// Default: None<para/>
        /// Standard parameters for the erlcommand used when invoking the RabbitMQ Server. 
        /// This should be overridden for debugging purposes only. 
        /// Overriding this variable replacesthe default value.<para/>
        /// </summary>
        [DisplayName("RABBITMQ_SERVER_ERL_ARGS")]
        string RabbitMqServerErlArgs { get; set; }
        /// <summary>
        /// Name: RABBITMQ_SERVER_ADDITIONAL_ERL_ARGS<para/>
        /// Default: None<para/>
        /// Additional parameters for the erlcommand used when invoking the RabbitMQ Server. 
        /// The value of this variable is appended the default list of arguments (RABBITMQ_SERVER_ERL_ARGS).<para/>
        /// </summary>
        [DisplayName("RABBITMQ_SERVER_ADDITIONAL_ERL_ARGS")]
        string RabbitMqServerAdditionalErlArgs { get; set; }
        /// <summary>
        /// Name: RABBITMQ_SERVER_START_ARGS<para/>
        /// Default: None<para/>
        /// </summary>
        [DisplayName("RABBITMQ_SERVER_START_ARGS")]
        string RabbitMqServerStartArgs { get; set; }
    }
    public class RabbitMqEnvironmentVariables : IRabbitMqEnvironmentVariables
    {
        public RabbitMqEnvironmentVariables()
        {
            var root = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            RabbitMqBase = Path.Combine(root, "RabbitMQ");

            if (Directory.Exists(RabbitMqBase)) Directory.Delete(RabbitMqBase, true);
            Directory.CreateDirectory(RabbitMqBase);
        }
        public string RabbitMqBase { get; set; }
        public string RabbitMqConfigFile { get; set; }
        public string RabbitMqMnesiaBase { get; set; }
        public string RabbitMqMnesiaDir { get; set; }
        public string RabbitMqLogBase { get; set; }
        public string RabbitMqLogs { get; set; }
        public string RabbitMqSaslLogs { get; set; }
        public string RabbitMqPluginsDir { get; set; }
        public string RabbitMqPluginsExpandDir { get; set; }
        public string RabbitMqEnabledPluginsFile { get; set; }
        public string RabbitMqPidFile { get; set; }

        public string RabbitMqNodeIpAddress { get; set; }
        public string RabbitMqNodePort { get; set; }
        public string RabbitMqDistPort { get; set; }
        public string RabbitMqNodename { get; set; }
        public string RabbitMqUseLongname { get; set; }
        public string RabbitMqServicename { get; set; }
        public string RabbitMqConsoleLog { get; set; }
        public string RabbitMqServerErlArgs { get; set; }
        public string RabbitMqServerAdditionalErlArgs { get; set; }
        public string RabbitMqServerStartArgs { get; set; }
    }
}
