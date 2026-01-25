namespace Shared.Utils;

public static class EndpointGroupTags
{
    public const string Users = nameof(Users);
    public const string Authentication = nameof(Authentication);
    public const string Interests = nameof(Interests);
    public const string Stories = nameof(Stories);
    public const string UsersInterests = Users + " " + Interests;
    public const string UsersFavourites = Users + " Favourites";
    public const string Tags = nameof(Tags);

}
