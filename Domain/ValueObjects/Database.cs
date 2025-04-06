namespace Domain.ValueObjects;

public record Database(
    string Server,
    string DatabaseName,
    string UserId,
    string Password);