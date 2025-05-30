namespace HyperKala.Application.Interfaces
{
    public interface ISmsService 
    {
        Task SendVerificationCode(string mobile, string activeCode);
    }
}
