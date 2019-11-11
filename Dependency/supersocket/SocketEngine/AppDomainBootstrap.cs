﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Provider;
using SuperSocket.SocketEngine.Configuration;

namespace SuperSocket.SocketEngine
{
    /// <summary>
    /// AppDomainBootstrap
    /// </summary>
    public sealed class AppDomainBootstrap : MarshalByRefObject, IBootstrap
    {
        class AppDomainWorkItemFactoryInfoLoader : WorkItemFactoryInfoLoader
        {
            class TypeValidator : MarshalByRefObject
            {
                public bool ValidateTypeName(string typeName)
                {
                    Type type = null;

                    try
                    {
                        type = Type.GetType(typeName, false);
                    }
                    catch
                    {

                    }

                    return type != null;
                }
            }

            public AppDomainWorkItemFactoryInfoLoader(IConfigurationSource config, ILogFactory passedInLogFactory)
                : base(config, passedInLogFactory)
            {
                InitliazeValidationAppDomain();
            }

            public AppDomainWorkItemFactoryInfoLoader(IConfigurationSource config)
                : base(config)
            {
                InitliazeValidationAppDomain();
            }

            private AppDomain m_ValidationAppDomain;

            private TypeValidator m_Validator; 

            private void InitliazeValidationAppDomain()
            {
                m_ValidationAppDomain = AppDomain.CreateDomain("ValidationDomain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.BaseDirectory, string.Empty, false);

                var validatorType = typeof(TypeValidator);
                m_Validator = (TypeValidator)m_ValidationAppDomain.CreateInstanceAndUnwrap(validatorType.Assembly.FullName, validatorType.FullName);
            }

            protected override string ValidateProviderType(string typeName)
            {
                if (!m_Validator.ValidateTypeName(typeName))
                    throw new Exception(string.Format("Failed to load type {0}!", typeName));

                return typeName;
            }

            public override void Dispose()
            {
                if (m_ValidationAppDomain != null)
                {
                    AppDomain.Unload(m_ValidationAppDomain);
                    m_ValidationAppDomain = null;
                }

                base.Dispose();
            }
        }

        class DefaultBootstrapAppDomainWrap : DefaultBootstrap
        {
            private AppDomainBootstrap m_AppDomainBootstrap;

            public DefaultBootstrapAppDomainWrap(AppDomainBootstrap appDomainBootstrap, IConfigurationSource config, string startupConfigFile)
                : base(config, startupConfigFile)
            {
                m_AppDomainBootstrap = appDomainBootstrap;
            }

            protected override IWorkItem CreateWorkItemInstance(string serviceTypeName)
            {
                return new AppDomainAppServer(serviceTypeName);
            }

            internal override bool SetupWorkItemInstance(IWorkItem workItem, WorkItemFactoryInfo factoryInfo)
            {
                return workItem.Setup(m_AppDomainBootstrap, factoryInfo.Config, factoryInfo.ProviderFactories.ToArray());
            }

            internal override WorkItemFactoryInfoLoader GetWorkItemFactoryInfoLoader(IConfigurationSource config, ILogFactory logFactory)
            {
                return new AppDomainWorkItemFactoryInfoLoader(config, logFactory);
            }
        }

        private IBootstrap m_InnerBootstrap;

        /// <summary>
        /// Gets all the app servers running in this bootstrap
        /// </summary>
        public IEnumerable<IWorkItem> AppServers
        {
            get { return m_InnerBootstrap.AppServers; }
        }

        /// <summary>
        /// Gets the config.
        /// </summary>
        public IRootConfig Config
        {
            get { return m_InnerBootstrap.Config; }
        }

        /// <summary>
        /// Gets the startup config file.
        /// </summary>
        public string StartupConfigFile
        {
            get { return m_InnerBootstrap.StartupConfigFile; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDomainBootstrap"/> class.
        /// </summary>
        public AppDomainBootstrap(IConfigurationSource config)
        {
            string startupConfigFile = string.Empty;

            if (config == null)
                throw new ArgumentNullException("config");

            var configSectionSource = config as ConfigurationSection;

            if (configSectionSource != null)
                startupConfigFile = configSectionSource.GetConfigSource();

            //Keep serializable version of configuration
            if(!config.GetType().IsSerializable)
                config = new ConfigurationSource(config);

            //Still use raw configuration type to bootstrap
            m_InnerBootstrap = new DefaultBootstrapAppDomainWrap(this, config, startupConfigFile);
        }

        /// <summary>
        /// Initializes the bootstrap with the configuration
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            return m_InnerBootstrap.Initialize();
        }

        /// <summary>
        /// Initializes the bootstrap with the configuration and config resolver.
        /// </summary>
        /// <param name="serverConfigResolver">The server config resolver.</param>
        /// <returns></returns>
        public bool Initialize(Func<IServerConfig, IServerConfig> serverConfigResolver)
        {
            return m_InnerBootstrap.Initialize(serverConfigResolver);
        }

        /// <summary>
        /// Initializes the bootstrap with the configuration and config resolver.
        /// </summary>
        /// <param name="logFactory">The log factory.</param>
        /// <returns></returns>
        public bool Initialize(ILogFactory logFactory)
        {
            return m_InnerBootstrap.Initialize(logFactory);
        }

        /// <summary>
        /// Initializes the bootstrap with a listen endpoint replacement dictionary
        /// </summary>
        /// <param name="listenEndPointReplacement">The listen end point replacement.</param>
        /// <returns></returns>
        public bool Initialize(IDictionary<string, System.Net.IPEndPoint> listenEndPointReplacement)
        {
            return m_InnerBootstrap.Initialize(listenEndPointReplacement);
        }

        /// <summary>
        /// Initializes the bootstrap with the configuration
        /// </summary>
        /// <param name="serverConfigResolver">The server config resolver.</param>
        /// <param name="logFactory">The log factory.</param>
        /// <returns></returns>
        public bool Initialize(Func<IServerConfig, IServerConfig> serverConfigResolver, ILogFactory logFactory)
        {
            if (logFactory != null)
                throw new Exception("You cannot pass in logFactory, if your isolation level is AppDomain!");

            return m_InnerBootstrap.Initialize(serverConfigResolver, logFactory);
        }

        /// <summary>
        /// Starts this bootstrap.
        /// </summary>
        /// <returns></returns>
        public StartResult Start()
        {
            return m_InnerBootstrap.Start();
        }

        /// <summary>
        /// Stops this bootstrap.
        /// </summary>
        public void Stop()
        {
            m_InnerBootstrap.Stop();
        }
    }
}
