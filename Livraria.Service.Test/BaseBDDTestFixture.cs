using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Livraria.Service.Test
{
    [Specification]
    public abstract class BaseBDDTestFixture
    {
        protected Exception CaugthException;

        public abstract void Initialize();
        public abstract void Given();
        public abstract void When();

        [TestInitialize]
        public void Setup()
        {
            try { Initialize(); } catch { throw; }
            try { Given(); } catch { throw; }
            try { When(); } catch (Exception ex) { CaugthException = ex; }
        }

        protected bool HasException(string exceptionMessage, Exception exception)
        {
            if (exception != null && exception.Message == exceptionMessage)
                return true;

            return HasException(exceptionMessage, exception.InnerException);
        }
    }

    public class ThenAtrribute : TestMethodAttribute { }
    public class SpecificationAttribute : TestClassAttribute { }
}
