using Backend.DataTransferObject.Message;
using Backend.Models;

namespace Backend.Interfaces;

public interface IMessageRepository
{
    IEnumerable<GetSendMessageResponse> GetSendMessages();
    IEnumerable<GetDeliveredMessageResponse> GetDeliveredMessages();
    FromRecruiterToStudentResponse CreateMessageRecruiterToStudent(MessagePostRequest messagePostRequest);
    FromStudentToRecruiterResponse CreateMessageStudentToRecruiter(MessagePostRequest messagePostRequest);
    List<GetSendMessageResponse> GetSendMessageStudentById(int studentId);
    List<GetSendMessageResponse> GetDeliveredMessageStudentById(int studentId);
    List<GetSendMessageResponse> GetSendRecruiterMessageById(int recruiterId);
    List<GetSendMessageResponse> GetDeliveredRecruiterMessageById(int recruiterId);
}