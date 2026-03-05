namespace AuthService.DTOs.AppDtos.RecipeInstructionDto
{
    public class CreateRecipeInstructionDto
    {
        public int StepNumber { get; set; }
        public string ActualInstruction { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; }
    }
}
