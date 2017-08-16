using Castle.MicroKernel.Registration;

namespace Cafe.Waiter.Web.DependencyInjection
{
    public interface ILifestyle
    {
        BasedOnDescriptor SetLifestyle(BasedOnDescriptor basedOnDescriptor);
    }
}