﻿using System;
using Gallio.Runtime;
using Gallio.Runtime.Logging;
using Gallio.Runtime.Extensibility;
using Gallio.Runtime.Loader;
using System.Diagnostics;
using Gallio.Common.Policies;

namespace Gallio.Icarus.Runtime
{
    internal static class RuntimeBootstrap
    {
        /// <summary>
        /// <para>
        /// Initializes the runtime.
        /// </para>
        /// <para>
        /// Loads plugins and initalizes the runtime component model.  The
        /// specifics of the system can be configured by editing the appropriate
        /// *.plugin files to register new components and facilities as required.
        /// </para>
        /// </summary>
        /// <param name="setup">The runtime setup parameters</param>
        /// <param name="logger">The runtime logging service</param>
        /// <returns>An object that when disposed automatically calls <see cref="Shutdown" />.
        /// This is particularly useful in combination with the C# "using" statement
        /// or its equivalent.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="setup"/> or <paramref name="logger"/> is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if the runtime has already been initialized</exception>
        public static IDisposable Initialize(RuntimeSetup setup, ILogger logger)
        {
            if (setup == null)
                throw new ArgumentNullException("setup");
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (RuntimeAccessor.IsInitialized)
                throw new InvalidOperationException("The runtime has already been initialized.");

            var registry = new Registry();
            var pluginLoader = new CachingPluginLoader(); //new PluginLoader();
            var assemblyResolverManager = new DefaultAssemblyResolverManager();
            IRuntime runtime = new IcarusRuntime(registry, pluginLoader, assemblyResolverManager, setup); // TODO: make me configurable via setup
            Debug.Assert(runtime != null, "The runtime returned by the runtime factory must not be null.");

            try
            {
                RuntimeAccessor.SetRuntime(runtime);
                runtime.Initialize(logger);

                if (!UnhandledExceptionPolicy.HasReportUnhandledExceptionHandler)
                    UnhandledExceptionPolicy.ReportUnhandledException += HandleUnhandledExceptionNotification;
            }
            catch (Exception)
            {
                RuntimeAccessor.SetRuntime(null);
                throw;
            }

            return new AutoShutdown();
        }

        /// <summary>
        /// Shuts down the runtime if it is currently initialized.
        /// Does nothing if the runtime has not been initialized.
        /// </summary>
        public static void Shutdown()
        {
            if (!RuntimeAccessor.IsInitialized)
                return;

            try
            {
                RuntimeAccessor.Instance.Dispose();
            }
            finally
            {
                UnhandledExceptionPolicy.ReportUnhandledException -= HandleUnhandledExceptionNotification;

                RuntimeAccessor.SetRuntime(null);
            }
        }

        private static void HandleUnhandledExceptionNotification(object sender, CorrelatedExceptionEventArgs e)
        {
            if (e.IsRecursive)
                return;

            RuntimeAccessor.Logger.Log(LogSeverity.Error, "Internal error: " + e.GetDescription());
        }

        private sealed class AutoShutdown : IDisposable
        {
            public void Dispose()
            {
                Shutdown();
            }
        }
    }
}