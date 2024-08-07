﻿using Aiba.Entities;

namespace Aiba.Repository
{
    public interface ILibraryRepository
    {
        public Task<LibraryEntity> AddLibraryEntityByUserIdAsync(string id, LibraryEntity library);
        public Task<IEnumerable<LibraryEntity>> GetLibraryEntitiesByUserIdAsync(string userId);
        public Task RemoveLibraryEntityByUserIdAsync(string id, LibraryEntity library);
    }
}