namespace PoshGit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Language;
    using System.Reflection;

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
            var scriptblock = ScriptBlock.Create(@"
    param(
		[string] $Command,
		[string] $ParameterName,
		[string] $WordToComplete,	
		[System.Management.Automation.Language.CommandAst] $Ast,
		[string] $FakeBoundParameter
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
        /// <param name="fakeBoundParameter">
        /// The fake bound parameter.
        /// </param>
        /// <param name="intrinsics">provides access to PowerShell session state</param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<CompletionResult> Complete(string command, string parameterName, string wordToComplete, CommandAst ast, string fakeBoundParameter, EngineIntrinsics intrinsics)
        {
            switch (command)
            {
                case "Switch-GitBranch":
                    switch (parameterName)
                    {
                        case "Branch":
                            return GetLocalBranchesCompletions(
                                intrinsics.SessionState.Path.CurrentFileSystemLocation.ProviderPath, 
                                wordToComplete);
                        default:
                            return null;
                    }     

                default:
                    return null;
            }            
        }

        /// <summary>
        /// The CompletionResults for all branches except the current one
        /// </summary>
        /// <param name="fullname">
        /// the path to find branches in.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<CompletionResult> GetAllBranches(string fullname)
        {            
            var repo = GitRepositoryFactory.Instance.GetRepository(fullname);
            return from b in repo.Branches
                       where !b.IsCurrentRepositoryHead
                       select new CompletionResult(b.Name, b.Name, CompletionResultType.ParameterValue, b.CanonicalName);
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
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>        
        public static IEnumerable<CompletionResult> GetLocalBranchesCompletions(string path, string wordToComplete)
        {
            var repo = GitRepositoryFactory.Instance.GetRepository(path);
            if (string.IsNullOrEmpty(wordToComplete))
            {
                return from b in repo.Branches
                       where !b.IsRemote && !b.IsCurrentRepositoryHead
                       select new CompletionResult(b.Name, b.Name, CompletionResultType.ParameterValue, b.CanonicalName);
            }

            return from b in repo.Branches
                   where
                       !b.IsRemote && !b.IsCurrentRepositoryHead
                       && b.Name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)
                   select new CompletionResult(b.Name, b.Name, CompletionResultType.ParameterValue, b.CanonicalName);
        }
    }
}
