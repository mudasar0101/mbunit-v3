// Copyright 2005-2010 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Runtime.Extensibility;

namespace Gallio.Runtime.UtilityCommands
{
    /// <summary>
    /// Provides services for managing utility commands.
    /// </summary>
    public interface IUtilityCommandManager
    {
        /// <summary>
        /// Gets handles for all registered utility commands.
        /// </summary>
        IList<ComponentHandle<IUtilityCommand, UtilityCommandTraits>> CommandHandles { get; }

        /// <summary>
        /// Gets the utility command with the specified name ignoring case, or null if not registered.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
        IUtilityCommand GetCommand(string name);
    }
}
