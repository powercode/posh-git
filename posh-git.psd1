#
# Module manifest for module 'posh-git'
#
# Generated by: Posh-Git contributors
#
# Generated on: 2013-04-16
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'posh-git.psm1'

# Version number of this module.
ModuleVersion = '1.0'

# ID used to uniquely identify this module
GUID = '34354fbd-3a88-4328-9ecf-794f6e4d3486'

# Author of this module
Author = 'Posh-Git contributors'

# Company or vendor of this module
CompanyName = 'Unknown'

# Copyright statement for this module
Copyright = '(c) 2013 Posh-Git contributors. All rights reserved.'

# Description of the functionality provided by this module
Description = 'PowerShell implementaion of git commands'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '3.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of the .NET Framework required by this module
DotNetFrameworkVersion = '4.0'

# Minimum version of the common language runtime (CLR) required by this module
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
#ProcessorArchitecture = 'None'

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @("$PSScriptRoot\bin\PoshGit.dll")

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @('posh-git.types.ps1xml')

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @('posh-git.formats.ps1xml')

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module
FunctionsToExport = @(
    'Invoke-NullCoalescing'
    'Write-GitStatus'
    'Write-Prompt'    
    'Enable-GitColors'
    'Get-GitDirectory'
    'TabExpansion'
    'Get-AliasPattern'
    'Get-SshAgent'
    'Start-SshAgent'
    'Stop-SshAgent'
    'Add-SshKey'
    'Get-SshPath'
    'Update-AllBranches'
    'tgit'
)

# Cmdlets to export from this module
CmdletsToExport = @(
	'Get-GitBranch'
	'Get-GitTag'
	'Get-GitLog'
	'Get-GitStatus'
	'Get-GitItem'
	'Get-GitContent'
	'New-GitRepository'
	'Copy-GitRepository'
	'Switch-GitBranch'
	'Add-GitItem'
	'Remove-GitItem'
	'Submit-GitIndex'
)

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = @(
	'ngr'
	'cpgr' 
	'ggb'
	'ggco'
	'ggi'
	'ggl'
	'ggs'
	'ggt'
	'swgb'
	'agi'
	'rgi'
	'sbgi'
	'??'
)

# List of all modules packaged with this module.
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess
# PrivateData = ''

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

