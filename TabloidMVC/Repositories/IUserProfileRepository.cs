using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        UserProfile GetUserById(int id);

        List<UserProfile> GetAllProfiles();
    }
}