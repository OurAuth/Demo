namespace OurAuth.Demo.Api.Shared;

/// <summary>
/// 
/// </summary>
public class UserInfo
{
    /// <summary>
    /// 
    /// </summary>
    public UserInfoIdentity Identity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<UserInfoClaim> Claims { get; set; }
}
