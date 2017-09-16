public class Constants 
{
	// This clients game version - used for netword security aswell for ensuring clients stay on same version as server
	public static string GameVersion = "1.0.0";

	// Address of login servers
	public static string TestLoginServerURL = "127.0.0.1/login.php";
	public static string LoginServerURL = "https://thedivergentnetwork.000webhostapp.com/login.php";

	// Server will send one of these responses back from login operation
	public static string AuthenticationSuccess = "DsnLoginOpSuccess";
	public static string AuthenticationFailed = "DsnLoginOpFailed";

	public static string InvalidCredentials = "Invalid credentials :(";
	public static string ServerError = "Unexpected server error :(";
}
