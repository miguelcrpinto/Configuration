﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Framework.Configuration
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly IList<IConfigurationProvider> _sources = new List<IConfigurationProvider>();

        public ConfigurationBuilder(params IConfigurationSource[] sources)
        {
            if (sources != null)
            {
                foreach (var singleSource in sources)
                {
                    Add(singleSource);
                }
            }
        }

        public IEnumerable<IConfigurationProvider> Sources
        {
            get
            {
                return _sources;
            }
        }

        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Adds a new configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source to add.</param>
        /// <returns>The same configuration source.</returns>
        public IConfigurationBuilder Add(IConfigurationProvider configurationSource)
        {
            return Add(configurationSource, load: true);
        }

        /// <summary>
        /// Adds a new configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source to add.</param>
        /// <param name="load">If true, the configuration source's <see cref="IConfigurationProvider.Load"/> method will
        ///  be called.</param>
        /// <returns>The same configuration source.</returns>
        /// <remarks>This method is intended only for test scenarios.</remarks>
        public IConfigurationBuilder Add(IConfigurationProvider configurationSource, bool load)
        {
            if (load)
            {
                configurationSource.Load();
            }
            _sources.Add(configurationSource);
            return this;
        }

        public IConfigurationRoot Build()
        {
            return new ConfigurationRoot(_sources);
        }
    }
}
