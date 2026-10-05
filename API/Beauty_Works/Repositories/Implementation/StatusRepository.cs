using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApplicationDbContext dbContext;

        public StatusRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Status> CreateAsync(Status status)
        {
            await dbContext.Status.AddAsync(status);
            await dbContext.SaveChangesAsync();

            return status;
        }

        public async Task<Status?> DeleteAsync(int statusID)
        {
            var existingStatus = await dbContext.Status.FirstOrDefaultAsync(s => s.ID == statusID);

            if (existingStatus != null)
            {
                dbContext.Status.Remove(existingStatus);
                await dbContext.SaveChangesAsync();
                return existingStatus;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await dbContext.Status.ToListAsync();
        }

        public async Task<Status?> GetByID(int statusID)
        {
            return await dbContext.Status.FirstOrDefaultAsync(s => s.ID == statusID);
        }

        public async Task<Status?> UpdateAsync(Status status)
        {
            var existingStatus = await dbContext.Status.FirstOrDefaultAsync(s => s.ID == status.ID);

            if (existingStatus != null)
            {
                dbContext.Entry(existingStatus).CurrentValues.SetValues(status);
                await dbContext.SaveChangesAsync();
                return existingStatus;
            }
            else
            {
                return null;
            }
        }
    }
}
