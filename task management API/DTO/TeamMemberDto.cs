namespace task_management_API.DTO
{
    public class TeamMemberDto
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<TaskDto> Tasks { get; set; }
    }
}
