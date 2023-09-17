using System;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
	public class ExerciseDbContext: DbContext
	{
        public ExerciseDbContext(DbContextOptions<ExerciseDbContext> option) : base(option)
        {
        }
		public DbSet<Exercise> Exercises { get; set; }

        public bool IsDatabaseConnected()
        {
            try
            {
                // Attempt to perform a simple database query
                var count = Exercises.Count();
                return true; // If the query succeeds, the connection is assumed to be fine.
            }
            catch (Exception)
            {
                return false; // If an exception occurs, consider the connection as failed.
            }
        }
    }
}

