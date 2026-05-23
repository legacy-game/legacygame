using System.Threading;
using System.Threading.Tasks;
using Legacy.World;

namespace Legacy.Save
{
    public interface ISaveManager
    {
        Task<SaveResult> SaveAsync(string slot, WorldState state, CancellationToken ct = default);
        Task<WorldState> LoadAsync(string slot, CancellationToken ct = default);
    }
}
