using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string DiaryName { get; set; }
            public JsonObject Body { get; set; }
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

                var record = (BaseDiary)request.Body.Deserialize(diaryProperty.PropertyTypeInfo);
                var linqQuery = _context.GetType().GetProperty(diaryProperty.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                var entity = await linqQuery.AsNoTracking().Where(x => x.Id == request.Id).SingleOrDefaultAsync();
                if (entity is null)
                {
                    return null;
                }



                entity = record;
                entity.Id = request.Id;
                entity.ChangeDate = DateTime.UtcNow;

                _context.Entry(entity).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                {
                    return Result<Unit>.Failure("Failed to update record");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}