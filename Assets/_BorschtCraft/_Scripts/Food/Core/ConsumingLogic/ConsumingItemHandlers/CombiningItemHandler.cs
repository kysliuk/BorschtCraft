using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningItemHandler : ConsumingItemHandlerBase, IInitializable
    {
        private CookableItemHandler _itemHandler;
        public void Initialize()
        {
            SetNext(_itemHandler);
        }

        protected override ISlotMatchingStrategy SetStrategy()
        {
            return new CombiningSlotStrategy();
        }


        public CombiningItemHandler(CookableItemHandler itemHandler)
        {

            this._itemHandler = itemHandler;

        }
    }
}
