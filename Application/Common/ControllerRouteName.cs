namespace Application.Common
{
    public static class ControllerRouteName
    {
        private const string ApiVersion = "v1";
        public const string BaseApi = "/api/" + ApiVersion;

        public const string UserRoute = BaseApi + "/auth";
        public const string ChapterRoute = BaseApi + "/chapters";
        public const string ChapterGameRoute = BaseApi + "/chapter-games";
        public const string CourseRoute = BaseApi + "/courses";
        public const string CourseLevelRoute = BaseApi + "/course-levels";
        public const string EnrollmentRoute = BaseApi + "/enrollments";
        public const string GameRoute = BaseApi + "/games";
        public const string GameHistoryRoute = BaseApi + "/game-histories";
        public const string LevelTemplateRelationRoute = BaseApi + "/level-template-relations";
        public const string ParticipantRoute = BaseApi + "/participants";
        public const string ProcessionRoute = BaseApi + "/processions";
        public const string ProgramTypeRoute = BaseApi + "/program-types";
        public const string QuestionAnswerRoute = BaseApi + "/question-answers";
        public const string QuestionRoute = BaseApi + "/questions";
        public const string QuestionLevelRoute = BaseApi + "/question-levels";
        public const string SessionRoute = BaseApi + "/sessions";
        public const string SubjectRoute = BaseApi + "/subjects";
        public const string TeachableRoute = BaseApi + "/teachables";
        public const string TeachingSlotRoute = BaseApi + "/teaching-slots";
        public const string TopicRoute = BaseApi + "/topics";
        public const string WorksheetAttemptAnswerRoute = BaseApi + "/worksheet-attempt-answers";
        public const string WorksheetAttemptRoute = BaseApi + "/worksheet-attempts";
        public const string WorksheetRoute = BaseApi + "/worksheets";
        public const string WorksheetQuestionRoute = BaseApi + "/worksheet-questions";
        public const string WorksheetTemplateRoute = BaseApi + "/worksheet-templates";

        public const string TeacherRoute = BaseApi + "/teachers";
        public const string StudentRoute = BaseApi + "/students";
    }
}
