namespace job_buddy_backend.DTO
{
    public class JobListingDto
    {

        public int JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string ShortJobDescription { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string SalaryRange { get; set; } = string.Empty;
        public decimal? PayRatePerYear { get; set; }
        public decimal? PayRatePerHour { get; set; }
        public string? JobType { get; set; }
        public string? WorkType { get; set; }
        public int EmployerID { get; set; }
        public string EmployerName { get; set; } = string.Empty;
        public string EmployerEmail { get; set; } = string.Empty;
        public string EmployerPhone { get; set; } = string.Empty;
    }
}

