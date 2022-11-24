// SPDX-License-Identifier: Apache-2.0

using SsisTool;
using System.CommandLine;

var deployCommand = new Command("deploy", "Deploy a built SSIS package");

var rootCommand = new RootCommand
{
    BuildCommandFactory.CreateBuildCommand(),
    deployCommand
};

return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
