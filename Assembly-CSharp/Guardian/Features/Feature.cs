namespace Guardian.Features
{
    public class Feature
    {
        public readonly string Name;
        public readonly string[] Aliases;

        public Feature(string name, params string[] aliases)
        {
            this.Name = name;
            this.Aliases = aliases;
        }
    }
}