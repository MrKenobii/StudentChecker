using Backend.Data;
using Backend.DataTransferObject.Post;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories;

public class PostRepository : IPostRepository
{
    private DataContext _context;

    public PostRepository(DataContext _context)
    {
        this._context = _context;
    }
    public IEnumerable<PostResponse> GetPosts()
    {
        var posts = _context.Posts.ToList();
        
        var postResponses = new List<PostResponse>();
        foreach (var post in posts)
        {
            var student = _context.Students.Where(s => s.Posts.Any(p => p.Id == post.Id)).FirstOrDefault();
            var recruiter = _context.Recruiters.Where(s => s.Posts.Any(p => p.Id == post.Id)).FirstOrDefault();
            
            if (student != null && recruiter == null)
            {
                Console.WriteLine("STUDENT :"  + student.Id + " " + student.Name + " " + student.LastName + " " + student.Email + " " + post.Content);
                postResponses.Add(new PostResponse()
                {
                    Content = post.Content,
                    Title = post.Title,
                    Id = post.Id,
                    Recruiter = null,
                    Student = student,
                    CreatedTime = post.CreatedTime
                });
            }
            if (student == null && recruiter != null)
            {
                Console.WriteLine("RECRUITER :"  + recruiter.Id + " " + recruiter.Name + " " + recruiter.LastName + " " + recruiter.Email + " " + post.Content);
                postResponses.Add(new PostResponse()
                {
                    Content = post.Content,
                    Title = post.Title,
                    Id = post.Id,
                    Recruiter = recruiter,
                    Student = null,
                    CreatedTime = post.CreatedTime
                });
            }
                
            
        }

        return postResponses;
    }

    
    public PostResponse GetPostById(int postId)
    {
        var post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
        if (post != null)
        {
            return new PostResponse()
            {
                Id = post.Id,
                Content = post.Content,
                Recruiter = post.Recruiter,
                Student = post.Student,
                Title = post.Title,
                CreatedTime = post.CreatedTime
            };
        }

        return new PostResponse();
    }

    public bool PostExists(int postId)
    {
        return _context.Posts.Any(p => p.Id == postId);
    }

    public PostResponse CreatePostForStudent(PostCreateRequest request)
    {
        if (request.StudentId != null && request.StudentId > 0)
        {
            var student = _context.Students.Where(s => s.Id == request.StudentId).FirstOrDefault();
            var post = new Post()
            {
                Content = request.Content,
                Title = request.Title,
                Student = student,
                Recruiter = null,
                CreatedTime = DateTime.Now
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return new PostResponse()
            {
                Id = post.Id,
                Content = post.Content,
                Title = post.Title,
                Student = student,
                Recruiter = null,
                CreatedTime = post.CreatedTime
            };
        }

        return new PostResponse();


    }

    public PostResponse CreatePostForRecruiter(PostCreateRequest request)
    {
        if (request.RecruiterId != null && request.RecruiterId > 0)
        {
            var recruiter = _context.Recruiters.Where(s => s.Id == request.RecruiterId).FirstOrDefault();
            var post = new Post()
            {
                Content = request.Content,
                Title = request.Title,
                Student = null,
                Recruiter = recruiter,
                CreatedTime = DateTime.Now
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return new PostResponse()
            {
                Id = post.Id,
                Content = post.Content,
                Title = post.Title,
                Student = null,
                Recruiter = recruiter,
                CreatedTime = post.CreatedTime
            };
        }

        return new PostResponse();
    }

    public IEnumerable<GetPostsByStudentResponse> GetPostByStudentId(int studentId)
    {
        var posts = _context.Posts.Where(p => p.Student.Id == studentId).ToList();
        var list = new List<GetPostsByStudentResponse>();

        foreach (var post in posts)
        {
            list.Add(new GetPostsByStudentResponse()
            {
                CreatedTime = post.CreatedTime,
                Content = post.Content,
                Id = post.Id,
                Title = post.Title
            });
        }
        if(list.Count > 0)
            return list;
        return new List<GetPostsByStudentResponse>();
    }

    public IEnumerable<GetPostsByRecruiterResponse> GetPostByRecruiterId(int recruiterId)
    {
        var posts = _context.Posts.Where(p => p.Recruiter.Id == recruiterId).ToList();
        var list = new List<GetPostsByRecruiterResponse>();

        foreach (var post in posts)
        {
            list.Add(new GetPostsByRecruiterResponse()
            {
                CreatedTime = post.CreatedTime,
                Content = post.Content,
                Id = post.Id,
                Title = post.Title
            });
        }
        if(list.Count > 0)
            return list;
        return new List<GetPostsByRecruiterResponse>();
    }
}