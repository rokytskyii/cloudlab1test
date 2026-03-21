using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Model.Smartphone;

namespace SmartphoneStore.Dal.Smartphone;

public class SmartphoneRepository : ISmartphoneRepository
{
    private readonly IMapper _mapper;
    private readonly SqlDbContext _context;

    public SmartphoneRepository(IMapper mapper, SqlDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<SmartphoneDto> CreateAsync(SmartphoneDto smartphone)
    {
        var entity = _mapper.Map<SmartphoneDao>(smartphone);
        var entry = _context.Smartphones.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<SmartphoneDto>(entry.Entity);
    }

    public async Task<SmartphoneDto> GetByIdAsync(int id)
    {
        var entity = await _context.Smartphones
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        return _mapper.Map<SmartphoneDto>(entity);
    }

    public async Task<IEnumerable<SmartphoneDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var entities = await _context.Smartphones
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SmartphoneDto>>(entities);
    }

    public async Task<SmartphoneDto> UpdateAsync(SmartphoneDto smartphone)
    {
        var entity = _mapper.Map<SmartphoneDao>(smartphone);
        _context.Smartphones.Update(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<SmartphoneDto>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Smartphones.SingleOrDefaultAsync(c => c.Id == id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            _context.Smartphones.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}