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

            public const string RefreshToken = Base + Account + "Refresh";

            public const string RevokeTokens = Base + Account + "Revoke";

            public const string ConfirmEmail = Base + Account + "ConfirmEmail";

            public const string InviteUser = Base + Account + "InviteUser";

        }

        public static class Garderie
        {
            public const string garderie = "Garderie/";

            public const string Get = Base + garderie + "Get";

            public const string Create = Base + garderie + "Create";

            public const string Update = Base + garderie + "Update";

            public const string Delete = Base + garderie + "Delete";
        }
    }
}
