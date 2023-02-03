using Backend.Data;
using Backend.DataTransferObject.Message;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;
    public MessageRepository(DataContext _context, IOptions<AppSettings> appSettings)
    {
        this._context = _context;
        _appSettings = appSettings.Value;
    }
    public IEnumerable<GetSendMessageResponse> GetSendMessages()
    {
        var sendMessages = _context.SendMessages.ToList();
        Console.WriteLine("In here GetSendMessages() " + sendMessages.Count);
        var list = new List<GetSendMessageResponse>();
        foreach (var sendMessage in sendMessages)
        {
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            
            if (sendMessage.Recruiter != null)
            {
                Console.WriteLine("Recruiter not null +++++++++++ ");
            
                // var _recruiter = _context.Recruiters.Include(o => o.SendMessages).ThenInclude(c => c.Recruiter).FirstOrDefault();
                var _recruiter = _context.Recruiters.Where(c => c.Id == sendMessage.Recruiter.Id).FirstOrDefault();
                var deliveredM = _context.DeliveredMessages.Where(s => s.Id == sendMessage.Id).FirstOrDefault();
                
                Console.WriteLine("Send from recruiter id: " +_recruiter.Id + " " + _recruiter.Name);    
                list.Add(new GetSendMessageResponse()
                {
                    Content = sendMessage.Content,
                    RecruiterId = _recruiter.Id,
                    StudentId = deliveredM.Student.Id
                });
            }
    
            if (sendMessage.Student != null)
            {
                Console.WriteLine("Student not null ++++++++ ");
                var _student = _context.Students.Where(c => c.Id == sendMessage.Student.Id).FirstOrDefault();
                var deliveredM = _context.DeliveredMessages.Where(s => s.Id == sendMessage.Id).FirstOrDefault();
                list.Add(new GetSendMessageResponse()
                {
                    Content = sendMessage.Content,
                    RecruiterId = deliveredM.Recruiter.Id,
                    StudentId = _student.Id
                });
            }    
            
        }

        if (list.Count > 0)
        {
            return list;
        }
        return new List<GetSendMessageResponse>();

    }

    public IEnumerable<GetDeliveredMessageResponse> GetDeliveredMessages()
    {
        var deliveredMessages = _context.DeliveredMessages.ToList();
        Console.WriteLine("In here GetSendMessages() " + deliveredMessages.Count);
        var list = new List<GetDeliveredMessageResponse>();
        foreach (var deliveredMessage in deliveredMessages)
        {
            // var recruiter = _context.Recruiters.Where(s => s.Id == 18).FirstOrDefault();
            // Console.WriteLine(recruiter.SendMessages.Count);
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            if (deliveredMessage.Recruiter != null)
            {
                Console.WriteLine("Recruiter not null +++++++++++ ");
            
                // var _recruiter = _context.Recruiters.Include(o => o.SendMessages).ThenInclude(c => c.Recruiter).FirstOrDefault();
                var _recruiter = _context.Recruiters.Where(c => c.Id == deliveredMessage.Recruiter.Id).FirstOrDefault();
                var deliveredM = _context.SendMessages.Where(s => s.Id == deliveredMessage.Id).FirstOrDefault();
                Console.WriteLine("Delivered to recruiter id: " +_recruiter.Id + " " + _recruiter.Name);
                list.Add(new GetDeliveredMessageResponse()
                {
                    Content = deliveredMessage.Content,
                    RecruiterId = _recruiter.Id,
                    StudentId = deliveredM.Student.Id
                });
            }
    
            if (deliveredMessage.Student != null)
            {
                Console.WriteLine("Student not null ++++++++ ");
                var _student = _context.Students.Where(c => c.Id == deliveredMessage.Student.Id).FirstOrDefault();
                var deliveredM = _context.SendMessages.Where(s => s.Id == deliveredMessage.Id).FirstOrDefault();
                Console.WriteLine("Delivered to student id: " +_student.Id + " " + _student.Name); 
                list.Add(new GetDeliveredMessageResponse()
                {
                    Content = deliveredMessage.Content,
                    RecruiterId = deliveredM.Recruiter.Id,
                    StudentId = _student.Id
                });
            }    
            
        }

        if (list.Count > 0)
        {
            return list;
        }
        return new List<GetDeliveredMessageResponse>();
    }




    public FromRecruiterToStudentResponse CreateMessageRecruiterToStudent(MessagePostRequest messagePostRequest)
    {
        var firstOrDefault = _context.Recruiters.Where(r => r.Id == messagePostRequest.FromId).FirstOrDefault();
        if (firstOrDefault != null)
        {
            var student = _context.Students.Where(s => s.Id == messagePostRequest.ToId).FirstOrDefault();
            var recruiter = _context.Recruiters.Where(s => s.Id == messagePostRequest.FromId).FirstOrDefault();
            var time = DateTime.Now;
            var message = new SendMessage()
            {
                Student = null,
                Recruiter = recruiter,
                Content = messagePostRequest.Content,
                SendTime = time
            };
            _context.SendMessages.Add(message);
            var deliverMessage = new DeliveredMessage()
            {
                Content = messagePostRequest.Content,
                Student = student,
                Recruiter = null,
                DeliveredTime = time,

            };
            _context.DeliveredMessages.Add(deliverMessage);
            _context.SaveChanges();
            return new FromRecruiterToStudentResponse()
            {
                Content = message.Content + " was sent to " + student.Name,
                FromRecruiterId = messagePostRequest.FromId,
                ToStudentId = messagePostRequest.ToId
            };

            
        }

        return new FromRecruiterToStudentResponse()
        {
            Content = "",
            FromRecruiterId = null,
            ToStudentId = null
        };
    }

    public FromStudentToRecruiterResponse CreateMessageStudentToRecruiter(MessagePostRequest messagePostRequest)
    {
        var firstOrDefault = _context.Students.Where(r => r.Id == messagePostRequest.FromId).FirstOrDefault();
        if (firstOrDefault != null)
        {
            var student = _context.Students.Where(s => s.Id == messagePostRequest.FromId).FirstOrDefault();
            var recruiter = _context.Recruiters.Where(s => s.Id == messagePostRequest.ToId).FirstOrDefault();
            var time = DateTime.Now;
            var message = new SendMessage()
            {
                Student = student,
                Recruiter = null,
                Content = messagePostRequest.Content,
                SendTime = time
            };
            _context.SendMessages.Add(message);
            var deliverMessage = new DeliveredMessage()
            {
                Content = messagePostRequest.Content,
                Student = null,
                Recruiter = recruiter,
                DeliveredTime = time

            };
            _context.DeliveredMessages.Add(deliverMessage);
            _context.SaveChanges();
            return new FromStudentToRecruiterResponse()
            {
                Content = message.Content + " was sent to " + recruiter.Name,
                FromStudentId = messagePostRequest.FromId,
                ToRecruiterId = messagePostRequest.ToId
            };

            
        }

        return new FromStudentToRecruiterResponse()
        {
            Content = "",
            FromStudentId = null,
            ToRecruiterId = null
        };
    }

    

    public List<GetSendMessageResponse> GetSendMessageStudentById(int studentId)
    {
        var sendMessages = _context.SendMessages.Where(s => s.Student.Id == studentId).ToList();
        var list = new List<GetSendMessageResponse>();
        foreach (var sendMessage in sendMessages)
        {
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            Console.WriteLine(sendMessage.Content);
            var firstOrDefault = _context.DeliveredMessages.Where(s => s.Id == sendMessage.Id).FirstOrDefault();
            var _rec = _context.Recruiters.Where(s => s.Id == firstOrDefault.Recruiter.Id).FirstOrDefault();
            var _stu = _context.Students.Where(r => r.Id == studentId).FirstOrDefault();
            list.Add(new GetSendMessageResponse()
            {
                Content = sendMessage.Content,
                StudentId = studentId,
                RecruiterId = firstOrDefault.Recruiter.Id,
                SendTime = sendMessage.SendTime,
                DeliveredTime = null,
                RecruiterImage = _rec.Image,
                RecruiterName = _rec.Name,
                RecruiterLastName = _rec.LastName,
                StudentImage = _stu.Image,
                StudentName = _stu.Name,
                StudentLastName = _stu.LastName,
            });
        }

        if (list.Count > 0)
        {
            return list;
        }

        return new List<GetSendMessageResponse>();
    }

    public List<GetSendMessageResponse> GetDeliveredMessageStudentById(int studentId)
    {
        var deliveredMessages = _context.DeliveredMessages.Where(s => s.Student.Id == studentId).ToList();
        var list = new List<GetSendMessageResponse>();
        foreach (var deliveredMessage in deliveredMessages)
        {
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            //Console.WriteLine(deliveredMessage.Content);
            var firstOrDefault = _context.SendMessages.Where(s => s.Id == deliveredMessage.Id).FirstOrDefault();
            var _rec = _context.Recruiters.Where(s => s.Id == firstOrDefault.Recruiter.Id).FirstOrDefault();
            var _stu = _context.Students.Where(r => r.Id == studentId).FirstOrDefault();
            Console.WriteLine("MESSAGE: " + deliveredMessage.Content);
            Console.WriteLine("STUDENT: " + _stu.Id + " " + _stu.Name + " " + _stu.LastName + " " + _stu.Email);
            Console.WriteLine("RECRUITER: " + _rec.Id + " " + _rec.Name + " " + _rec.LastName + " " + _rec.Email);
            list.Add(new GetSendMessageResponse()
            {
                Content = deliveredMessage.Content,
                StudentId = studentId,
                RecruiterId = firstOrDefault.Recruiter.Id,
                DeliveredTime = deliveredMessage.DeliveredTime,
                SendTime = null,
                RecruiterImage = _rec.Image,
                RecruiterName = _rec.Name,
                RecruiterLastName = _rec.LastName,
                StudentImage = _stu.Image,
                StudentName = _stu.Name,
                StudentLastName = _stu.LastName,
            });
        }

        if (list.Count > 0)
        {
            return list;
        }

        return new List<GetSendMessageResponse>();
    }

    public List<GetSendMessageResponse> GetSendRecruiterMessageById(int recruiterId)
    {
        var sendMessages = _context.SendMessages.Where(s => s.Recruiter.Id == recruiterId).ToList();
        var list = new List<GetSendMessageResponse>();
        foreach (var sendMessage in sendMessages)
        {
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            Console.WriteLine(sendMessage.Content);
            var firstOrDefault = _context.DeliveredMessages.Where(s => s.Id == sendMessage.Id).FirstOrDefault();
            var _stu = _context.Students.Where(s => s.Id == firstOrDefault.Student.Id).FirstOrDefault();
            var _rec = _context.Recruiters.Where(r => r.Id == recruiterId).FirstOrDefault();
            list.Add(new GetSendMessageResponse()
            {
                Content = sendMessage.Content,
                StudentId = firstOrDefault.Student.Id,
                RecruiterId = recruiterId,
                SendTime = sendMessage.SendTime,
                DeliveredTime = null,
                RecruiterImage = _rec.Image,
                RecruiterName = _rec.Name,
                RecruiterLastName = _rec.LastName,
                StudentImage = _stu.Image,
                StudentName = _stu.Name,
                StudentLastName = _stu.LastName,
            });
        }

        if (list.Count > 0)
        {
            return list;
        }

        return new List<GetSendMessageResponse>();
    }

    public List<GetSendMessageResponse> GetDeliveredRecruiterMessageById(int recruiterId)
    {
        var sendMessages = _context.DeliveredMessages.Where(s => s.Recruiter.Id == recruiterId).ToList();
        var list = new List<GetSendMessageResponse>();
        foreach (var sendMessage in sendMessages)
        {
            var recruiters = _context.Recruiters.ToList();
            var students = _context.Students.ToList();
            Console.WriteLine(sendMessage.Content);
            var firstOrDefault = _context.SendMessages.Where(s => s.Id == sendMessage.Id).FirstOrDefault();
            var _stu = _context.Students.Where(s => s.Id == firstOrDefault.Student.Id).FirstOrDefault();
            var _rec = _context.Recruiters.Where(r => r.Id == recruiterId).FirstOrDefault();
            list.Add(new GetSendMessageResponse()
            {
                Content = sendMessage.Content,
                StudentId = firstOrDefault.Student.Id,
                RecruiterId = recruiterId,
                DeliveredTime = sendMessage.DeliveredTime,
                SendTime = null,
                RecruiterImage = _rec.Image,
                RecruiterName = _rec.Name,
                RecruiterLastName = _rec.LastName,
                StudentImage = _stu.Image,
                StudentName = _stu.Name,
                StudentLastName = _stu.LastName,
            });
        }

        if (list.Count > 0)
        {
            return list;
        }

        return new List<GetSendMessageResponse>();
    }
}