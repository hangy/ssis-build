# SSISBuild

A set of utilities that allow to autonomously build a Visual Studio SSIS project (dtproj) into a deployment package (ispac), and deploy the package to an SSIS catalog. Project deployment model only. This set is distributed as a NuGet package and can installed as a [dotnet-tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools). Utilities do not use any Microsoft SSIS or Visual Studio components, so there is no additional installation is needed on the build server.

This is a port of [ssis-build](https://github.com/rtumaykin/ssis-build) as a dotnet-tool. If you need to use this as a PowerShell module, please use the original.

## `dotnet ssis build`

Command line utility that builds a deployment package from a Visual Studio Project File

### Build Syntax

`dotnet ssis build --project [Project File] --configuration <Value> [--output-folder <Value>] [--protection-level <Value>] [--password <Value>] [--new-password <Value>] [--release-notes <Value>] [--parameter '{"Name":"Value"}']`

### Build Switches

- **--project:**
  Full path to a SSIS project file (with dtproj extension). If a project file is not specified, ssisbuild searches current working directory for a file with dtproj extension and uses that file.

- **--configuration:**
  Required. Name of project configuration to use.

- **--output-folder**
  Full path to a folder where the ispac file will be created. If ommitted, then the ispac file will be created in the bin/&lt;Configuration&gt; subfolder of the project folder.

- **--protection-level:**
  Overrides current project protection level. Available values are `DontSaveSensitive`, `EncryptAllWithPassword`, `EncryptSensitiveWithPassword`.

- **--password:**
  Password to decrypt original project data if its current protection level is either `EncryptAllWithPassword` or `EncryptSensitiveWithPassword`, in which case the value should be supplied, otherwise build will fail.

- **--new-password:**
  Password to encrypt resulting deployment packageif its resulting protection level is either `EncryptAllWithPassword` or `EncryptSensitiveWithPassword`. If ommitted, the value of the **-Password** switch is used for encryption, unless original protection level was `DontSaveSensitive`. In this case the value should be supplied, otherwise build will fail.

- **--parameter:**
  Project or Package parameter. Name is a standard full parameter name including the scope. For example `Project::Parameter1`. During the build, these values will replace existing values regardless of what they were originally. Example:

  ```json
  { "UserName": "root" }
  ```

- **--release-notes:**
  Path to a release notes file. Supports simple or complex release notes format, as defined [here](http://fsharp.github.io/FAKE/apidocs/fake-releasenoteshelper.html).

### **Example:**

`dotnet ssis build --project sample.dtproj --configuration Release --parameter '{ "SampleParameter": "some value" }'`

## `dotnet ssis deploy`

A command line utility that deploys an SSIS deployment package to an SSIS catalog.

### Deployment Syntax

`dotnet ssis deploy [Ispac File] --connection <ConnectionString> --catalog <CatalogName> --folder <FolderName> --project <ProjectName> [--password <ProjectPassword>] [--erase-sensitive-info]`

### Deployment Switches

- **Ispac File:**
  Full path to an SSIS deployment file (with ispac extension). If a deployment file is not specified, ssisdeploy searches current working directory for a file with ispac extension and uses that file.

- **--connection:**
  Required. Full connection string of the target SQL Server instance.

- **--catalog:**
  Name of the SSIS Catalog on the target server. If not supplied, then SSISDB value is used.

- **--folder:**
  Required. Deployment folder within destination catalog.

- **--project:**
  Name of the project in the destination folder. If not supplied, then deployment file name is used.

- **--password**
  Password to decrypt sensitive data for deployment.

- **--erase-sensitive-info:**
  Option to remove all sensitive info from the deployment ispac and deploy all sensitive parameters separately. If not specified then sensitive data will not be removed.

### Example

`dotnet ssis deploy sample.ispac --connection "Data Source=dbserver\\instance;Integrated Security=True;" --catalog SSISDB --folder SampleFolder --project Sample --password xyz --erase-sensitive-info`

## Sample Build PowerShell Script

```pwsh
dotnet tool install -g SsisTool
dotnet tool ssis build \
  --protection-level DontSaveSensitive \
  --configuration Deployment \
  --project .\SampleSSISProject\SampleSSISProject\SampleSSISProject.dtproj
```
