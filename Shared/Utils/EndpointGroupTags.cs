namespace Shared.Utils
{
    public class EndpointGroupTags
    {
        public const string Users = nameof(Users);
        public const string Authentication = nameof(Authentication);
        public const string Interests = nameof(Interests);
        public const string Comments = nameof(Comments);
        public const string UsersInterests = Users + " " + Interests;
    }
}
