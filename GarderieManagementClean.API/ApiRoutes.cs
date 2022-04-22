namespace GarderieManagementClean.API
{
    public static class ApiRoutes
    {

        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version + "/";


        public static class Identity
        {
            public const string Account = "Account/";

            public const string Login = Base + Account + "Login";

            public const string Register = Base + Account + "Register";

            public const string RefreshToken = Base + Account + "RefreshToken";

            public const string RevokeTokens = Base + Account + "RevokeTokens";

            public const string ConfirmEmail = Base + Account + "ConfirmEmail";
            
            public const string ConfirmEmailTest = Base + Account + "ConfirmEmailTEST/{userId}";


            public const string InviteUser = Base + Account + "InviteUser";

            public const string CompleteRegister = Base + Account + "CompleteRegister";


        }

        public static class Garderie
        {
            public const string garderie = "Garderie/";

            public const string Get = Base + garderie + "Get";

            public const string Create = Base + garderie + "Create";

            public const string Update = Base + garderie + "Update";

            public const string Delete = Base + garderie + "Delete";
        }

        public static class Group
        {
            public const string group = "Group/";

            public const string Get = Base + group + "Get/{groupId:int}";

            public const string GetAll = Base + group + "Get/";

            public const string Create = Base + group + "Create";

            public const string Update = Base + group + "Update";

            public const string Delete = Base + group + "Delete/{groupId:int}";
        }
    }
}
