using MemberApp.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberApp.Data.Infrastructure.Services.Abstract
{
    public interface INotificationService
    {
        Task<long> CreateNotificationAsync(NotificationCreateViewModel model);

        Task MarkNotificationsAsReadAsync(long notifiactionId, string currentUser);

        Task<IEnumerable<NotificationListViewModel>> GetMostRecentNotificationsAsync(string currentUser, int numberOfRecords = 10);
    }
}
