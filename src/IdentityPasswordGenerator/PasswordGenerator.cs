using Microsoft.AspNetCore.Identity;

namespace IdentityPasswordGenerator;

public class PasswordGenerator : IPasswordGenerator
{
    /// <summary>
    /// All non-alphanumeric characters.
    /// </summary>
    public const string NonAlphanumerics = "!@#$%^&*";

    /// <summary>
    /// All lower case ASCII characters.
    /// </summary>
    public const string Lowercases = "abcdefghijkmnopqrstuvwxyz";

    /// <summary>
    /// All upper case ASCII characters.
    /// </summary>
    public const string Uppercases = "ABCDEFGHJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// All digits.
    /// </summary>
    public const string Digits = "0123456789";

    private readonly CryptoRandom _random = new();
    private readonly OptionsValidator _optionsValidator = new(Requirements);
    private static readonly Requirement[] Requirements =
    [
        new(Digits, nameof(PasswordOptions.RequireDigit)),
        new(Lowercases, nameof(PasswordOptions.RequireLowercase)),
        new(Uppercases, nameof(PasswordOptions.RequireUppercase)),
        new(NonAlphanumerics, nameof(PasswordOptions.RequireNonAlphanumeric))
    ];

    public string GeneratePassword(PasswordOptions options, bool excludeNonRequiredChars = false)
    {
        _optionsValidator.ThrowIfInvalid(options, excludeNonRequiredChars);

        var password = new Password(_random);

        SatisfyCharacterTypeRequirements(password, options);
        SatisfyLengthRequirements(password, options, excludeNonRequiredChars);

        return password.ToString();
    }

    private static void SatisfyCharacterTypeRequirements(Password password, PasswordOptions options)
    {
        foreach (var requirement in Requirements)
        {
            if (requirement.Required(options))
            {
                password.InsertRandom(requirement.Chars);
            }
        }
    }

    private void SatisfyLengthRequirements(Password password, PasswordOptions options, bool excludeNonRequiredChars)
    {
        for (var i = password.Length; i < options.RequiredLength || password.UniqueChars < options.RequiredUniqueChars; i++)
        {
            var requirement = GetRandomRequirement();
            if (excludeNonRequiredChars)
            {
                while (!requirement.Required(options))
                {
                    requirement = GetRandomRequirement();
                }
            }

            password.InsertRandom(requirement.Chars);
        }

        return;

        Requirement GetRandomRequirement() => Requirements[_random.Next(0, Requirements.Length)];
    }
}