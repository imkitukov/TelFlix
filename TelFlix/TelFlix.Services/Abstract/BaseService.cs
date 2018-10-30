using TelFlix.Data.Context;

namespace TelFlix.Services.Abstract
{
    public abstract class BaseService
    {
        private readonly TFContext context;

        public BaseService(TFContext context)
        {
            this.context = context;
        }

        public TFContext Context => this.context;
    }
}
