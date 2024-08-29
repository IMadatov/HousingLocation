namespace ServiceStatusResult
{
    public class BadRequesServiceResult<T> : ServiceResultBase<T>
    {
        public BadRequesServiceResult() : base(400, null, default) { }

        public BadRequesServiceResult(T result) : base(400, null, result) { }

        public BadRequesServiceResult(string message, T result = default) : base(400, message, default) { }
    }
}
