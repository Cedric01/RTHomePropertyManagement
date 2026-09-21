namespace RTHomePropertyManagement.DTOs;

public class AgentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Designation { get; set; }
    public string? ImageUrl { get; set; }
    public string Link { get; set; } = "/agent_details";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Location { get; set; }
}
