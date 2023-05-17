namespace borntocode_backend.Dto;

public class SignInResultDto
{
    public SignInResultDto(string name, string token)
    {
        Name = name;
        Token = token;
    }

    public string Name { get; set; }

    public string Token { get; set; }
}