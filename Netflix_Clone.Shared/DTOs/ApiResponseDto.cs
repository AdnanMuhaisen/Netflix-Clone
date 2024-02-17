namespace Netflix_Clone.Shared.DTOs
{
    public record ApiResponseDto<T>
    {
        public required T Result { get; set; } = default!;
        /// <summary>
        /// Returns true if the whole api is executed correctly , false otherwise
        /// </summary>
        public required bool IsSucceed { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
