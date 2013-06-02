Import-Module ..\..\posh-git
Import-Module PSCompletion

cd C:\Users\Staffan\Documents\GitHub
if (test-path libgit2sharp_clone)
{
	REmove-Item -re -fo libgit2sharp_clone
}
#Copy-GitRepository libgit2sharp libgit2sharp_clone
cd posh-git