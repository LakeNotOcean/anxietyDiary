using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Api.Extensions;
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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
            public string DiaryName { get; set; }
            public JsonObject Body { get; set; }

            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;
            private readonly JsonSerializerOptions _opt;
            public Handler(DataContext context, DiaryService diaryService, UserManager<User> userManager)
            {
                _userManager = userManager;
                _diaryService = diaryService;
                _context = context;
                _opt = new JsonSerializerOptions();
                _opt.Converters.Add(new DateTimeConverter());
            }
            private readonly UserManager<User> _userManager;

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var diaryProperty = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryProperty is null)
                {
                    return Result<Unit>.Failure("diaryProperty not found");
                }
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user is null)
                {
                    return Result<Unit>.Failure("User is not defined");
                }

                var record = (BaseDiary)request.Body.Deserialize(diaryProperty.PropertyTypeInfo, _opt);
                var linqQuery = _context.GetType().GetProperty(diaryProperty.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                var entity = await linqQuery.AsNoTracking().Where(x => x.Id == request.Id && x.DiaryUserId == user.Id).SingleOrDefaultAsync();
                if (entity is null)
                {
                    return null;
                }


                record.Date = entity.Date;
                entity = record;
                entity.DiaryUserId = user.Id;
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