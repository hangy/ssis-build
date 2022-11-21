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

namespace SsisBuild.Core.ProjectManagement;

using System.Collections.Generic;
using System.IO;

public interface IProjectFile
{
    IReadOnlyDictionary<string, IParameter> Parameters { get; }
    ProtectionLevel ProtectionLevel { get; set; }

    void Initialize(string filePath, string password);
    void Initialize(Stream fileStream, string password);
    void Save(string filePath);
    void Save(Stream fileStream);
    void Save(string filePath, ProtectionLevel protectionLevel, string password);
    void Save(Stream fileStream, ProtectionLevel protectionLevel, string password);
}