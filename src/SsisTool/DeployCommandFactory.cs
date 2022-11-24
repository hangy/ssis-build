// SPDX-License-Identifier: Apache-2.0

namespace SsisTool;

using SsisBuild.Core.Deployer;
using System.CommandLine;

internal static class DeployCommandFactory
{
    public static Command CreateDeployCommand()
    {
        var workingFolderOption = new Option<DirectoryInfo>("--working-folder", () => new DirectoryInfo(Environment.CurrentDirectory), "Path to working folder");
        var fileArgument = new Argument<FileInfo>("file", "Full path to an SSIS deployment file (with ispac extension). If a deployment file is not specified, ssisdeploy searches current working directory for a file with ispac extension and uses that file.");
        var serverInstanceOption = new Option<string>("--server", "Full Name of the target SQL Server instance.") { IsRequired = true };
        var serverInstanceUserIdOption = new Option<string>("--server-user", "User in case SQL Server Authentication is used (e.g. SSIS in ADFv2)");
        var serverInstancePasswordOption = new Option<string>("--server-password", "Password in case SQL Server Authentication is used (e.g. SSIS in ADFv2)");
        var catalogOption = new Option<string>("--catalog", "Name of the SSIS Catalog on the target server.") { IsRequired = true };
        var folderOption = new Option<string>("--folder", "Deployment folder within destination catalog.") { IsRequired = true };
        var projectNameOption = new Option<string>("--project", "Name of the project in the destination folder.") { IsRequired = true };
        var projectPasswordOption = new Option<string?>("--password", "Password to decrypt sensitive data for deployment.");
        var eraseSensitiveInfoOption = new Option<bool>("--erase-sensitive-info", () => false, "Option to remove all sensitive info from the deployment ispac and deploy all sensitive parameters separately. If not specified then sensitive data will not be removed.");

        var command = new Command("deploy", "Deploys an Ispac file to an SSIS Catalog.")
        {
            fileArgument,
            serverInstanceOption,
            serverInstanceUserIdOption,
            serverInstancePasswordOption,
            catalogOption,
            folderOption,
            projectNameOption,
            projectPasswordOption,
            eraseSensitiveInfoOption,
            workingFolderOption
        };
        command.SetHandler(deployArguments =>
        {
            new Deployer().Deploy(deployArguments);
        },
        new DeployArgumentsBinder(fileArgument,
                                  serverInstanceOption,
                                  serverInstanceUserIdOption,
                                  serverInstancePasswordOption,
                                  catalogOption,
                                  folderOption,
                                  projectNameOption,
                                  projectPasswordOption,
                                  eraseSensitiveInfoOption,
                                  workingFolderOption));
        return command;
    }
}
