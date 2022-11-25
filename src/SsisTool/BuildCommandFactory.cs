// SPDX-License-Identifier: Apache-2.0

namespace SsisTool;

using SsisBuild.Core.Builder;
using SsisBuild.Core.ProjectManagement;
using System.CommandLine;
using System.Text.Json;

internal static class BuildCommandFactory
{
    public static Command CreateBuildCommand()
    {
        var workingFolderOption = new Option<DirectoryInfo>("--working-folder", () => new DirectoryInfo(Environment.CurrentDirectory), "Path to working folder");

        var projectPathOption = new Option<FileInfo>("--project", "Full path to a SSIS project file (with dtproj extension). If a project file is not specified, ssisbuild searches current working directory for a file with dtproj extension and uses that file.");
        var configurationOption = new Option<string>("--configuration", "Name of project configuration to use.") { IsRequired = true };

        var outputFolderOption = new Option<DirectoryInfo?>("--output-folder", "Full path to a folder where the ispac file will be created. If ommitted, then the ispac file will be created in the bin/<Configuration> subfolder of the project folder.");

        var protectionLevelOption = new Option<ProtectionLevel>("--protection-level", "Overrides current project protection level. Available values are DontSaveSensitive, EncryptAllWithPassword, EncryptSensitiveWithPassword.")
        { IsRequired = true }
        .FromAmong(nameof(ProtectionLevel.DontSaveSensitive), nameof(ProtectionLevel.EncryptSensitiveWithPassword), nameof(ProtectionLevel.EncryptSensitiveWithPassword));

        var passwordOption = new Option<string?>("--password", "Password to decrypt original project data if its current protection level is either EncryptAllWithPassword or EncryptSensitiveWithPassword, in which case the value should be supplied, otherwise build will fail.");
        var newPasswordOption = new Option<string?>("--new-password", "Password to encrypt resulting project if its resulting protection level is either EncryptAllWithPassword or EncryptSensitiveWithPassword. If ommitted, the value of the <Password> switch is used for encryption, unless original protection level was DontSaveSensitive, in which case the value should be supplied, otherwise build will fail.");
        var releaseNotesOption = new Option<FileInfo?>("--release-notes", "Path to a release notes file. File can have simple or complex release notes format.");

        var parametersOption = new Option<IDictionary<string, string>?>(name: "--parameters", description: "Project or Package parameter. Name is a standard full parameter name including the scope. For example Project::Parameter1. During the build, these values will replace existing values regardless of what these values were originally. Example: { \"UserName\": \"root\" }", parseArgument: result =>
        {
            const string error = "Parameter has to be a single JSON object";
            if (result.Tokens.Count != 1)
            {
                result.ErrorMessage = error;
                return null;
            }

            var value = JsonSerializer.Deserialize<IDictionary<string, string>?>(result.Tokens.Single().Value);
            if (value == null)
            {
                result.ErrorMessage = error;
                return null;
            }

            return value;
        });

        var command = new Command("build", "Builds an SSIS project into an ispac file. Project must not be encrypted by a user key.")
        {
            workingFolderOption,
            projectPathOption,
            configurationOption,
            outputFolderOption,
            protectionLevelOption,
            passwordOption,
            newPasswordOption,
            releaseNotesOption,
            parametersOption
        };
        command.SetHandler(buildArguments =>
        {
            new Builder().Build(buildArguments);
        },
        new BuildArgumentsBinder(projectPathOption,
                                 outputFolderOption,
                                 protectionLevelOption,
                                 configurationOption,
                                 passwordOption,
                                 newPasswordOption,
                                 releaseNotesOption,
                                 parametersOption,
                                 workingFolderOption));
        return command;
    }
}
