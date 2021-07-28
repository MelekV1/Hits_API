using HitsAPI.Core.Models;
using HitsAPI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HitsAPI.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(HitsAPIDbContext context)
            : base(context)
        {

        }
        public async Task<IEnumerable<Artist>> GetAllWithMusicsAsync()
        {
            return await HitsAPIDbContext.Artists
                .Include(m => m.Musics)
                .ToListAsync();
        }

        public async Task<Artist> GetWithMusicsByIdAsync(int id)
        {
            return await HitsAPIDbContext.Artists
                .Include(a => a.Musics)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
        
        private HitsAPIDbContext HitsAPIDbContext
        {
            get { return Context as HitsAPIDbContext; }
        }
    }
}
