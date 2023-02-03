using Backend.DataTransferObject;
using Backend.DataTransferObject.Recruiter;
using Backend.Models;

namespace Backend.Interfaces;

public interface IStudentRepository
{
    ICollection<StudentDto> GetStudents();
    Student GetStudent(int id);
    StudentDto GetStudentById(int studentId);
    CityDto GetStudentCity(int studentId);
    CollegeDto GetStudentCollege(int studentId);
    ICollection<CourseDto> GetCourses(int id);
    bool StudentExists(int studentId);
    StudentPostResponse CreateStudent(StudentPostRequest studentDto);
    StudentPostResponse UpdateStudent(int studentId, StudentPostRequest studentDto);
    DeleteResponse DeleteStudent(int studentId);
    string AddCourses(int studentId, AddCourseToStudent addCourseToStudent);
    StudentResponse UpdateStudentProfile(int studentId, StudentUpdateProfile studentUpdateProfile);
    StudentResponse Signup(StudentSignUpRequest signUpRequest);
    StudentLoginReponse Login(StudentLoginRequest loginRequest);
    string SendEmail(StudentSignUpRequest signUpRequest);
    StudentVerifyAccountResponse VerifyAccount(int studentId, StudentVerifyAccountRequest studentVerifyAccountRequest);
    StudentDto GetStudentByKey(string key);

    StudentGetKeyResponse GetKeyByStudentId(int id);
    StudentEditProfileResponse EditStudentProfile(int studentId, StudentEditProfile studentEditProfile);
    ChangePasswordResponse ChangePassword(int studentId, ChangePasswordRequest request);
    ICollection<StudentRandomResponse> GetRandomStudents(int studentId);
}