namespace Netflix_Clone.Domain.DTOs
{
    public class DeletionResultDto
    {
        public required bool IsDeleted { get; set; }
        public string Message { get; set; }  = string.Empty;
    }
}
