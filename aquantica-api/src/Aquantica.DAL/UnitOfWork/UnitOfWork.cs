using Aquantica.Core.Entities;
using Aquantica.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aquantica.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IGenericRepository<User>> _accountRepository;
    private readonly Lazy<IGenericRepository<Role>> _roleRepository;
    private readonly Lazy<IGenericRepository<AccessAction>> _accessActionRepository;
    private readonly Lazy<IGenericRepository<RefreshToken>> _refreshTokenRepository;
    private readonly Lazy<IGenericRepository<Settings>> _settingsRepository;
    private readonly Lazy<IGenericRepository<IrrigationEvent>> _irrigationHistoryRepository;
    private readonly Lazy<IGenericRepository<IrrigationSection>> _sectionRepository;
    private readonly Lazy<IGenericRepository<IrrigationSectionType>> _sectionTypeRepository;
    private readonly Lazy<IGenericRepository<IrrigationRuleset>> _rulesetRepository;

    public UnitOfWork(
        ApplicationDbContext context,
        Lazy<IGenericRepository<User>> accountRepository,
        Lazy<IGenericRepository<Role>> roleRepository,
        Lazy<IGenericRepository<AccessAction>> accessActionRepository,
        Lazy<IGenericRepository<RefreshToken>> refreshTokenRepository,
        Lazy<IGenericRepository<Settings>> settingsRepository,
        Lazy<IGenericRepository<IrrigationEvent>> irrigationHistoryRepository,
        Lazy<IGenericRepository<IrrigationSection>> sectionRepository,
        Lazy<IGenericRepository<IrrigationSectionType>> sectionTypeRepository,
        Lazy<IGenericRepository<IrrigationRuleset>> rulesetRepository
        )
    {
        _context = context;
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
        _accessActionRepository = accessActionRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _settingsRepository = settingsRepository;
        _irrigationHistoryRepository = irrigationHistoryRepository;
        _sectionRepository = sectionRepository;
        _sectionTypeRepository = sectionTypeRepository;
        _rulesetRepository = rulesetRepository;
    }

    public IGenericRepository<User> UserRepository => _accountRepository.Value;

    public IGenericRepository<Role> RoleRepository => _roleRepository.Value;

    public IGenericRepository<AccessAction> AccessActionRepository => _accessActionRepository.Value;

    public IGenericRepository<RefreshToken> RefreshTokenRepository => _refreshTokenRepository.Value;
    
    public IGenericRepository<Settings> SettingsRepository => _settingsRepository.Value;
    
    public IGenericRepository<IrrigationEvent> IrrigationHistoryRepository => _irrigationHistoryRepository.Value;
    
    public IGenericRepository<IrrigationSection> SectionRepository => _sectionRepository.Value;
    
    public IGenericRepository<IrrigationSectionType> SectionTypeRepository => _sectionTypeRepository.Value;
    
    public IGenericRepository<IrrigationRuleset> RulesetRepository => _rulesetRepository.Value;

    
    public Task<IDbContextTransaction> CreateTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        if (_context.Database.CurrentTransaction != null)
            return _context.Database.RollbackTransactionAsync();

        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();

        _context.Database.CurrentTransaction?.Dispose();
    }
}
