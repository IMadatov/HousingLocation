namespace ServiceStatusResult
{
    public class NoContentServiceResult<T> : ServiceResultBase<T>
    {
        public NoContentServiceResult() : base(204, null, default) { }

        public NoContentServiceResult(T result) : base(204, null, result) { }

        public NoContentServiceResult(string message, T result) : base(204, message, result) { }
    }
}
