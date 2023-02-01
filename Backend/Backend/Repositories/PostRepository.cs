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
                });
            }
                
            
        }

        return postResponses;
    }

    private Student? GetStudentsInside(int postId)
    {
        var student = _context.Students.Where(s => s.Id == 1).FirstOrDefault();
        Console.WriteLine(student.Posts.Count);
        return student;
    }

    private Recruiter? GetRecruiterInside(int postId)
    {
        var recruiter = _context.Recruiters.Where(s => s.Id == 4).FirstOrDefault();
        
        return recruiter;
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
                Title = post.Title
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
                Recruiter = null
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return new PostResponse()
            {
                Content = post.Content,
                Title = post.Title,
                Student = student,
                Recruiter = null
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
                Recruiter = recruiter
            };
            _context.Posts.Add(post);
            _context.SaveChanges();
            return new PostResponse()
            {
                Content = post.Content,
                Title = post.Title,
                Student = null,
                Recruiter = recruiter
            };
        }

        return new PostResponse();
    }
}