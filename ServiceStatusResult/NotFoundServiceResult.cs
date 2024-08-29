namespace ServiceStatusResult
{
    public class NotFoundServiceResult<T> : ServiceResultBase<T>
    {
        public NotFoundServiceResult() : base(404, null, default) { }

        public NotFoundServiceResult(T result) : base(404, null, result) { }

        public NotFoundServiceResult(string message, T result = default) : base(404, message, result) { }
    }
}
