using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using API.DTO;
using API.Services;
using Domain.Diaries;
using MediatR;
using Persistance;
using System.Linq;
using API.Core;
using System.Text.Json.Nodes;
using Api.Extensions;
using Microsoft.AspNetCore.Identity;
using Domain.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace API.CRUD
{
    public class Create
    {
        public class Command : IRequest<Result<int>>
        {
            public JsonObject Body { get; set; }
            public string DiaryName { get; set; }
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<int>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;
            private readonly JsonSerializerOptions _opt;
            private readonly UserManager<User> _userManager;
            public Handler(DataContext context, DiaryService diaryService, UserManager<User> userManager)
            {
                _userManager = userManager;
                _diaryService = diaryService;
                _context = context;
                _opt = new JsonSerializerOptions();
                _opt.Converters.Add(new DateTimeConverter());

            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var diaryProperty = _diaryService.getDiaryTypeByName(request.DiaryName);
                if (diaryProperty is null)
                {
                    return Result<int>.Failure("diaryProperty not found");
                }
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user is null)
                {
                    return Result<int>.Failure("User is not defined");
                }
                var record = (BaseDiary)request.Body.Deserialize(diaryProperty.PropertyTypeInfo, _opt);
                record.Date = System.DateTime.UtcNow;
                record.ChangeDate = System.DateTime.UtcNow;
                record.DiaryUserId = user.Id;
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