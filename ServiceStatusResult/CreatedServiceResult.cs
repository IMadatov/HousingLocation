namespace ServiceStatusResult
{
    public sealed class CreatedServiceResult<T> : ServiceResultBase<T>
    {
        public CreatedServiceResult() : base(201, null, default) { }

        public CreatedServiceResult(T result) : base(201, null, result) { }

        public CreatedServiceResult(string message, T result = default) : base(201, message, result) { }
    }
}
