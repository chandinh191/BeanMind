namespace Application.Common;

public static class ControllerRouteName
{
    private const string ApiVersion = "v1";
    public const string BaseApi = "/api/" + ApiVersion;

    public const string ChapterRoute = BaseApi + "/chapters";
    public const string QuestionRoute = BaseApi + "/questions";
    public const string QuestionAnswerRoute = BaseApi + "/question-answers";
    public const string QuestionLevelRoute = BaseApi + "/question-levels";
    public const string SubjectRoute = BaseApi + "/subjects";
    public const string CourseRoute = BaseApi + "/courses";
    public const string TopicRoute = BaseApi + "/topics";
    public const string UserRoute = BaseApi + "/auth";
    public const string WorksheetRoute = BaseApi + "/worksheets";
    public const string WorksheetQuestionRoute = BaseApi + "/worksheet-questions";
    public const string WorksheetTemplateRoute = BaseApi + "/worksheet-templates";


    public const string EnrollmentRoute = BaseApi + "/enrollments";
    public const string CourseLevelRoute = BaseApi + "/course-levels";
    public const string ProgramTypeRoute = BaseApi + "/program-types";
    public const string ParticipantRoute = BaseApi + "/participants";


}
