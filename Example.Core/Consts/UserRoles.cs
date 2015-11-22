namespace Example.Core.Consts
{
    public enum UserRoles
    {
        Anonymous = 0,
        User = 1,
        Moderator = 2,
        Administrator = 3
    }

    public static class UserRolesExtension
    {
        public static string ToString(this UserRoles role)
        {
            switch (role)
            {
                case UserRoles.User:
                    {
                        return "Пользователь";
                    }
                case UserRoles.Administrator:
                    {
                        return "Администратор";
                    }
                case UserRoles.Moderator:
                    {
                        return "Модератор";
                    }
                case UserRoles.Anonymous:
                default:
                    {
                        return "Анонимный пользователь";
                    }
            }
        }
    }
}
