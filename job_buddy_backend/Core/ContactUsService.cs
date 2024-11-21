using System.Threading.Tasks;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;

namespace job_buddy_backend.Core
{
    public class ContactUsService
    {
        private readonly JobBuddyDbContext _context;
        //private readonly EmailService _emailService; 

        public ContactUsService(JobBuddyDbContext context/*, EmailService emailService*/)
        {
            _context = context;
            //_emailService = emailService;
        }

        public async Task<bool> SubmitContactRequest(ContactUsDto contactUsDto)
        {
           
            var contactRequest = new ContactUs
            {
                Name = contactUsDto.Name,
                Email = contactUsDto.Email,
                Subject = contactUsDto.Subject,
                Message = contactUsDto.Message
            };
            await _context.ContactUsRequests.AddAsync(contactRequest);
            await _context.SaveChangesAsync();

            // Send email notification
            var emailContent = $"New Contact Us Request:\n\nName: {contactUsDto.Name}\nEmail: {contactUsDto.Email}\nSubject: {contactUsDto.Subject}\nMessage: {contactUsDto.Message}";
            //Commenting this because we are not using email service due to secret key issues
            //await _emailService.SendEmailAsync("admin@gmail.com", "New Contact Us Request", emailContent);

            return true;
        }
    }
}
