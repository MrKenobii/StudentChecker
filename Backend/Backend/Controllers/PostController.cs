using AutoMapper;
using Backend.DataTransferObject.Post;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public PostController(IPostRepository postRepository, IMapper mapper)
    {
        this._postRepository = postRepository;
        this._mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostResponse>))]
    public IActionResult GetPosts()
    {
        var posts = _postRepository.GetPosts();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(posts);
    }

    [HttpGet("{postId}")]
    [ProducesResponseType(200, Type = typeof(PostResponse))]
    [ProducesResponseType(400)]
    public IActionResult GetPostById(int postId)
    {
        if (!_postRepository.PostExists(postId))
            return NotFound();
        var post = _postRepository.GetPostById(postId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(post);
    }
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetPostsByStudentResponse>))]
    [ProducesResponseType(400)]
    public IActionResult GetPostsByStudentId(int studentId)
    {
        var posts = _postRepository.GetPostByStudentId(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(posts);
    }
    [HttpGet("recruiter/{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetPostsByRecruiterResponse>))]
    [ProducesResponseType(400)]
    public IActionResult GetPostsByRecruiterId(int recruiterId)
    {
        var posts = _postRepository.GetPostByRecruiterId(recruiterId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(posts);
    }

    [HttpPost("student")]
    [ProducesResponseType(201, Type = typeof(PostResponse))]
    [ProducesResponseType(400)]
    public IActionResult CreatePostForStudent(PostCreateRequest request)
    {
       var postResponse = _postRepository.CreatePostForStudent(request);
       if (!ModelState.IsValid)
           return BadRequest();
       return Ok(postResponse);
    }
    [HttpPost("recruiter")]
    [ProducesResponseType(201, Type = typeof(PostResponse))]
    [ProducesResponseType(400)]
    public IActionResult CreatePostForRecruiter(PostCreateRequest request)
    {
       var postResponse = _postRepository.CreatePostForRecruiter(request);
       if (!ModelState.IsValid)
           return BadRequest();
       return Ok(postResponse);
    }
    
}