using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Interfaces;

public interface IStudentRepository
{
    ICollection<Student> GetStudents();
    Student GetStudent(int id);
    Student GetStudentCity(int city);
    ICollection<Course> GetCourses(int id);
    bool StudentExists(int studentId);
    Student CreateStudent(StudentDto studentDto);
    Student UpdateStudent(int studentId, StudentDto studentDto);
    void DeleteStudent(int studentId);
    Student AddCourses(int studentId, AddCourseToStudent addCourseToStudent);
    Student UpdateStudentProfile(int studentId, StudentUpdateProfile studentUpdateProfile);
    Student Signup(StudentSignUpRequest signUpRequest);
    StudentLoginReponse Login(StudentLoginRequest loginRequest);
}