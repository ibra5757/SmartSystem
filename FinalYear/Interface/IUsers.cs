using FinalYear.Models;

namespace FinalYear.Interface
{
    public interface IUsers
    {
        string Contact { get; set; }
        string ID { get; set; }
        string Password { get; set; }
        string Role { get;  set; }
        string UserName { get; set; }

        Users Login();
    }
}