using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Core;
using API.Services;
using Domain.Diaries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Api.CRUD
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string DiaryName { get; set; }
            public int Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;
            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;

            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var diaryProperty = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryProperty is null)
                {
                    return Result<Unit>.Failure("diaryProperty not found");
                }

                var linqQuery = _context.GetType().GetProperty(diaryProperty.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                var entity = await linqQuery.AsTracking().Where(x => x.Id == request.Id).SingleOrDefaultAsync();
                if (entity == null)
                {
                    return null;
                }
                _context.Remove(entity);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                {
                    return Result<Unit>.Failure("failed to delete the record");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}