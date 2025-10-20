using BookShop.Data;

namespace BookShop.Common;

public static class CurrentUser
{
    public static Guid? CurrentUserId { get; set; } = null;
}