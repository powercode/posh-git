if (Get-Module posh-git) { return }

Import-Module $psScriptRoot\bin\PoshGit.dll

Push-Location $psScriptRoot
.\CheckVersion.ps1 > $null

. .\Utils.ps1
. .\GitUtils.ps1
. .\GitPrompt.ps1
. .\GitTabExpansion.ps1
. .\TortoiseGit.ps1
Pop-Location

if (!$Env:HOME) { $Env:HOME = "$Env:HOMEDRIVE$Env:HOMEPATH" }
if (!$Env:HOME) { $Env:HOME = "$Env:USERPROFILE" }

Get-TempEnv 'SSH_AGENT_PID'
Get-TempEnv 'SSH_AUTH_SOCK'


Set-Alias ggs Get-GitStatus
Set-Alias ggb Get-GitBranch
Set-Alias ggco Get-GitCommit
Set-Alias ggi Get-GitItem
Set-Alias ggc Get-GitContent


Export-ModuleMember -Alias * -Function * -Cmdlet *

