namespace AuthService.DTOs.AppDtos.Instruction
{
    public class InstructionDto
    {
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; }
    }
}
