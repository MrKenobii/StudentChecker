using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface IStudentRepository
{
    ICollection<Student> GetStudents();
    Student GetStudent(int id);
    CityDto GetStudentCity(int studentId);
    CollegeDto GetStudentCollege(int studentId);
    ICollection<CourseDto> GetCourses(int id);
    bool StudentExists(int studentId);
    Student CreateStudent(StudentDto studentDto);
    StudentResponse UpdateStudent(int studentId, StudentDto studentDto);
    string DeleteStudent(int studentId);
    string AddCourses(int studentId, AddCourseToStudent addCourseToStudent);
    StudentResponse UpdateStudentProfile(int studentId, StudentUpdateProfile studentUpdateProfile);
    StudentResponse Signup(StudentSignUpRequest signUpRequest);
    StudentLoginReponse Login(StudentLoginRequest loginRequest);
    string SendEmail(StudentSignUpRequest signUpRequest);
    StudentVerifyAccountResponse VerifyAccount(int studentId, StudentVerifyAccountRequest studentVerifyAccountRequest);
}