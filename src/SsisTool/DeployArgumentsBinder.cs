// SPDX-License-Identifier: Apache-2.0

namespace SsisTool;

using SsisBuild.Core.Deployer;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;

internal class DeployArgumentsBinder : BinderBase<DeployArguments>
{
    private readonly Argument<FileInfo> _fileArgument;

    private readonly Option<string> _serverInstanceOption;

    private readonly Option<string> _serverInstanceUserIdOption;

    private readonly Option<string> _serverInstancePasswordOption;

    private readonly Option<string> _catalogOption;

    private readonly Option<string> _folderOption;

    private readonly Option<string> _projectNameOption;

    private readonly Option<string?> _projectPasswordOption;

    private readonly Option<bool> _eraseSensitiveInfoOption;

    private readonly Option<DirectoryInfo> _workingFolderOption;

    public DeployArgumentsBinder(Argument<FileInfo> fileArgument, Option<string> serverInstanceOption, Option<string> serverInstanceUserIdOption, Option<string> serverInstancePasswordOption, Option<string> catalogOption, Option<string> folderOption, Option<string> projectNameOption, Option<string?> projectPasswordOption, Option<bool> eraseSensitiveInfoOption, Option<DirectoryInfo> workingFolderOption)
    {
        _fileArgument = fileArgument ?? throw new ArgumentNullException(nameof(fileArgument));
        _serverInstanceOption = serverInstanceOption ?? throw new ArgumentNullException(nameof(serverInstanceOption));
        _serverInstanceUserIdOption = serverInstanceUserIdOption ?? throw new ArgumentNullException(nameof(serverInstanceUserIdOption));
        _serverInstancePasswordOption = serverInstancePasswordOption ?? throw new ArgumentNullException(nameof(serverInstancePasswordOption));
        _catalogOption = catalogOption ?? throw new ArgumentNullException(nameof(catalogOption));
        _folderOption = folderOption ?? throw new ArgumentNullException(nameof(folderOption));
        _projectNameOption = projectNameOption ?? throw new ArgumentNullException(nameof(projectNameOption));
        _projectPasswordOption = projectPasswordOption ?? throw new ArgumentNullException(nameof(projectPasswordOption));
        _eraseSensitiveInfoOption = eraseSensitiveInfoOption ?? throw new ArgumentNullException(nameof(eraseSensitiveInfoOption));
        _workingFolderOption = workingFolderOption ?? throw new ArgumentNullException(nameof(workingFolderOption));
    }

    protected override DeployArguments GetBoundValue(BindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        return new DeployArguments(
            bindingContext.ParseResult.GetValueForOption(_workingFolderOption)?.FullName,
            bindingContext.ParseResult.GetValueForArgument(_fileArgument)?.FullName,
            bindingContext.ParseResult.GetValueForOption(_serverInstanceOption),
            bindingContext.ParseResult.GetValueForOption(_catalogOption),
            bindingContext.ParseResult.GetValueForOption(_folderOption),
            bindingContext.ParseResult.GetValueForOption(_projectNameOption),
            bindingContext.ParseResult.GetValueForOption(_projectPasswordOption),
            bindingContext.ParseResult.GetValueForOption(_eraseSensitiveInfoOption),
            bindingContext.ParseResult.GetValueForOption(_serverInstanceUserIdOption),
            bindingContext.ParseResult.GetValueForOption(_serverInstancePasswordOption));
    }
}