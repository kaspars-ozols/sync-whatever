using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Tests
{
    public class EmployeeUserMapper : IEntityMapper<Employee, User>
    {
        public User MapNew(Employee source)
        {
            var target = new User();
            return MapExisting(source, target);
        }

        public User MapExisting(Employee source, User target)
        {
            target.FullName = source.FullName;
            target.Username = source.Email;
            return target;
        }
    }
}