using System.Runtime.CompilerServices;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly DataContext _context;
    public CollegeRepository(DataContext dataContext)
    {
        this._context = dataContext;
    }
    public ICollection<CollegeGetResponse> GetColleges()
    {
        var _colleges = new List<CollegeGetResponse>();
        var colleges = _context.Colleges.ToList();
        if (colleges.Count > 0)
        {
            foreach (var college in colleges)
            {
                _colleges.Add(new CollegeGetResponse()
                {
                    City = this.GetCityByCollege(college.Id),
                    EmailExtension = college.EmailExtension,
                    FoundationDate = college.FoundationDate,
                    Id = college.Id,
                    Name = college.Name,
                    Students = this.GetStudentsInside(college.Id)
                });
            }

            return _colleges;
        }

        return new List<CollegeGetResponse>();

    }

    public College GetCollege(int id)
    {
        return _context.Colleges.Where(c => c.Id == id).FirstOrDefault();
    }

    public CollegeGetResponse GetCollegeById(int collegeId)
    {
        var college = _context.Colleges.Where(c => c.Id == collegeId).FirstOrDefault();
        if (college != null)
        {
            return new CollegeGetResponse()
            {
                City = this.GetCityByCollege(college.Id),
                EmailExtension = college.EmailExtension,
                FoundationDate = college.FoundationDate,
                Id = college.Id,
                Name = college.Name,
                Students = this.GetStudentsInside(college.Id)
            };
        }

        return new CollegeGetResponse();
    }


    public CityDto GetCityByCollege(int collegeId)
    {
        var college = this.GetCollege(collegeId);
        Console.WriteLine("College Name " + college.Name);
        
        var city = _context.Colleges.Where(o => o.Id == collegeId).Select(c => c.City).FirstOrDefault();
        if (city != null)
        {
            Console.WriteLine("City : " + city.Name);
            return new CityDto()
            {
                Name = city.Name,
                Id = city.Id
            };
        }

        return new CityDto()
        {

        };
    }

    public ICollection<Student> GetStudentsByCollege(int id)
    {
        return _context.Students.Where(c => c.College.Id == id).ToList();
    }

    public bool CollegeExists(int collegeId)
    {
        return _context.Colleges.Any(c => c.Id == collegeId);
    }

    private ICollection<StudentDto> GetStudentsInside(int collegeId)
    {
        Console.WriteLine("Inside GetStudentsInside()");
        var students = _context.Students.ToList();
        var _students = new List<StudentDto>();
        foreach (var student in students)
        {
            Console.WriteLine("Iteration 1" + " " + student.Name);
            var _city = _context.Students.Where(o => o.Id == student.Id).Select(c => c.City).FirstOrDefault();
            var _college = _context.Students.Where(o => o.Id == student.Id).Select(c => c.College).FirstOrDefault();
            if (_city != null)
            {
                if (_college.Id == collegeId)
                {
                    _students.Add(new StudentDto()
                    {
                        Name = student.Name,
                        LastName = student.LastName,
                        Address = student.Address,
                        CityName = _city.Name, //City Name
                        CollegeName = _college.Name, // CollegeName
                        Phone = student.Phone,
                        Skills = student.Skills,
                        Languages = student.Languages,
                        DateOfBirth = student.DateOfBirth,
                        Department = student.Department,
                        Email = student.Email,
                        EnrollDate = student.EnrollDate,
                        Id = student.Id,
                        Image = student.Image,
                        IsActivated = student.IsActivated
                        
                    });
                    Console.WriteLine(student.Name);
                }    
            }
            else
            {
                Console.WriteLine("City is null");
            }
            
        }
        return _students;
    }
    public CollegePostResponse CreateCollege(CollegePostRequest collegeDto)
    {
        if (collegeDto != null)
        {
            Console.WriteLine("College DTO:  " + collegeDto.CityName);
            var cityByName = _context.Cities.Where(e => e.Name == collegeDto.CityName).FirstOrDefault();
            // var cityByName = _cityRepository.GetCityByName(collegeDto.Name);
            var college = new College()
            {
                Name = collegeDto.Name,
                FoundationDate = collegeDto.FoundationDate,
                EmailExtension = collegeDto.EmailExtension
            };
            Console.WriteLine("COLLEGE: " + college.Name + " " + college.Id + " " + college.FoundationDate);
            if (cityByName != null)
            {
                Console.WriteLine("CITY: " + cityByName.Name + " " + cityByName.Id);
                college.Name = collegeDto.Name;
                college.FoundationDate = collegeDto.FoundationDate;
                college.City = cityByName;
                
                _context.Colleges.Add(college);
                _context.SaveChanges();
                return new CollegePostResponse()
                {
                    CityName = cityByName.Name,
                    FoundationDate = college.FoundationDate,
                    Id = college.Id,
                    Message = "College " + college.Name + " was successfully added",
                    Name = college.Name
                };
            }
            return new CollegePostResponse()
            {
                CityName = null,
                FoundationDate = new DateTime(),
                Id = 0,
                Message = "Something went wrong!!!",
                Name = null
            };

        }
        return new CollegePostResponse()
        {
            CityName = null,
            FoundationDate = new DateTime(),
            Id = 0,
            Message = "Payload is not valid!! Check your request again",
            Name = null
        };
        
    }

    public CollegePostResponse UpdateCollege(int collegeId, CollegePostRequest collegeDto)
    {
        if (collegeDto != null)
        {
            var cityByName = _context.Cities.Where(e => e.Name == collegeDto.CityName).FirstOrDefault();
            var college = this.GetCollege(collegeId);
        
            if (collegeDto.Name != null)
            {
                college.Name = collegeDto.Name;    
            }

            if (collegeDto.FoundationDate != null)
            {
                college.FoundationDate = collegeDto.FoundationDate;
            }

            if (collegeDto.CityName != null)
            {
                college.City = cityByName;
            }

            if (collegeDto.EmailExtension != null)
            {
                college.EmailExtension = collegeDto.EmailExtension;
            }
        
        
            _context.Colleges.Update(college);
            _context.SaveChanges();
            return new CollegePostResponse()
            {
                CityName = cityByName.Name,
                FoundationDate = college.FoundationDate,
                Id = college.Id,
                Message = "College " + college.Name + " was successfully updated",
                Name = college.Name
            };    
        }
        return new CollegePostResponse()
        {
            CityName = null,
            FoundationDate = new DateTime(),
            Id = 0,
            Message = "Payload is not valid!! Check your request again",
            Name = null
        };
        
    }

    public void DeleteCollege(int collegeId)
    {
        _context.Colleges.Remove(this.GetCollege(collegeId));
        _context.SaveChanges();
    }
}