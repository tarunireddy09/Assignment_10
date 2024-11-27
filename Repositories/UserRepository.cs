using CarRentalSystem.Models;
using System.Linq;
using System.Threading.Tasks;

public class UserRepository
{
    private readonly CarRentalContext _context;

    public UserRepository(CarRentalContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await Task.FromResult(_context.Users.SingleOrDefault(u => u.Email == email));
    }

    public async Task<User> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}
