namespace PoshGit
{
    using System;

    /// <summary>
    /// The tab completion provider attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HasTabCompleterAttribute : Attribute
    {                
    }
}
