using SensorProcessingDemo.Common;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IEntityRepository<User> _userContext;

        public UserService(ILogger<UserService> logger, IEntityRepository<User> userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<IEnumerable<User>> GetAll(int userId)
        {
            _logger.LogInformation("Trying to get all users from db.");
            
            // filter out current user,cause its admin            
            var predicate = PredicateBuilder.True<User>().And(s => s.Id != userId);
            var users = await _userContext.FindAsync(predicate);          

            return users;
        }

        public async Task<User> GetUser(int userId)
        {
            _logger.LogInformation("Trying to get current user from db.");
            var user = await _userContext.GetByIdAsync(userId);

            return user;
        }

        public async Task ChangeUserRole(int userId)
        {
            _logger.LogInformation("Trying to get current user from db to change the role.");

            var user = await _userContext.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found.");

            }
            if (user.Role == "Admin")
            {
                user.Role = "User";
                await _userContext.UpdateAsync(user);
                
                _logger.LogInformation("The role was changed Admin -> User.");
                return;
            }

            if (user.Role == "User")
            {
                user.Role = "Admin";
                await _userContext.UpdateAsync(user);
                _logger.LogInformation("The role was changed User -> Admin.");
                return;
            }

            await _userContext.UpdateAsync(user);
        }        
    }
}
