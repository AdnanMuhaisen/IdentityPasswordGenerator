namespace IdentityPasswordGenerator;

internal class Password(CryptoRandom random)
{
    private readonly List<char> _content = [];

    public int Length => _content.Count;

    public int UniqueChars => _content.Distinct().Count();

    public void InsertRandom(string chars) => _content.Insert(random.Next(0, _content.Count), chars[random.Next(0, chars.Length)]);

    public override string ToString() => new(_content.ToArray());
}