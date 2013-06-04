namespace PoshGit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Language;
    using System.Reflection;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    /// The git tab completer.
    /// </summary>
    public static class GitTabCompleter
    {
        /// <summary>
        /// Register tab completion functions for cmdlet parameters with HasTabCompleter attribute
        /// </summary>
        /// <param name="customCompleters">
        /// The custom completers.
        /// </param>
        public static void RegisterCustomCompleters(Hashtable customCompleters)
        {
            Contract.Requires(customCompleters != null);
            var scriptblock = ScriptBlock.Create(@"
    param(
		[string] $Command,
		[string] $ParameterName,
		[string] $WordToComplete,	
		[System.Management.Automation.Language.CommandAst] $Ast,
		[System.Collections.Hashtable] $FakeBoundParameter
	)

	[PoshGit.GitTabCompleter]::Complete($Command, $ParameterName, $WordToComplete, $Ast, $FakeBoundParameter, $ExecutionContext)
");
            var cmdletTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where t.GetCustomAttribute<CmdletAttribute>() != null
                                   select t;

            var keys = from t in cmdletTypes
                      let ca = t.GetCustomAttribute<CmdletAttribute>()
                      let cmdletName = string.Format("{0}-{1}", ca.VerbName, ca.NounName)
                      from p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      where p.GetCustomAttribute<HasTabCompleterAttribute>() != null
                      select string.Format(CultureInfo.InvariantCulture, "{0}:{1}", cmdletName, p.Name);

            foreach (var key in keys)
            {
                customCompleters.Add(key, scriptblock);
            }
        }

        /// <summary>
        /// The get git completions.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="parameterName">
        /// The parameter name.
        /// </param>
        /// <param name="wordToComplete">
        /// The word to complete.
        /// </param>
        /// <param name="ast">
        /// The abstract syntax tree.
        /// </param>
        /// <param name="fakeBoundParameters">
        /// a Hashtable with the parameters already specified on the command line.
        /// </param>
        /// <param name="intrinsics">provides access to PowerShell session state</param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<CompletionResult> Complete(string command, string parameterName, string wordToComplete, CommandAst ast, Hashtable fakeBoundParameters, EngineIntrinsics intrinsics)
        {
            Contract.Requires(!string.IsNullOrEmpty(command));
            Contract.Requires(!string.IsNullOrEmpty(parameterName));
            Contract.Requires(intrinsics != null);
            Contract.Requires(fakeBoundParameters != null);
            var currentDirectory = intrinsics.SessionState.Path.CurrentFileSystemLocation.ProviderPath;
            var param = command + ":" + parameterName;          

            switch (param)
            {
                case "Remove-GitBranch:Name":
                case "Switch-GitBranch:Branch":
                    return GetLocalBranchesCompletions(currentDirectory, wordToComplete, excludeCurrent: true);

                case "New-GitBranch:Reference":
                    return GetReferences(currentDirectory, wordToComplete);
                
                case "Rename-GitBranch:Name":                
                    return GetLocalBranchesCompletions(currentDirectory, wordToComplete, excludeCurrent: false);
                
                case "Add-GitItem:Path":
                    return GetModifiedItems(currentDirectory, wordToComplete);

                case "Remove-GitItem:Path":
                    return fakeBoundParameters.ContainsKey("Cached")
                               ? GetStagedItems(currentDirectory, wordToComplete)
                               : null;

                case "Publish-GitBranch:Remote":
                    return GetRemotesCompletions(currentDirectory, wordToComplete);

                case "Publish-GitBranch:Reference":
                    return GetLocalBranchesCompletions(currentDirectory, wordToComplete, false);
                                
                default:
                    return null;
            }
        }

        /// <summary>
        /// The get remotes completions.
        /// </summary>
        /// <param name="currentDirectory">
        /// The current directory.
        /// </param>
        /// <param name="wordToComplete">
        /// The word to complete.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>        
        private static IEnumerable<CompletionResult> GetRemotesCompletions(string currentDirectory, string wordToComplete)
        {
            var repo = GitRepositoryFactory.Instance.GetRepository(currentDirectory);
            return from r in repo.Network.Remotes
                   where string.IsNullOrEmpty(wordToComplete) || r.Name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
                   select new CompletionResult(r.Name, r.Name, CompletionResultType.ParameterValue, r.Url);
        }

        /// <summary>
        /// The get references.
        /// </summary>
        /// <param name="workingDirectory">
        /// The working directory.
        /// </param>
        /// <param name="wordToComplete">
        /// The word to complete.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<CompletionResult> GetReferences(string workingDirectory, string wordToComplete)
        {
            
            var repo = GitRepositoryFactory.Instance.GetRepository(workingDirectory);
            return from r in repo.Refs
                       let abbr = TrimReferenceName(r.CanonicalName)
                       where abbr.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
                       select new CompletionResult(abbr, abbr, CompletionResultType.ParameterValue, r.CanonicalName);                 
        }

        /// <summary>
        /// Removes refs/ and refs/remote/ from the start of the reference name
        /// </summary>
        /// <param name="canonicalName">
        /// The canonical name.
        /// </param>
        /// <returns>
        /// The abbreviated string<see cref="string"/>.
        /// </returns>
        private static string TrimReferenceName(string canonicalName)
        {
            Contract.Requires(!string.IsNullOrEmpty(canonicalName));
            Contract.Assume(canonicalName.Length > 10);
            switch (canonicalName[5])
            {
                case 'h': // refs/heads/
                    return canonicalName.Substring(11);
                case 't': // refs/tags/
                    return canonicalName.Substring(10);
                case 'r': // refs/remotes/
                    return canonicalName.Substring(13);
                default:
                    return canonicalName;
            }            
        }

        /// <summary>
        /// The get staged items completion result.
        /// </summary>
        /// <param name="workingDirectory">
        /// The working directory.
        /// </param>
        /// <param name="wordToComplete">
        /// The word to complete.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<CompletionResult> GetStagedItems(string workingDirectory, string wordToComplete)
        {
            var repo = GitRepositoryFactory.Instance.GetRepository(workingDirectory);
            return from s in repo.Index.RetrieveStatus()
                   where (s.State.HasFlag(FileStatus.Staged) || 
                            s.State.HasFlag(FileStatus.StagedTypeChange)) &&
                          s.FilePath.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
                   select new CompletionResult(s.FilePath, s.FilePath, CompletionResultType.ParameterValue, s.FilePath);
        }

        /// <summary>
        /// The get modified items completion result.
        /// </summary>
        /// <param name="workingDirectory">
        /// The working directory.
        /// </param>
        /// <param name="wordToComplete">
        /// The word to complete.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<CompletionResult> GetModifiedItems(string workingDirectory, string wordToComplete)
        {            
            var repo = GitRepositoryFactory.Instance.GetRepository(workingDirectory);
            return from s in repo.Index.RetrieveStatus()
                   where s.State.HasFlag(FileStatus.Modified) &&
                         s.FilePath.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
                   select new CompletionResult(s.FilePath, s.FilePath, CompletionResultType.ParameterValue, s.FilePath);
        }

        /// <summary>
        /// The completion for local branches except the current.
        /// </summary>
        /// <param name="path">
        /// The full path of the working directory.
        /// </param>
        /// <param name="wordToComplete">
        /// The current word being completed
        /// </param>
        /// <param name="excludeCurrent">
        /// Exclude the current branch if true
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>        
        private static IEnumerable<CompletionResult> GetLocalBranchesCompletions(string path, string wordToComplete, bool excludeCurrent)
        {
            var repo = GitRepositoryFactory.Instance.GetRepository(path);
            return from b in repo.Branches
                   where !b.IsRemote && (excludeCurrent || !b.IsCurrentRepositoryHead)
                       && (string.IsNullOrEmpty(wordToComplete) || b.Name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
                   select new CompletionResult(b.Name, b.Name, CompletionResultType.ParameterValue, b.CanonicalName);
        }
    }
}
