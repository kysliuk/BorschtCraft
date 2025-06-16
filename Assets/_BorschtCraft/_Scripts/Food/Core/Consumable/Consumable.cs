using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumable<T> : Item, IConsumable where T : IConsumed
    {
        public virtual IConsumed Consume(IConsumed item = null)
        {
            TryConsume(item, out var consumed);
            return consumed;
        }

        public virtual bool TryConsume(IConsumed item) => TryConsume(item, out _);

        public virtual bool TryConsume(IConsumed item, out IConsumed consumed)
        {
            var succeed = CanDecorate(item);
            if (!succeed)
            {
                Logger.LogWarning(this, $"Cannot decorate {item?.GetType().Name} by {GetType().Name}");
                consumed = item;
                return succeed;
            }

            consumed = ConsumeAbstractFactory.CreateConsumed<T>(Price, item);
            return succeed;
        }

        public virtual bool CanDecorate(IConsumed item)
        {
            if(item == null) return false;

            if (item.HasIngredientOfType<T>())
            {
                Logger.LogInfo(this, $"Item {item.GetType().Name} already contains ingredient {typeof(T).Name}");
                return false;
            }

            return InnerCanDecorate(item);
        }

        protected virtual bool InnerCanDecorate(IConsumed item)
        {
            return true;
        }

        public Consumable(int price) : base(price) { }
    }
}
