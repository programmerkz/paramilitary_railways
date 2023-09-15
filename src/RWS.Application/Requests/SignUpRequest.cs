namespace RWS.Application.Requests
{
    public class SignUpRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }

        // для создания и привязки бъекта Employee к объекту User
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
    }
}
