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
        public class Command : IRequest<Result<int>>
        {
            public JsonObject Body { get; set; }
            public string DiaryName { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<int>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;
            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var diaryProperty = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryProperty is null)
                {
                    return Result<int>.Failure("diaryProperty not found");
                }

                var record = (BaseDiary)request.Body.Deserialize(diaryProperty.PropertyTypeInfo);
                record.Date = System.DateTime.UtcNow;
                record.ChangeDate = System.DateTime.UtcNow;
                await _context.AddAsync(record);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<int>.Failure("Failed to create record");
                }
                return Result<int>.Success(record.Id);
            }
        }
    }
}