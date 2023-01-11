using Microsoft.Extensions.Options;
using ToDo.Database;
using ToDo.Database.Models;
using ToDo.Models;
using ToDo.Models.Exceptions;
using ToDo.Utils;

namespace ToDo.Services;

public interface IUserService
{
    string Authenticate(LoginModel model);
    void Register(RegisterModel model);
    IEnumerable<DbUser> GetAll();
    DbUser GetById(Guid id);
}

public class UserService : IUserService
{
    private DatabaseContext _context;
    private IJwtUtils _jwtUtils;

    public UserService(DatabaseContext context, IJwtUtils jwtUtils)
    {
        _context = context;
        _jwtUtils = jwtUtils;
    }

    public void Register(RegisterModel model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
        {
            throw new AppException($"Username '{model.Username}' is already taken.");
        }

        var user = new DbUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Email = model.Email,
            Username = model.Username
        };

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }


    public string Authenticate(LoginModel model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful so generate jwt token
        return _jwtUtils.GenerateJwtToken(user);
    }

    public IEnumerable<DbUser> GetAll()
    {
        return _context.Users;
    }

    public DbUser GetById(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
}