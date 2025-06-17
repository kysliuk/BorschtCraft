using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorschtCraft.Food
{
    public abstract class ItemHandlerBase : IItemHandler
    {
        private IItemHandler _nextHandler;

        public bool Handle(IItem item)
        {
            if(CanHandle(item))
                return Process(item);

            return _nextHandler?.Handle(item) ?? false;
        }

        public void SetNext(IItemHandler nextHandler) => _nextHandler = nextHandler;

        protected abstract bool CanHandle(IItem item);
        protected abstract bool Process(IItem item);
    }
}
