using CPALMSAssessment.Application.AssessmentTests.Commands.DTO;
using CPALMSAssessment.Application.AssessmentTests.Dtos;
using CPALMSAssessment.Application.Common.Interfaces;
using CPALMSAssessment.Application.Common.Models.Common;
using CPALMSAssessment.Domain.Exceptions;
using MediatR; 
using System.Data; 

namespace CPALMSAssessment.Application.AssessmentTests.Commands
{
    public class UpdateTestCommand : IRequest<bool>
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int NoOfQuestions { get; set; } = 0;

        public List<Guid> SubjectAreaIds { get; set; } = new();
        public List<string> GradeCodes { get; set; } = new();
        public List<Guid> StrandIds { get; set; } = new();
        public List<Guid> BenchmarkIds { get; set; } = new();

        public List<QuestionOrderNumberDTO> Questions { get; set; } = new();
        public List<Guid> AssignedClassIds { get; set; } = new();
        /// <summary>
        /// Automatic = 0
        /// Manual = 1
        /// </summary>
        public TestMode TestMode { get; set; } = TestMode.Automatic;
    }



    internal class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, bool>
    {
        private readonly IReadOnlyAssessmentContext _rawContext;
        private readonly IAssessmentContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateTestCommandHandler(IReadOnlyAssessmentContext rawContext,
                                        IAssessmentContext context,
                                        ICurrentUserService currentUserService,
                                        IDateTimeService dateTimeService)
        {
            _rawContext = rawContext;
            _context = context;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }


        public async Task<bool> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.SubjectAreaIds.Count == 0 &&
                    request.GradeCodes.Count == 0 &&
                    request.StrandIds.Count == 0 &&
                    request.BenchmarkIds.Count == 0)
                    throw new BusinessRuleException("Update Test", "At least one search parameter is required");


                if (request.Questions.Count == 0)
                    throw new BusinessRuleException("Update Test", "Questions is required");

                if (request.Questions.Count != request.NoOfQuestions)
                    throw new BusinessRuleException("Update Test", "Questions is not equal to NoOfQuestions");

                var jsonCriteria = new SearchCriteriaModel(request.SubjectAreaIds, request.GradeCodes, request.StrandIds, request.BenchmarkIds, request.TestMode).ToJsonString();


                 await SaveTest(request.Id, request.Name, request.Description, jsonCriteria, request.Questions,
                                            request.AssignedClassIds, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessRuleException("UpdateTest", ex.Message);
            }
        }

        public async Task SaveTest(Guid testId, string testName, string description, string searchCriteria,
                                   List<QuestionOrderNumberDTO> listQuestions,
                                   List<Guid> listClasses,
                                   CancellationToken cancellationToken)
        {
            await _rawContext.ExecuteWithTableParamsAsync<int>("sp_SaveTest",
                    SP_TVP_ReturnValueKind.Single_Non_Class, param: new
                    {
                        testId,
                        name = testName,
                        description = description ?? string.Empty,
                        questions = listQuestions.Count,
                        searchCriteria = searchCriteria ?? string.Empty,
                        createdOn = _dateTimeService.EstNow,
                        createdBy = _currentUserService.UserId
                    },
                new Dictionary<string, DataTable>()
                {
                    { "tableQuestion", _GetQuestionAsTable(listQuestions) },
                    { "tableClass", _GetClassAsTable(listClasses) },
                },
                isStoredProcedure: true,
                transaction: null,
                cancellationToken: cancellationToken);
        }


        private DataTable _GetQuestionAsTable(List<QuestionOrderNumberDTO> questions)
        {
            DataTable table = new("dbo.TVP_IdCountField");
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("CountNumber", typeof(int));

            questions = questions.OrderBy(x => x.OrderNumber).ToList();
            questions.ForEach(c => table.Rows.Add(c.Id, c.OrderNumber));

            return table;
        }

        private DataTable _GetClassAsTable(List<Guid> classes)
        {
            DataTable table = new("dbo.TVP_IdCountField");
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("CountNumber", typeof(int));

            classes?.ForEach(c => table.Rows.Add(c, 0));

            return table;
        }




    }




}
