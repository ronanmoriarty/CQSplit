using Castle.MicroKernel.Registration;

namespace Cafe.Waiter.Web
{
    public interface ILifestyle
    {
        BasedOnDescriptor SetLifestyle(BasedOnDescriptor basedOnDescriptor);
    }
}