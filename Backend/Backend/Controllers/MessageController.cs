using Backend.DataTransferObject.Message;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/[controller]")]
public class MessageController : Controller
{
    private readonly IMessageRepository _messageRepository;

    public MessageController(IMessageRepository messageRepository)
    {
        this._messageRepository = messageRepository;
    }
    
    [HttpGet("send")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetSendMessageResponse>))]
    public IActionResult GetSendMessages()
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetSendMessageResponse> sendMessages = _messageRepository.GetSendMessages();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(sendMessages);
    }
    [HttpGet("delivered")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetDeliveredMessageResponse>))]
    public IActionResult GetDeliveredMessages()
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetDeliveredMessageResponse> deliveredMessages = _messageRepository.GetDeliveredMessages();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(deliveredMessages);
    }

    [HttpGet("send/student/{studentId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetSendMessageResponse>))]
    public IActionResult GetSendMessageStudentById(int studentId)
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetSendMessageResponse> sendMessageById = _messageRepository.GetSendMessageStudentById(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(sendMessageById);
    }
    [HttpGet("delivered/student/{studentId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetSendMessageResponse>))]
    public IActionResult GetDeliveredStudentMessageById(int studentId)
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetSendMessageResponse> sendMessageById = _messageRepository.GetDeliveredMessageStudentById(studentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(sendMessageById);
    }

    [HttpGet("send/recruiter/{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(GetDeliveredMessageResponse))]
    public IActionResult GetSendRecruiterMessageById(int recruiterId)
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetSendMessageResponse> deliveredMessageById = _messageRepository.GetSendRecruiterMessageById(recruiterId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(deliveredMessageById);
    }
    
    [HttpGet("delivered/recruiter/{recruiterId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GetSendMessageResponse>))]
    public IActionResult GetDeliveredRecruiterMessageById(int recruiterId)
    {
        //var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());
        IEnumerable<GetSendMessageResponse> deliveredMessageById = _messageRepository.GetDeliveredRecruiterMessageById(recruiterId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(deliveredMessageById);
    }
    [HttpPost("recruiter-student")]
    [ProducesResponseType(201, Type = typeof(FromRecruiterToStudentResponse))]
    public IActionResult SendMessageFromRecruiterToStudent([FromBody] MessagePostRequest messagePostRequest)
    {
        FromRecruiterToStudentResponse message = _messageRepository.CreateMessageRecruiterToStudent(messagePostRequest);
        return Created("HttpStatusCode.Created",message);
    }
    [HttpPost("student-recruiter")]
    [ProducesResponseType(201, Type = typeof(FromStudentToRecruiterResponse))]
    public IActionResult SendMessageFromStudentToRecruiter([FromBody] MessagePostRequest messagePostRequest)
    {
        FromStudentToRecruiterResponse message = _messageRepository.CreateMessageStudentToRecruiter(messagePostRequest);
        return Created("HttpStatusCode.Created",message);
    }



}