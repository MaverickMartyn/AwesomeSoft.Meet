namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// The response object containing all relevant User info and the JWT Token.
    /// </summary>
    public class AuthenticateResponse
    {
        public uint Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Name;
            Token = token;
        }
    }
}
