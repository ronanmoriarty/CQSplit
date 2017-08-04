using Castle.MicroKernel.Registration;

namespace Cafe.Waiter.Web
{
    public class TransientLifestyle : ILifestyle
    {
        public BasedOnDescriptor SetLifestyle(BasedOnDescriptor basedOnDescriptor)
        {
            return basedOnDescriptor.LifestyleTransient(); // bit of a hack! Doing this so we don't run into problems trying to configure controllers as PerWebRequest in a test context.
        }
    }
}