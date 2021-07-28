using HitsAPI.Core.Models;
using HitsAPI.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitsAPI.Data.Repositories
{
    public class MusicRepository : Repository<Music>,IMusicRepository
    {
        public MusicRepository(HitsAPIDbContext context)
            :base(context)
        {

        }

        public async Task<IEnumerable<Music>> GetAllWithArtistAsync()
        {
            return await HitsAPIDbContext.Musics
                .Include(m => m.Artist)
                .ToListAsync();
        }

        public async Task<Music> GetWithArtistByIdAsync(int id)
        {
            return await HitsAPIDbContext.Musics
                .Include(m => m.Artist)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistByArtistIdAsync(int artistId)
        {
            return await HitsAPIDbContext.Musics
                .Include(m => m.Artist)
                .Where(m => m.ArtistId == artistId)
                .ToListAsync();
        }

        private HitsAPIDbContext HitsAPIDbContext
        {
            get { return Context as HitsAPIDbContext; }
        }
    }
}
