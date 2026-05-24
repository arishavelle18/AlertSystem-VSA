namespace AlertSystem.API.Domain;

public class AlertItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateOnly ExpiryDate { get; private set; }
    public bool IsNotified { get; private set; } = false;
    public int NotificationLeadDate { get; private set; }

    private AlertItem()
    {
    }

    public static AlertItem Create(string title,string description, DateOnly expiredDate, int notificationLeadDate)
    {
        return new AlertItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            ExpiryDate = expiredDate,
            NotificationLeadDate = notificationLeadDate,
        };
    }

    public void Update(string title, string description, DateOnly expiredDate, bool isNotified, int notificationLeadDate)
    {
        Title = title;
        Description = description;
        ExpiryDate = expiredDate;
        IsNotified = isNotified;
        NotificationLeadDate = notificationLeadDate;
    }
}
