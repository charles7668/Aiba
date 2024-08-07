﻿using Aiba.Model;

namespace Aiba.Repository
{
    public interface IUnitOfWork
    {
        public Task AddLibraryInfoByUserIdAsync(string userId, LibraryInfo libraryInfo);
        public Task<IEnumerable<LibraryInfo>> GetLibraryInfosByUserIdAsync(string userId);
        public Task RemoveLibraryByUserIdAsync(string userId, LibraryInfo libraryInfo);
    }
}