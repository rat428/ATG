using ATGHR.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ATGHR.Models.EntityFramework.DbContext
{
	public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Division> Division { get; set; }
		public DbSet<CommonFile> CommonFiles { get; set; }
		public DbSet<Course> Courses { get; set; }
	}
}
