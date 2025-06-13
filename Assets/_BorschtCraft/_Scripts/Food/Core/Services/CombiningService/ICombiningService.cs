using Zenject;

namespace BorschtCraft.Food
{
    public interface ICombiningService
    {
        bool AttemptCombination(IConsumable consumable);
    }
}