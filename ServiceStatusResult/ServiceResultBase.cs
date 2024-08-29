namespace ServiceStatusResult
{
    public class ServiceResultBase<T> : IEquatable<T>
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public T Result { get; set; }



        #region Protected-Constructor

        protected ServiceResultBase(T result)
        {
            Result = result;
        }

        protected ServiceResultBase(string message, T result) : this(result)
        {
            StatusMessage = message;
        }

        protected ServiceResultBase(int statusCode, string message, T result) : this(message, result)
        {
            StatusCode = statusCode;
        }

        #endregion

        #region Implict-Cast-Operator

        public static implicit operator ServiceResultBase<T>(T result)
        {
            return new ServiceResultBase<T>(result == null ? 204 : 200, null, result);
        }
        public static implicit operator T(ServiceResultBase<T> resultBase)
        {
            return resultBase.Result;
        }

        #endregion

        #region Conversion-Methods

        public static ServiceResultBase<T> FromServiceResult<T2>(ServiceResultBase<T2> result)
        {
            return new ServiceResultBase<T>(result.StatusCode, result.StatusMessage, default);
        }

        public ServiceResultBase<T2> ToType<T2>()
        {
            return new ServiceResultBase<T2>(StatusCode, StatusMessage, default);
        }

        #endregion

        #region Overriden-Object-class-Methods

        protected bool Equals(ServiceResultBase<T> other)
        {
            return StatusCode == other.StatusCode
                && StatusMessage == other.StatusMessage
                && EqualityComparer<T>.Default.Equals(Result, other.Result);
        }
        public bool Equals(T other)
        {
            return other is not null && ReferenceEquals(Result, other);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType()
                && Equals((ServiceResultBase<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StatusCode, StatusMessage, Result);
        }

        #endregion


    }
}
