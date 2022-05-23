using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Core;
using API.DTO;
using API.Services;
using Domain.Diaries;
using Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.CRUD
{
    public class LastDatesDiaries
    {
        public class Query : IRequest<List<DiaryViewDTO>>
        {
            public UserDoctor userDoctor { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<DiaryViewDTO>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;

            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;
            }


            public async Task<List<DiaryViewDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lastUserViewList = await _context.UsersViews.Where(v => v.UserDoctorId == request.userDoctor.Id).ToListAsync();
                var diariesDatesList = new List<DiaryViewDTO> { };
                var taskList = new List<Task<DateTime>>();
                var dates = new List<DateTime>();
                foreach (var v in lastUserViewList)
                {
                    var diaryInfo = _diaryService.getDiaryTypeByName(v.DiaryName);
                    var diary = _context.GetType().GetProperty(diaryInfo.PropertyName).GetValue(_context) as IQueryable<BaseDiary>;
                    dates.Add(await diary.OrderBy(r => r.ChangeDate).Select(r => r.ChangeDate).FirstOrDefaultAsync());
                }

                for (int i = 0; i < lastUserViewList.Count; ++i)
                {
                    diariesDatesList.Add(new DiaryViewDTO
                    {
                        diaryName = lastUserViewList[i].DiaryName,
                        lastViewDate = lastUserViewList[i].LastViewDate,
                        lastModifyDate = dates[i]
                    });
                }
                return diariesDatesList;
            }
        }
    }
}