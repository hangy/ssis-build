// SPDX-License-Identifier: Apache-2.0

namespace SsisTool;

using SsisBuild.Core.Builder;
using SsisBuild.Core.ProjectManagement;
using System;
using System.CommandLine;
using System.CommandLine.Binding;

internal sealed class BuildArgumentsBinder : BinderBase<IBuildArguments>
{
    private readonly Option<FileInfo> _projectPathOption;

    private readonly Option<DirectoryInfo?> _outputFolderOption;

    private readonly Option<ProtectionLevel> _protectionLevelOption;

    private readonly Option<string> _configurationOption;

    private readonly Option<string?> _passwordOption;

    private readonly Option<string?> _newPasswordOption;

    private readonly Option<FileInfo?> _releaseNotesOption;

    private readonly Option<IDictionary<string, string>?> _parametersOption;

    private readonly Option<DirectoryInfo> _workingFolderOption;

    public BuildArgumentsBinder(Option<FileInfo> projectPathOption,
                                Option<DirectoryInfo?> outputFolderOption,
                                Option<ProtectionLevel> protectionLevelOption,
                                Option<string> configurationOption,
                                Option<string?> passwordOption,
                                Option<string?> newPasswordOption,
                                Option<FileInfo?> releaseNotesOption,
                                Option<IDictionary<string, string>?> parametersOption,
                                Option<DirectoryInfo> workingFolderOption)
    {
        _projectPathOption = projectPathOption ?? throw new ArgumentNullException(nameof(projectPathOption));
        _outputFolderOption = outputFolderOption ?? throw new ArgumentNullException(nameof(outputFolderOption));
        _protectionLevelOption = protectionLevelOption ?? throw new ArgumentNullException(nameof(protectionLevelOption));
        _configurationOption = configurationOption ?? throw new ArgumentNullException(nameof(workingFolderOption));
        _passwordOption = passwordOption ?? throw new ArgumentNullException(nameof(passwordOption));
        _newPasswordOption = newPasswordOption ?? throw new ArgumentNullException(nameof(newPasswordOption));
        _releaseNotesOption = releaseNotesOption ?? throw new ArgumentNullException(nameof(releaseNotesOption));
        _parametersOption = parametersOption ?? throw new ArgumentNullException(nameof(parametersOption));
        _workingFolderOption = workingFolderOption ?? throw new ArgumentNullException(nameof(workingFolderOption));
    }

    protected override IBuildArguments GetBoundValue(BindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        return new BuildArguments(
            bindingContext.ParseResult.GetValueForOption(_workingFolderOption)?.FullName,
            bindingContext.ParseResult.GetValueForOption(_projectPathOption)?.FullName,
            bindingContext.ParseResult.GetValueForOption(_outputFolderOption)?.FullName,
            bindingContext.ParseResult.GetValueForOption(_protectionLevelOption).ToString(),
            bindingContext.ParseResult.GetValueForOption(_passwordOption),
            bindingContext.ParseResult.GetValueForOption(_newPasswordOption),
            bindingContext.ParseResult.GetValueForOption(_configurationOption),
            bindingContext.ParseResult.GetValueForOption(_releaseNotesOption)?.FullName,
            bindingContext.ParseResult.GetValueForOption(_parametersOption));
    }
}
