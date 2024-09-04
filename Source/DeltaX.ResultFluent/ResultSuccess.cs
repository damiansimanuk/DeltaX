namespace DeltaX.ResultFluent;

public class ResultSuccess : Result<ResultSuccess>
{
    public ResultSuccess()
    {
        IsSuccess = true;
        Value = this;
    }
}
