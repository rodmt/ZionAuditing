using ZionAuditing.Domain.Entities;

namespace ZionAuditing.Domain.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByEntityAsync(Guid entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
