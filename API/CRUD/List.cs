using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using Domain.Diaries;
using MediatR;
using Persistance;


namespace Api.CRUD
{
    public class List
    {
        public class Query : IRequest<List<BaseDiary>>
        {
            public string DiaryName { get; set; }
            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<BaseDiary>>
        {
            private readonly DataContext _context;
            private readonly DiaryService _diaryService;

            public Handler(DataContext context, DiaryService diaryService)
            {
                _diaryService = diaryService;
                _context = context;

            }

            public async Task<List<BaseDiary>> Handle(Query request, CancellationToken cancellationToken)
            {

                var diary = _context.InitialDiaries;
            }
        }
    }
}