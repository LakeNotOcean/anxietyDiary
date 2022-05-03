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


namespace Api.CRUD
{
    public class List
    {
        public class Query : IRequest<Result<PageList<BaseDiary>>>
        {
            public string DiaryName { get; set; }
            public DateTime Date { get; set; }
            public PagingParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PageList<BaseDiary>>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;

            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;

            }

            public async Task<Result<PageList<BaseDiary>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var diaryInfo = _diaryService.getDiaryTypeByName(request.DiaryName);

                var diary = _context.GetType().GetProperty(diaryInfo.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                var result = diary.Where(d => d.Date.Date == request.Date.Date).OrderBy(d => d.Date).AsQueryable();
                return Result<PageList<BaseDiary>>.Success(await PageList<BaseDiary>.CreateAsync(
                    result, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}