using Microsoft.EntityFrameworkCore;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Interfaces
{
    public interface IOfficesContext
    {
        DbSet<Office> Offices { get; set; }
    }
}
