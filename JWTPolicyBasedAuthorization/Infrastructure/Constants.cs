namespace JWTPolicyBasedAuthorization.Infrastructure
{
    public static class Constants
    {
        public static class Policy
        {
            public const string AdminPolicy = "AdminPolicy";
            public const string ManagerPolicy = "ManagerPolicy";
            public const string UserPolicy = "UserPolicy";
        }

        public static class Role
        {
            public const string SuperAdmin = "Super Admin";
            public const string SystemAdmin = "System Admin";
            public const string RegionalManager = "Regional Manager";
            public const string DepartmentManager = "Department Manager";
            public const string Reviewer = "Reviewer";
            public const string Owner = "Owner";
            public const string TeamMember = "Team Member";
        }

        public static class Permission
        {
            public const string CanCreate = "CanCreate";
            public const string CanDelete = "CanDelete";
            public const string CanApprove = "CanApprove";
            public const string CanView = "CanView";
        }

    }
}