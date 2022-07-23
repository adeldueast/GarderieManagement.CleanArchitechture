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


            public const string InviteStaff = Base + Account + "InviteStaff";

            public const string InviteTutor = Base + Account + "InviteTutor";


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

            public const string GetAll = Base + group + "Get";

            public const string Create = Base + group + "Create";

            public const string Update = Base + group + "Update";

            public const string Delete = Base + group + "Delete/{groupId:int}";

        }

        public static class Enfant
        {
            public const string enfant = "Enfant/";

            public const string Get = Base + enfant + "Get/{enfantId:int}";

            public const string GetAll = Base + enfant + "GetAll";

            public const string GetAllTutorsChilds = Base + enfant + "GetAllTutorsChilds";


            public const string GetAllGrouped = Base + enfant + "GetAllGrouped";


            public const string Create = Base + enfant + "Create";

            public const string Update = Base + enfant + "Update";

            public const string Delete = Base + enfant + "Delete/{enfantId:int}";

            public const string AssignTutorToEnfant = Base + enfant + "AssignTutorToEnfant";

        }

        public static class User
        {
            public const string user = "User/";

            public const string GetAllEmployees = Base + user + "employees";

            public const string GetAllEmployeesNoGroup = Base + user + "employeesNoGroup";


            public const string GetAllTutors = Base + user + "tutors";

            public const string GetAllChildsTutors = Base + user + "ChildsTutors";



            public const string GetAll = Base + user + "Get/";

            public const string Create = Base + user + "Create";

            public const string Update = Base + user + "Update";

            public const string Delete = Base + user + "Delete/{groupId:int}";
        }

        public static class Journal
        {
            public const string journal = "Journal/";

            public const string Get = Base + journal + "Get/{enfantId:int}";

            public const string Create = Base + journal + "Create/{enfantId:int}";

            public const string CreateGrouped = Base + journal + "Create";




            public const string Update = Base + journal + "Update/{enfantId:int}";

            public const string Delete = Base + journal + "Delete/{journalId:int}";
        }

        public static class Notification
        {
            public const string notification = "Notification/";

            public const string Get = Base + notification + "Get";

            public const string Create = Base + notification + "Create";

            public const string Update = Base + notification + "Update";

            public const string Delete = Base + notification + "Delete";
        }

    }
}
