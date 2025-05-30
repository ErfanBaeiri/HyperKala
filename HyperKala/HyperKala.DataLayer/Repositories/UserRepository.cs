using HyperKala.DataLayer.Context;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HyperKala.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region DI
        private readonly HyperKalaDbContext _context;
        public UserRepository(HyperKalaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region General Method
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Account
        public async Task<bool> IsExistUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(s => s.PhoneNumber == phoneNumber);
        }

        public async Task AddNewUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.SingleOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        #endregion

        #region UserPanel
        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Id == id);
        }
        #endregion

    }
}
