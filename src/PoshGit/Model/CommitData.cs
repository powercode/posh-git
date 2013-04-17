﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LibGit2Sharp;

namespace PoshGit.Model
{
    public class CommitData
    {
        public CommitData(Commit commit)
        {
            Message = commit.Message;
            Subject = commit.MessageShort;
            Parents = new ReadOnlyCollection<ObjectId>(commit.Parents.Select(c => c.Id).ToList());
            CommitterWhen = commit.Committer.When;
            CommitterEmail = commit.Committer.Email;
            CommitterName = commit.Committer.Name;
            AuthorWhen = commit.Author.When;
            AuthorEmail = commit.Author.Email;
            AuthorName = commit.Author.Name;
            Id = commit.Id;
            Encoding = commit.Encoding;            
        }

        public ObjectId Id { get; private set; }
        public ReadOnlyCollection<ObjectId> Parents { get; private set; }        

        public string AuthorName { get; private set; }
        public string AuthorEmail { get; private set; }
        public DateTimeOffset AuthorWhen { get; private set; }

        public string Subject { get; private set; }
        public string Message { get; private set; }
        
        public string CommitterName { get; private set; }
        public string CommitterEmail { get; private set; }
        public DateTimeOffset CommitterWhen { get; private set; }

        public string Encoding { get; private set; }


        public override string ToString()
        {
            return Id.Sha.Substring(0, 10);
        }
    }
}