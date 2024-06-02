using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.GradesDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.GradeServ
{
    public class GradeService
    {
        private readonly DatabaseContext databaseContext;
        public GradeService(DatabaseContext _databaseContext)
        {
            databaseContext = _databaseContext;

        }

        public async void AddGrade(int userId, int exerciseId, int? fileId, string? comment)
        {
            Grade grade = new Grade(userId, exerciseId, fileId, comment);
            databaseContext.Grades.Add(grade);
            databaseContext.SaveChanges();
        }
    }
}
