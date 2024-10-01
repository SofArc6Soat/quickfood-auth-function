namespace ServerlessAPI.Models.Response
{
    public record TokenUsuarioResponse
    {
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string? AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset? Expiry { get; set; }
        public List<string>? Errors { get; set; } = [];

        public void AddError(string error) =>
            Errors?.Add(error);
    }
}
