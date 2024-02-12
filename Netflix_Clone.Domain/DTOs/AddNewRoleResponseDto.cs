namespace Netflix_Clone.Domain.DTOs
{
    public record AddNewRoleResponseDto
    {
        public bool IsAdded { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
