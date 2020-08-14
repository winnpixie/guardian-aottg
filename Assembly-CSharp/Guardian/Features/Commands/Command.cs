namespace Guardian.Features.Commands
{
    public abstract class Command : Feature
    {
        public string usage;
        public bool masterClient;

        public Command(string name, string[] aliases, string usage, bool masterClient) : base(name, aliases)
        {
            this.usage = usage;
            this.masterClient = masterClient;
        }

        public abstract void Execute(InRoomChat irc, string[] args);
    }
}
