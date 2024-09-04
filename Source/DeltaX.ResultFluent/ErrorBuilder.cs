namespace DeltaX.ResultFluent;


public class ErrorBuilder
{
    private List<Error> errors = new();

    public static ErrorBuilder Create() => new();

    public ErrorBuilder Add(string code, string message, string? detail = null)
    {
        errors.Add(new Error(code, message, detail));
        return this;
    }

    public ErrorBuilder Add(params Error[] errors)
    {
        this.errors.AddRange(errors);
        return this;
    }

    public ErrorBuilder Add(Exception e)
    {
        var code = e.GetType().Name;
        return Add(code, e.Message, e.StackTrace);
    }

    public bool HasError => errors.Count > 0;

    public Error[] GetErrors() => errors.ToArray();
}
