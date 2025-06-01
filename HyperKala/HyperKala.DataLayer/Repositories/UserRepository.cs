using HyperKala.DataLayer.Context;
using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Admin;
using HyperKala.Domain.ViewModels.Paging;
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

        #region AdminPanel
        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            var query = _context.Users.AsQueryable();


            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber == filter.PhoneNumber);
            }

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();


            return filter.SetPaging(pager).SetUsers(allData);
        }

        public async Task<EditUserFromAdmin?> GetEditUserFromAdmin(long userId)
        {
            return await _context.Users.AsQueryable()
                .Where(c => c.Id == userId)
                .Select(x => new EditUserFromAdmin
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserGender = x.UserGender
                }).SingleOrDefaultAsync();
        }

        #endregion

    }
}
