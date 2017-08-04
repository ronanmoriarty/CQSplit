using Castle.MicroKernel.Registration;

namespace Cafe.Waiter.Web.DependencyInjection
{
    public class PerWebRequestLifestyle : ILifestyle
    {
        public BasedOnDescriptor SetLifestyle(BasedOnDescriptor basedOnDescriptor)
        {
            return basedOnDescriptor.LifestylePerWebRequest();
        }
    }
}