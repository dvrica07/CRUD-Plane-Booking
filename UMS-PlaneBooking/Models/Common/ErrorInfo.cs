namespace UMS_PlaneBooking.Models.Common;

public class ErrorInfo
{
    public ErrorInfo()
    {
    }

    public ErrorInfo(Exception ex)
    {
        this.Exception = ex;
    }

    public virtual string Description { get; set; }
    public virtual string Code { get; set; }
    public virtual Exception Exception { get; set; }
}