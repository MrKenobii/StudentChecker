using Backend.DataTransferObject.Post;

namespace Backend.Interfaces;

public interface IPostRepository
{
    IEnumerable<PostResponse> GetPosts();
    PostResponse GetPostById(int postId);
    bool PostExists(int postId);
    PostResponse CreatePostForStudent(PostCreateRequest request);
    PostResponse CreatePostForRecruiter(PostCreateRequest request);
    IEnumerable<GetPostsByStudentResponse> GetPostByStudentId(int studentId);
    IEnumerable<GetPostsByRecruiterResponse> GetPostByRecruiterId(int recruiterId);
}