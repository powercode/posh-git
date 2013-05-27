using System;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Model;
using PoshGit.Tests.TestHelpers;
using Xunit;
using System.Linq;

namespace PoshGit.Tests
{
    public class CommitsFixture : BaseFixture
    {
        [Fact]
        public void GetCommitsWithTestReference()
        {
            string path = CloneStandardTestRepo();
            using (var repo = new Repository(path))
            {
                // Hard reset and then remove untracked files
                repo.Reset(ResetOptions.Hard);
                repo.RemoveUntrackedFiles();                

                using (var commitHelper = Model.GitCommitHelper.EnumerateCommits(path, new Filter{Since="test"}))
                {
                    Assert.Equal(2, commitHelper.Commits.Count());
                    var first = commitHelper.Commits.First();
                    Assert.Equal("e90810b8df3e80c413d903f631643c716887138d", first.Id.Sha);
                    Assert.Equal("Test commit 2", first.Subject);
                    Assert.Equal("tanoku@gmail.com", first.AuthorEmail);
                    Assert.Equal("Vicent Marti", first.AuthorName);
                    var dateTimeOffset = new DateTimeOffset(2010, 08, 05, 18, 42, 20, TimeSpan.FromHours(2));
                    Assert.Equal(dateTimeOffset, first.AuthorWhen);

                    Assert.Equal("tanoku@gmail.com", first.CommitterEmail);
                    Assert.Equal("Vicent Marti", first.CommitterName);
                    Assert.Equal(dateTimeOffset, first.CommitterWhen);
                }                
            }
        }

        [Fact]
        public void GetCommitsWithNoReference()
        {
            string path = CloneStandardTestRepo();
            using (var repo = new Repository(path))
            {
                // Hard reset and then remove untracked files
                repo.Reset(ResetOptions.Hard);
                repo.RemoveUntrackedFiles();

                using (var commitHelper = Model.GitCommitHelper.EnumerateCommits(path, new Filter { }))
                {
                    Assert.Equal(9, commitHelper.Commits.Count());
                    var first = commitHelper.Commits.First();
                    Assert.Equal("32eab9cb1f450b5fe7ab663462b77d7f4b703344", first.Id.Sha);
                    Assert.Equal(@"Add ""1.txt"" file beside ""1"" folder", first.Subject);
                    Assert.Equal("emeric.fermas@gmail.com", first.AuthorEmail);
                    Assert.Equal("nulltoken", first.AuthorName);
                    var dateTimeOffset = new DateTimeOffset(2011, 10, 31, 8, 52, 17, TimeSpan.FromHours(1));
                    Assert.Equal(dateTimeOffset, first.AuthorWhen);

                    Assert.Equal("emeric.fermas@gmail.com", first.CommitterEmail);
                    Assert.Equal("nulltoken", first.CommitterName);
                    Assert.Equal(dateTimeOffset, first.CommitterWhen);
                }
            }
        }


        [Fact]
        public void GetCommitsWithCmdlet()
        {
            string path = CloneStandardTestRepo();            
            using(var ps = PowerShell.Create()){                
                ps.AddCommand("Get-GitCommit");
                ps.AddParameter("LiteralPath", path);
                var output = ps.Invoke<CommitData>();
                Assert.Equal(9, output.Count());
                 
                var first = output.First();
                Assert.Equal("32eab9cb1f450b5fe7ab663462b77d7f4b703344", first.Id.Sha);                
            }
        }

        [Fact]
        public void GetCmdletCommitsTestReference()
        {
            string path = CloneStandardTestRepo();
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitCommit");
                ps.AddParameter("LiteralPath", path);
                ps.AddParameter("Reference", "test");
                var output = ps.Invoke<CommitData>();
                Assert.Equal(2, output.Count());

                var first = output.First();
                Assert.Equal("e90810b8df3e80c413d903f631643c716887138d", first.Id.Sha);
            }
        }

        [Fact]
        public void GetCmdletCommitsFirstSkip()
        {
            string path = CloneStandardTestRepo();
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Get-GitCommit");
                ps.AddParameter("LiteralPath", path);
                ps.AddParameter("First", 4);
                ps.AddParameter("Skip", 2 );
                ps.AddParameter("Reference", "master");
                var output = ps.Invoke<CommitData>();
                Assert.Equal(4, output.Count());

                var first = output.First();
                Assert.Equal("4c062a6361ae6959e06292c1fa5e2822d9c96345", first.Id.Sha);
            }
        }
        
    }
}