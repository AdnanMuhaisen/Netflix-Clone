using Nest;

namespace Netflix_Clone.Domain.Documents
{
    public abstract class Document
    {
        [Number(NumberType.Integer,Name = "id")]
        public int Id { get; set; }
    }
}
