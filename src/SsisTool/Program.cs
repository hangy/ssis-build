// SPDX-License-Identifier: Apache-2.0

using SsisTool;
using System.CommandLine;

var rootCommand = new RootCommand
{
    BuildCommandFactory.CreateBuildCommand(),
    DeployCommandFactory.CreateDeployCommand()
};

return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
