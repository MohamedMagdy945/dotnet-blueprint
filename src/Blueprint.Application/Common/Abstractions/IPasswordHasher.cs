namespace Blueprint.Application.Common.Abstractions;

public interface IPasswordHasher
{
    public string Hash(string password);

    public bool Verify(string password, string hashedPassword);
}