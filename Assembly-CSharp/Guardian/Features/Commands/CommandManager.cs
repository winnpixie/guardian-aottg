using Guardian.Features.Commands.Impl;
using Guardian.Features.Commands.Impl.MasterClient;
using Guardian.Features.Commands.Impl.Debug;
using Guardian.Features.Commands.Impl.RC;

namespace Guardian.Features.Commands
{
    class CommandManager : FeatureManager<Command>
    {
        public override void Load()
        {
            // Normal
            base.Add(new CommandHelp());
            base.Add(new CommandClear());
            base.Add(new CommandIgnore());
            base.Add(new CommandMute());
            base.Add(new CommandRejoin());
            base.Add(new CommandReloadConfig());
            base.Add(new CommandSay());
            base.Add(new CommandSetGuild());
            base.Add(new CommandSetLighting());
            base.Add(new CommandSetName());
            base.Add(new CommandTranslate());
            base.Add(new CommandUnignore());
            base.Add(new CommandUnmute());
            base.Add(new CommandWhois());

            // MasterClient
            base.Add(new CommandAso());
            base.Add(new CommandDifficulty());
            base.Add(new CommandKill());
            base.Add(new CommandPause());
            base.Add(new CommandRestart());
            base.Add(new CommandRevive());
            base.Add(new CommandRoom());
            base.Add(new CommandScatterTitans());
            base.Add(new CommandSetMap());
            base.Add(new CommandSetTitans());
            base.Add(new CommandUnpause());

            // Debug
            base.Add(new CommandDrawDistance());
            base.Add(new CommandLogProperties());

            // RC
            base.Add(new CommandBan());
            base.Add(new CommandBanlist());
            base.Add(new CommandIgnoreList());
            base.Add(new CommandKick());
            base.Add(new CommandPM());
            base.Add(new CommandResetKD());
            base.Add(new CommandRules());
            base.Add(new CommandSpectate());
            base.Add(new CommandTeam());
            base.Add(new CommandUnban());
        }

        public void HandleCommand(InRoomChat irc)
        {
            var message = irc.inputLine.Trim();
            var args = message.Substring(1).Split(' ');

            var command = base.Find(args[0]);
            if (command != null)
            {
                if (!command.MasterClient || PhotonNetwork.isMasterClient)
                {
                    command.Execute(irc, args.Length > 1 ? args.CopyOfRange(1, args.Length) : new string[0]);
                }
                else
                {
                    irc.AddLine("Command requires MasterClient!".WithColor("FF0000"));
                }
            }
        }
    }
}
