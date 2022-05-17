using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Core;
using API.Services;
using Domain.Diaries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.CRUD
{
    public class Dates
    {
        public class Query : IRequest<Result<List<DateTime>>>
        {
            public string DiaryName { get; set; }

        }
        public class Handler : IRequestHandler<Query, Result<List<DateTime>>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;

            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;
            }
            public async Task<Result<List<DateTime>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var diaryInfo = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryInfo is null)
                {
                    return Result<List<DateTime>>.Failure("diary not found");
                }

                var diary = _context.GetType().GetProperty(diaryInfo.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                var result = await diary.Select(d => d.Date.Date).Distinct().ToListAsync();
                return Result<List<DateTime>>.Success(result);
            }
        }
    }
}