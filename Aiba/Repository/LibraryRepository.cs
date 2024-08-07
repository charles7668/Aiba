using Aiba.Entities;
using Aiba.Exceptions;

namespace Aiba.Repository
{
    public class LibraryRepository(AppDBContext context) : ILibraryRepository
    {
        public Task RemoveLibraryEntityByUserIdAsync(string id, LibraryEntity library)
        {
            LibraryEntity? entity = context.Libraries.FirstOrDefault(l => l.UserId == id && l.Name == library.Name);
            if (entity == null)
            {
                return Task.CompletedTask;
            }

            context.Libraries.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<LibraryEntity>> GetLibraryEntitiesByUserIdAsync(string userId)
        {
            return Task.FromResult<IEnumerable<LibraryEntity>>(context.Libraries.Where(l => l.UserId == userId));
        }

        public async Task<LibraryEntity> AddLibraryEntityByUserIdAsync(string id, LibraryEntity library)
        {
            if (context.Libraries.Any(x => x.Name == library.Name && x.UserId == id))
            {
                throw new ValueExistException("library name already exist");
            }

            if (context.Libraries.Any(x => x.Path == library.Path && x.UserId == id))
            {
                throw new ValueExistException("library path already exist");
            }

            library.UserId = id;
            await context.Libraries.AddAsync(library);
            return library;
        }
    }
}