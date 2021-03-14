using System.Collections.Generic;

namespace FormsAuthTest.Services
{
    public interface IUsersService
    {
        public bool UserExists(string username);
        public bool UserHasPassword(string username, string password);
    }

    public class UsersServise : IUsersService
    {
        private static Dictionary<string, string> _users = new Dictionary<string, string>()
        {
            {"tacos","tac"},
            {"nachos", "nac"}
        };

        public bool UserHasPassword(string username, string password)
        {
            return _users.TryGetValue(username, out var validPassword) && validPassword == password;
        }

        public bool UserExists(string username)
        {
            return _users.ContainsKey(username);
        }
    }
}
