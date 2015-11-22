namespace Example.Core.Consts
{
    public enum TopicStatus
    {
        Draft = 0,
        NotApproved = 1,
        Approved = 2,
        Deleted = 3,
        Archive = 4
    }

    public static class TopicStatusExtension
    {
        public static string ToString(this TopicStatus status)
        {
            switch (status)
            {
                case TopicStatus.NotApproved:
                {
                    return "Не проверен";
                }
                case TopicStatus.Approved:
                {
                    return "Проверен";
                }
                case TopicStatus.Deleted:
                {
                    return "Удален";
                }
                case TopicStatus.Archive:
                {
                    return "В архиве";
                }
                case TopicStatus.Draft:
                default:
                {
                    return "Черновик";
                }
            }
        }
    }
}
