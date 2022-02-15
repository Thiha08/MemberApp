using MemberApp.Data.Abstract;
using MemberApp.Data.Infrastructure.Core.Extensions;
using MemberApp.Data.Infrastructure.Services.Abstract;
using MemberApp.Model.Entities;
using MemberApp.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberApp.Data.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<NotificationRecipient> _notificationRecipientRepository;

        public NotificationService(
            IConfiguration configuration,
            IRepository<Notification> notificationRepository,
            IRepository<NotificationRecipient> notificationRecipientRepository)
        {
            _configuration = configuration;
            _notificationRepository = notificationRepository;
            _notificationRecipientRepository = notificationRecipientRepository;
        }

        public async Task<long> CreateNotificationAsync(NotificationCreateViewModel model)
        {
            var recipients = new List<NotificationRecipient>();

            foreach (var recipient in model.Recipients)
            {
                recipients.Add(new NotificationRecipient
                {
                    RecipientUserName = recipient
                });
            }

            var newNotification = new Notification
            {
                Content = model.Content,
                TargetUrl = !string.IsNullOrWhiteSpace(model.TargetUrl) ? model.TargetUrl : _configuration["AppSettings:HostUrl"].ToString(),
                Recipients = recipients,
                Category = model.Category,
            };

            await _notificationRepository.AddAsync(newNotification);
            await _notificationRepository.CommitAsync();

            return newNotification.Id;
        }

        public async Task MarkNotificationsAsReadAsync(long notifiactionId, string currentUser)
        {
            var unreadNotification = await _notificationRepository.AllIncluding(x => x.Recipients)
                    .FirstOrDefaultAsync(x => x.Id == notifiactionId);

            if (unreadNotification != null)
            {
                var correspondingRecipient = unreadNotification.Recipients.FirstOrDefault(r => r.RecipientUserName == currentUser);
                if (correspondingRecipient != null && !correspondingRecipient.IsReceived)
                {
                    correspondingRecipient.IsReceived = true;
                    correspondingRecipient.ReceivedAt = DateTime.UtcNow;
                }

                await _notificationRepository.UpdateAsync(unreadNotification);
                await _notificationRepository.CommitAsync();
            }
        }

        public async Task<IEnumerable<NotificationListViewModel>> GetMostRecentNotificationsAsync(string currentUser, int numberOfRecords = 10)
        {
            var notificationRecords = await _notificationRecipientRepository.AllIncluding(x => x.Notification)
                .Where(x => x.RecipientUserName == currentUser)
                .OrderByDescending(x => x.CreatedDate)
                .Take(numberOfRecords)
                .ToListAsync();

            var notifications = new List<NotificationListViewModel>();

            foreach (var receipientRecord in notificationRecords)
            {
                if (receipientRecord.Notification != null)
                {
                    receipientRecord.IsSent = true;
                    receipientRecord.SentAt = DateTime.UtcNow;

                    notifications.Add(new NotificationListViewModel
                    {
                        Id = receipientRecord.NotificationId,
                        Content = receipientRecord.Notification.Content,
                        TargetUrl = receipientRecord.Notification.TargetUrl,
                        Category = receipientRecord.Notification.Category,
                        CreatedDate = receipientRecord.Notification.CreatedDate.ToTimeZoneTimeString("dd MMM HH:mm"),
                        IsReceived = receipientRecord.IsReceived,
                    });
                }
            }

            return notifications.OrderByDescending(x => x.CreatedDate);
        }
    }
}
