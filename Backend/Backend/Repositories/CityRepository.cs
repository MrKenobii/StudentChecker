using System.Collections;
using Backend.Data;
using Backend.DataTransferObject;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class CityRepository : ICityRepository
{
    
    private readonly DataContext _context;
    public CityRepository(DataContext _context)
    {
        this._context = _context;
    }
    public ICollection<CityResponse> GetCities()
    {
        var _cities = new List<CityResponse>();
        var cities = _context.Cities.ToList();
        if (cities.Count > 0)
        {
            foreach (var city in cities)
            {
                _cities.Add(new CityResponse()
                {
                    Name = city.Name,
                    Id = city.Id,
                    Colleges = this.GetCollegesInside(city.Id),
                    Companies = this.GetCompaniesInside(city.Id),
                    Students = this.GetStudentsInside(city.Id)
                });
            }    
        }

        return _cities;

    }

    public City GetCity(int id)
    {
        return _context.Cities.Where(e => e.Id == id).FirstOrDefault();
    }

    public CityResponse GetCityResponse(int id)
    {
        var city = _context.Cities.Where(e => e.Id == id).FirstOrDefault();
        if (city != null)
        {
            return new CityResponse()
            {
                Name = city.Name,
                Id = city.Id,
                Colleges = this.GetCollegesInside(city.Id),
                Companies = this.GetCompaniesInside(city.Id),
                Students = this.GetStudentsInside(city.Id)
            };
        }

        return new CityResponse();
    }

    public City GetCityByName(string collegeName)
    {
        return _context.Cities.Where(e => e.Name == collegeName).FirstOrDefault();
    }

    public bool CityExists(int id)
    {
        return _context.Cities.Any(c => c.Id == id);
    }

    public List<CompanyDto> GetCompaniesByCity(int id)
    {
        Console.WriteLine("Inside GetCompaniesByCity()");
        var companies = _context.Companies.ToList();
        var _companies = new List<CompanyDto>();
        foreach (var company in companies)
        {
            Console.WriteLine("Iteration 1" + " " + company.Name);
            var _city = _context.Companies.Where(o => o.Id == company.Id).Select(c => c.City).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
                {
                    _companies.Add(new CompanyDto()
                    {
                        CityName = _city.Name,
                        Id = company.Id,
                        Name = company.Name,
                        CompanyType = company.CompanyType,
                        Email = company.Email,
                        FoundationDate = company.FoundationDate,
                        Address = company.Address,
                        Phone = company.Phone,
                        CompanyKey = company.CompanyKey
                        
                    });
                    Console.WriteLine(company.Name);
                }    
            }
            else
            {
                Console.WriteLine("City is null");
            }
            
        }
        return _companies;

    }

    private ICollection<CompanyDto> GetCompaniesInside(int id)
    {
        Console.WriteLine("Inside GetCompaniesByCity()");
        var companies = _context.Companies.ToList();
        var _companies = new List<CompanyDto>();
        foreach (var company in companies)
        {
            Console.WriteLine("Iteration 1" + " " + company.Name);
            var _city = _context.Companies.Where(o => o.Id == company.Id).Select(c => c.City).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
                {
                    _companies.Add(new CompanyDto()
                    {
                        CityName = _city.Name, // City Name
                        Id = company.Id,
                        Name = company.Name,
                        CompanyType = company.CompanyType,
                        Email = company.Email,
                        FoundationDate = company.FoundationDate,
                        Address = company.Address,
                        Phone = company.Phone,
                        CompanyKey = company.CompanyKey
                        
                    });
                    Console.WriteLine(company.Name);
                }    
            }
            else
            {
                Console.WriteLine("City is null");
            }
            
        }
        return _companies;
    }

    private ICollection<CollegeResponse> GetCollegesInside(int id)
    {
        Console.WriteLine("Inside GetCollegesByCity()");
        var colleges = _context.Colleges.ToList();
        var _colleges = new List<CollegeResponse>();
        foreach (var college in colleges)
        {
            var students = _context.Students.Where(c => c.College.Id == college.Id).ToList();
            Console.WriteLine(students.Count);
            Console.WriteLine("Iteration 1" + " " + college.Name);
            var _city = _context.Colleges.Where(o => o.Id == college.Id).Select(c => c.City).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
                {
                    _colleges.Add(new CollegeResponse()
                    {
                        EmailExtension = college.EmailExtension,
                        FoundationDate = college.FoundationDate,
                        Id = college.Id,
                        Name = college.Name, // Student Count and City Name
                        CityName = _city.Name,
                        StudentCount = students.Count
                    });
                    Console.WriteLine(college.Name);
                }    
            }
            else
            {
                Console.WriteLine("City is null");
            }
            
        }
        return _colleges;
    }
    
    private ICollection<StudentDto> GetStudentsInside(int id)
    {
        Console.WriteLine("Inside GetStudentsByCity()");
        var students = _context.Students.ToList();
        var _students = new List<StudentDto>();
        foreach (var student in students)
        {
            Console.WriteLine("Iteration 1" + " " + student.Name);
            var _city = _context.Students.Where(o => o.Id == student.Id).Select(c => c.City).FirstOrDefault();
            var _college = _context.Students.Where(o => o.Id == student.Id).Select(c => c.College).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
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
    public List<CollegeDto> GetCollegesByCity(int id)
    {
        Console.WriteLine("Inside GetCollegesByCity()");
        var colleges = _context.Colleges.ToList();
        var _colleges = new List<CollegeDto>();
        foreach (var college in colleges)
        {
            Console.WriteLine("Iteration 1" + " " + college.Name);
            var _city = _context.Colleges.Where(o => o.Id == college.Id).Select(c => c.City).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
                {
                    _colleges.Add(new CollegeDto()
                    {
                        CityName = _city.Name,
                        EmailExtension = college.EmailExtension,
                        FoundationDate = college.FoundationDate,
                        Id = college.Id,
                        Name = college.Name
                    });
                    Console.WriteLine(college.Name);
                }    
            }
            else
            {
                Console.WriteLine("City is null");
            }
            
        }
        return _colleges;
    }

    public List<StudentDto> GetStudentsByCity(int id)
    {
        Console.WriteLine("Inside GetStudentsByCity()");
        var students = _context.Students.ToList();
        var _students = new List<StudentDto>();
        foreach (var student in students)
        {
            Console.WriteLine("Iteration 1" + " " + student.Name);
            var _city = _context.Students.Where(o => o.Id == student.Id).Select(c => c.City).FirstOrDefault();
            if (_city != null)
            {
                if (_city.Id == id)
                {
                    _students.Add(new StudentDto()
                    {
                        Name = student.Name,
                        LastName = student.LastName,
                        Address = student.Address,
                        CityName = _city.Name,
                        CollegeName = "",
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

    
    public CityPostResponse CreateCity(CityPostRequest cityDto)
    {
        if (cityDto != null)
        {
            var city = new City()
            {
                Name = cityDto.Name
            };
            Console.WriteLine(city);
            _context.Cities.Add(city);
            _context.SaveChanges();
            return new CityPostResponse()
            {
                Id = city.Id,
                Message = "City " + city.Name + " was successfully added",
                Name = city.Name
            };    
        }

        return new CityPostResponse()
        {
            Id = 0,
            Message = "Something went wrong!! Check your request payload",
            Name = null
        };

    }

    public CityPostResponse UpdateCity(int cityId, CityPostRequest cityDto)
    {
        var city = this.GetCity(cityId);
        if (city != null)
        {
            if (cityDto != null)
            {
                city.Name = cityDto.Name;
                _context.Cities.Update(city);
                _context.SaveChanges();
                return new CityPostResponse()
                {
                    Id = city.Id,
                    Message = "City " + city.Name + " was successfully updated",
                    Name = city.Name
                };    
            }

            return new CityPostResponse()
            {
                Id = 0,
                Message = "Something went wrong!! Check your request payload",
                Name = null
            };
        }
        
        return new CityPostResponse()
        {
            Id = 0,
            Message = "City id " + cityId + " not exists!!!",
            Name = null
        };
    }

    public void DeleteCity(int cityId)
    {
        _context.Cities.Remove(this.GetCity(cityId));
        _context.SaveChanges();
    }

    
}