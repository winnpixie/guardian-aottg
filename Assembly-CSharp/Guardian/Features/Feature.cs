namespace Guardian.Features
{
    public class Feature
    {
        public string Name;
        public string[] aliases;

        public Feature(string name, params string[] aliases)
        {
            this.Name = name;
            this.aliases = aliases;
        }
    }
}
