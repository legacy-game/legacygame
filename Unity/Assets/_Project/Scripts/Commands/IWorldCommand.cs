namespace Legacy.Commands
{
    public interface IWorldCommand
    {
        WorldCommandResult Execute(WorldCommandContext context);
    }
}
