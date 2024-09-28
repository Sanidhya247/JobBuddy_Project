namespace job_buddy_backend.DTO
{
        public class ForgotPasswordDto
        {
            public string Email { get; set; }

            public ForgotPasswordDto()
            {
                // Default constructor 
            }

            public ForgotPasswordDto(string email)
            {
                Email = email;
            }
        }
    
}
