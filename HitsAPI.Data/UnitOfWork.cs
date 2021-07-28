using HitsAPI.Core;
using HitsAPI.Core.Repositories;
using HitsAPI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HitsAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HitsAPIDbContext _context;
        private ArtistRepository _artistRepository;
        private MusicRepository _musicRepository;

        public UnitOfWork(HitsAPIDbContext context)
        {
            this._context = context;
        }

        public IMusicRepository Musics => _musicRepository = _musicRepository ?? new MusicRepository(_context);

        public IArtistRepository Artists => _artistRepository = _artistRepository ?? new ArtistRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
