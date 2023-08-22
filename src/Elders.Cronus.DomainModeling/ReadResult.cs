namespace Elders.Cronus;

public struct ReadResult<T>
{
    public ReadResult(T data) : this(data, string.Empty) { }

    ReadResult(T data, string error)
    {
        Data = data;
        Error = error;
        NotFoundHint = string.Empty;
    }

    public T Data { get; set; }

    public string Error { get; set; }

    public string NotFoundHint { get; set; }

    /// <summary>
    /// Indicates whether a data is found. In this case the query was successful but no data was found
    /// See <see cref="NotFoundHint"/> for details
    /// </summary>
    public bool NotFound => (Data is null);

    /// <summary>
    /// https://www.youtube.com/watch?v=7heswgZgJJs&feature=youtu.be&t=221
    /// </summary>
    public bool IsSuccess => NotFound == false && HasError == false;

    /// <summary>
    /// Something went wrong. See <see cref="Error"/> for more details.
    /// </summary>
    public bool HasError => string.IsNullOrEmpty(Error) == false;

    public override string ToString()
    {
        return $"{nameof(ReadResult<T>)}<{typeof(T).Name}> => IsSuccess: {IsSuccess} | HasData: {NotFound == false} {NotFoundHint} | HasError: {HasError} {Error}";
    }

    public static ReadResult<T> WithNotFoundHint(string hint) => new ReadResult<T>(default(T)) { NotFoundHint = hint };
    public static ReadResult<T> WithError(string error) => new ReadResult<T>(default(T), error);
    public static ReadResult<T> WithError(System.Exception exception) => new ReadResult<T>(default(T), exception.Message);

}
