using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.QueryParameters;
using PhotoForum.Repositories.Contracts;

namespace PhotoForum.Repositories
{
    public class AdminsRepository : IAdminsRepository
    {
        private readonly PhotoForumContext context;

        public AdminsRepository(PhotoForumContext context)
        {
            this.context = context;
        }

        public Admin CreateAdmin(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();

            return admin;
        }
        public Admin GetAdminByUsername(string username)
        {
            var admin = GetAdmins().FirstOrDefault(u => u.Username == username);
            //if (admin == null) throw new EntityNotFoundException($"Admin with username {username} could not be found.");

            return admin;
        }

        private IQueryable<Admin> IQ_GetAdmins()
        {
            return context.Admins;
        }
        public IList<Admin> GetAdmins()
        {
            return IQ_GetAdmins().ToList();
        }
        private IQueryable<Log> IQ_GetLogs()
        {
            return context.Logs;
        }
        public IList<Log> GetLogs()
        {
            return IQ_GetLogs().ToList();
        }
        public IList<Log> GetLastAddedLogs()
        {
            return GetLogs().TakeLast(10).ToList();
        }

        public Log AddLog(string message)
        {
            Log logEntry = new Log
            {
                Event = message
            };
            context.Logs.Add(logEntry);
            context.SaveChanges();

            return logEntry;
        }

        public IList<Admin> FilterBy(AdminQueryParameters filterParameters)
        {
            IQueryable<Admin> result = IQ_GetAdmins();

            result = FilterByFirstName(result, filterParameters.FirstName);
            result = FilterByLastName(result, filterParameters.LastName);
            result = FilterByUsername(result, filterParameters.Username);
            result = FilterByEmail(result, filterParameters.Email);
            return result.ToList();
        }

        private static IQueryable<Admin> FilterByUsername(IQueryable<Admin> admins, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return admins.Where(admin => admin.Username.Contains(username));
            }
            else
            {
                return admins;
            }
        }
        private static IQueryable<Admin> FilterByEmail(IQueryable<Admin> admins, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return admins.Where(admin => admin.Email.Contains(email));
            }
            else
            {
                return admins;
            }
        }
        private static IQueryable<Admin> FilterByFirstName(IQueryable<Admin> admins, string firstName)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                return admins.Where(admin => admin.FirstName.Contains(firstName));
            }
            else
            {
                return admins;
            }
        }
        private static IQueryable<Admin> FilterByLastName(IQueryable<Admin> admins, string lastName)
        {
            if (!string.IsNullOrEmpty(lastName))
            {
                return admins.Where(admin => admin.LastName.Contains(lastName));
            }
            else
            {
                return admins;
            }
        }

        private static IQueryable<Admin> FilterByPhoneNumber(IQueryable<Admin> admins, string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                return admins.Where(admin => admin.PhoneNumber.Contains(phoneNumber));
            }
            else
            {
                return admins;
            }
        }
    }
}
