namespace Elders.Cronus
{
    public struct ReadResult<T>
    {
        public ReadResult(string error) : this(default(T), error) { }

        public ReadResult(T data, string error = "")
        {
            Data = data;
            Error = error;
        }

        public ReadResult(System.Exception exception) : this(exception.Message) { }

        public T Data { get; set; }

        public string Error { get; set; }

        public bool NotFound => (Data.Equals(default(T)));

        public bool IsSuccess => HasFailed == false;

        public bool HasFailed => NotFound || string.IsNullOrEmpty(Error) == false;
    }
}
