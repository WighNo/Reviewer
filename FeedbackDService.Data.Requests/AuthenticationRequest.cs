namespace FeedbackDService.Requests;

public record AuthenticationRequest(string Login, string Password);

public record RegistrationWithRoleRequest(string Login, string Password, string Role);