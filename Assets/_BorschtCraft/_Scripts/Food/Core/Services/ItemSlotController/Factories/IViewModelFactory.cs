namespace BorschtCraft.Food.UI.Factories
{
    public interface IViewModelFactory
    {
        IConsumedViewModel CreateViewModel(IConsumed itemLayer);
    }
}
