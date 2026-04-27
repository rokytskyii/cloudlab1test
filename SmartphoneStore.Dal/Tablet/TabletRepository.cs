using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Model.Tablet;

namespace SmartphoneStore.Dal.Tablet;

public class TabletRepository : ITabletRepository
{
    private readonly CosmosDbContext _context;
    private readonly IMapper _mapper;

    public TabletRepository(CosmosDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

        _context.Database.EnsureCreated();
    }

    public async Task<TabletDto> CreateAsync(TabletDto tablet)
    {
        var dao = _mapper.Map<TabletDao>(tablet);
        dao.Id = Guid.NewGuid();
        _context.Tablets.Add(dao);
        await _context.SaveChangesAsync();

        return _mapper.Map<TabletDto>(dao);
    }

    public async Task<TabletDto> GetByIdAsync(Guid id)
    {
        var dao = await _context.Tablets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<TabletDto>(dao);
    }

    public async Task<IEnumerable<TabletDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var daos = await _context.Tablets
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TabletDto>>(daos);
    }

    public async Task<TabletDto> UpdateAsync(TabletDto tablet)
    {
        var dao = _mapper.Map<TabletDao>(tablet);
        _context.Tablets.Update(dao);
        await _context.SaveChangesAsync();

        return _mapper.Map<TabletDto>(dao);
    }

    public async Task<TabletDto> DeleteAsync(Guid id)
    {
        var localEntity = _context.Tablets.Local.FirstOrDefault(x => x.Id == id);

        TabletDao dao;
        if (localEntity != null)
        {
            dao = localEntity;
        }
        else
        {
            dao = new TabletDao { Id = id };
            _context.Tablets.Attach(dao);
        }

        _context.Tablets.Remove(dao);
        await _context.SaveChangesAsync();

        return _mapper.Map<TabletDto>(dao);
    }
}