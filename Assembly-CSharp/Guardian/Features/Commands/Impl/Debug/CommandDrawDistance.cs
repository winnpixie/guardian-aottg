namespace Guardian.Features.Commands.Impl.Debug
{
	class CommandDrawDistance : Command
	{
		public CommandDrawDistance() : base("drawdistance", new string[] { "renderdistance", "renderdist", "drawdist" }, "[distance]", false) { }

		public override void Execute(InRoomChat irc, string[] args)
		{
			var distance = 1500f; // Observed default

			if (args.Length > 0 && float.TryParse(args[0], out distance))
			{
				irc.AddLine($"Draw distance is now {distance}!");
			}
			else
			{
				irc.AddLine($"Draw distance reverted back to default ({distance})!");
			}

			UnityEngine.Camera.main.farClipPlane = distance;
		}
	}
}
