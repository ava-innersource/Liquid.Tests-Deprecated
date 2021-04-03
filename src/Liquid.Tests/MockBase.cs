namespace Liquid.Tests
{
    /// <summary>
    /// The base class for mock classes.
    /// </summary>
    /// <typeparam name="TMock">The type of the mock.</typeparam>
    public abstract class MockBase<TMock>
    {
        /// <summary>
        /// Gets the mock class or interface.
        /// </summary>
        /// <returns>the mock.</returns>
        public abstract TMock GetMock();
    }
}