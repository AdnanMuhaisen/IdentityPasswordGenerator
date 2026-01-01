using Microsoft.AspNetCore.Identity;

namespace IdentityPasswordGenerator;

internal class Requirement
{
    public Requirement(string chars, string propName)
    {
        Chars = chars;
        Required = options => (bool)typeof(PasswordOptions).GetProperty(propName)!.GetValue(options)!;
    }

    public string Chars { get; private set; }

    public Func<PasswordOptions, bool> Required { get; private set; }
}