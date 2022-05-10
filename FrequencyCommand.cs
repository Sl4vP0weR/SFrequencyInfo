namespace SFrequencyInfo
{
    public class FrequencyCommand : IRocketCommand
    {
        public string Name { get; } = "frequency";

        public AllowedCaller AllowedCaller { get; } = AllowedCaller.Both;

        public string Help { get; } = "Shows radio frequency of player.";

        public string Syntax => $"/{Name} [target]";

        public List<string> Aliases { get; } = new() { "freq" };

        List<string> permissions;
        public List<string> Permissions => permissions ??= Aliases.Prepend(Name).ToList();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var arg = string.Join(" ", command);
            if(string.IsNullOrWhiteSpace(arg))
            {
                UnturnedChat.Say(caller, Help, Color.yellow);
                UnturnedChat.Say(caller, Syntax, Color.yellow);
                return;
            }    
            var target = PlayerTool.getPlayer(arg);
            if (!target)
            {
                if (ulong.TryParse(arg, out var steamId))
                    target = PlayerTool.getPlayer(new CSteamID(steamId));
                else
                {
                    UnturnedChat.Say(caller, TranslateNotFound(arg), Color.red);
                    return;
                }
            }
            UnturnedChat.Say(caller, TranslateFrequency(target.channel.owner.playerID.characterName, target.quests.radioFrequency));
        }
    }
}