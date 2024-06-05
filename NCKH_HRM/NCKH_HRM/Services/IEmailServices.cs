using NCKH_HRM.ViewModels;

namespace NCKH_HRM.Services
{
    public interface IEmailServices
    {
        void SendEmail(Message message);
    }
}
