using Microsoft.EntityFrameworkCore;

namespace servis.Models
{
    public class PsychologistDBContext : DbContext
    {
        public PsychologistDBContext(DbContextOptions<PsychologistDBContext> options) :base(options)
        {
        }
        public DbSet<Psychologist> Psychologist { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<Methods> Methods { get; set; }
        public DbSet<GetSession> GetSession{ get; set; }
        public DbSet<GetSession> Session { get; set; }

    }
}
