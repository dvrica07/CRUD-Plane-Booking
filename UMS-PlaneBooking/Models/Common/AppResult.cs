namespace UMS_PlaneBooking.Models.Common;

public class AppResult<TTarget>
{
    protected bool succeeded;
    protected string message;
    protected TTarget? result;
    protected ErrorInfo error;

    public virtual bool Succeeded => succeeded;
    public virtual string Message => message;
    public virtual TTarget? Result => result;
    public virtual ErrorInfo Error => error;

    public AppResult()
    {
        succeeded = false;
        message = "The operation failed without explicit reason. Please contact technical support.";
        result = default(TTarget);
        error = new ErrorInfo
        {
            Code = "-1",
            Description = "The operation failed without explicit reason. Please contact technical support."
        };
    }

    public AppResult(Exception ex, string message)
    {
        if (ex == null)
        {
            ex = new Exception("The operation failed without explicit reason. Please contact technical support.");
        }

        succeeded = false;
        this.message = (message ?? "The operation failed without explicit reason. Please contact technical support.");
        result = default(TTarget);
        error = new ErrorInfo
        {
            Code = "-2",
            Description = ex.Message,
            Exception = ex
        };
    }

    public AppResult(TTarget result, string message)
    {
        succeeded = true;
        this.message = message;
        this.result = result;
    }

    public AppResult(Exception ex, string message, string errorCode, string errorDesc)
    {
        succeeded = false;
        this.message = message;
        result = default(TTarget);
        error = new ErrorInfo
        {
            Code = errorCode,
            Description = errorDesc,
            Exception = ex
        };
    }

    public static AppResult<TTarget> CreateSucceeded(TTarget result, string message)
    {
        return new AppResult<TTarget>(result, message);
    }

    public static AppResult<TTarget> CreateFailed()
    {
        return new AppResult<TTarget>();
    }

    public static AppResult<TTarget> CreateFailed(Exception ex, string message)
    {
        return new AppResult<TTarget>(ex, message);
    }

    public static AppResult<TTarget> CreateFailed(Exception ex, string message,
        string errorCode = "", string errorDesc = "")
    {
        return new AppResult<TTarget>(ex, message, errorCode, errorDesc);
    }
}