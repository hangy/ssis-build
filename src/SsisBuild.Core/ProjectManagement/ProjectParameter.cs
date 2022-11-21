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

using SsisBuild.Core.ProjectManagement.Helpers;
using System;
using System.Xml;

public sealed class ProjectParameter : Parameter
{
    public ProjectParameter(string scopeName, XmlNode parameterNode) : base(scopeName, parameterNode, ParameterSource.Original)
    {
        InitializeFromXml();
    }

    protected override void InitializeFromXml()
    {
        if (ScopeName == null)
            throw new ArgumentNullException(nameof(ScopeName));

        var namespaceManager = ParameterNode.GetNameSpaceManager();
        var name = XmlHelpers.GetAttributeNode(ParameterNode, "SSIS:Name")?.Value;
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidXmlException("SSIS:Name attribute can not be null or empty", ParameterNode);

        if (ParameterNode.SelectSingleNode("./SSIS:Properties", namespaceManager) is not XmlElement propertiesXmlElement)
            throw new InvalidXmlException("Could not find collection of parameter properties", ParameterNode);

        var valueXmlElement = propertiesXmlElement.SelectSingleNode("./SSIS:Property[@SSIS:Name = \"Value\"]", namespaceManager) as XmlElement;
        var value = valueXmlElement?.InnerText;

        Name = $"{ScopeName}::{name}";
        ParentElement = propertiesXmlElement;
        ValueElement = valueXmlElement;
        ParameterDataType = ExtractDataType();
        Value = value;
        Sensitive = ParentElement.SelectSingleNode("./SSIS:Property[@SSIS:Name = \"Sensitive\"]", ParentElement.GetNameSpaceManager())?.InnerText == "1";

        if (valueXmlElement == null)
        {
            ValueElement = ParentElement.GetDocument().CreateElement("SSIS:Property", XmlHelpers.Schemas.SSIS);
            ValueElement.SetAttribute("Name", XmlHelpers.Schemas.SSIS, "Value");
            if (Sensitive)
                ValueElement.SetAttribute("Sensitive", XmlHelpers.Schemas.SSIS, "1");
        }
        else
        {
            ValueElement = valueXmlElement;
        }
    }

    private Type ExtractDataType()
    {
        if (!Enum.TryParse(ParentElement.SelectSingleNode("./SSIS:Property[@SSIS:Name = \"DataType\"]", ParentElement.GetNameSpaceManager())?.InnerText, out DataType parameterDataType))
            return null;

        switch (parameterDataType)
        {
            case DataType.Boolean:
                return typeof(bool);

            case DataType.Byte:
                return typeof(byte);

            case DataType.DateTime:
                return typeof(DateTime);

            case DataType.Decimal:
                return typeof(decimal);

            case DataType.Double:
                return typeof(double);

            case DataType.Int16:
                return typeof(short);

            case DataType.Int32:
                return typeof(int);

            case DataType.Int64:
                return typeof(long);

            case DataType.SByte:
                return typeof(sbyte);

            case DataType.Single:
                return typeof(float);

            case DataType.String:
                return typeof(string);

            case DataType.UInt32:
                return typeof(uint);

            case DataType.UInt64:
                return typeof(ulong);

            default:
                return null;
        }
    }
}
