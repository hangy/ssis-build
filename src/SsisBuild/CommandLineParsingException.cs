﻿//-----------------------------------------------------------------------
//   Copyright 2017 Roman Tumaykin
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//-----------------------------------------------------------------------

namespace SsisBuild;

using System;

/// <summary>
/// Base class for all ssisbuild.exe command line arguments parsing exceptions.
/// Derives from <see cref="Exception"/>.
/// </summary>
public class CommandLineParsingException : Exception
{
    /// <summary>
    /// Constructor for <see cref="CommandLineParsingException"/> class. 
    /// </summary>
    /// <param name="message">Exception message</param>
    public CommandLineParsingException(string message) : base(message) { }
}
