using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using API.DTO;
using API.Services;
using Domain.Diaries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System.Linq;
using API.Core;
using System.Text.Json.Nodes;

namespace API.CRUD
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public JsonObject Body { get; set; }
            public string DiaryName { get; set; }
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

                var record = (BaseDiary)request.Body.Deserialize(diaryProperty.PropertyTypeInfo);
                record.Date = System.DateTime.UtcNow;
                await _context.AddAsync(record);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to create record");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}