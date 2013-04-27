using LibGit2Sharp;

namespace PoshGit.Model
{
    public class TagData
    {
        public TagData(Tag tag)
        {
            Target = tag.Target.Id;
            Name = tag.Name;
            CanonicalName = tag.CanonicalName;
            if (tag.IsAnnotated)
            {
                Message = tag.Annotation.Message;
                Tagger = tag.Annotation.Tagger;
            }

        }

        
        public string Name { get; private set; }
        public string CanonicalName { get; private set; }
        public string Message { get; private set; }
        public ObjectId Target { get; set; }
        public Signature Tagger { get; private set; }        

    }
}