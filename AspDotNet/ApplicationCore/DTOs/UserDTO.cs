namespace ApplicationCore.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string EmailPassword { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeCode { get; set; }
        public int CompanyId { get; set; }
        public int DefaultApplicationId { get; set; }
        public int UserTypeId { get; set; }
        public string EmployeeName { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
    }
}
