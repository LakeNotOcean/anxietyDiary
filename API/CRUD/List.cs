using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.Core;
using API.Services;
using Domain.Diaries;
using Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

            public TimeZoneInfo TimeZone { get; set; }

            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PageList<BaseDiary>>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;
            private readonly UserManager<User> _userManager;

            public Handler(DataContext context, DiaryService diaryService, UserManager<User> userManager)
            {
                _userManager = userManager;
                _diaryService = diaryService;
                _context = context;
            }

            public async Task<Result<PageList<BaseDiary>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var diaryInfo = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryInfo is null)
                {
                    return Result<PageList<BaseDiary>>.Failure("diary not found");
                }
                var diary = _context.GetType().GetProperty(diaryInfo.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;

                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user is null)
                {
                    return Result<PageList<BaseDiary>>.Failure("User is not defined");
                }

                var utcBeginDateTime = request.Date.toBeginOfDayUtc(request.TimeZone);
                var utcEndDateTime = request.Date.toEndOfDayUtc(request.TimeZone);

                var result = diary.Where(d => d.Date >= utcBeginDateTime && d.Date <= utcEndDateTime && d.DiaryUserId == user.Id).OrderBy(d => d.Date).AsQueryable();
                return Result<PageList<BaseDiary>>.Success(await PageList<BaseDiary>.CreateAsync(
                    result, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}