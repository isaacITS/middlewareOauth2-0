namespace ITS_Middleware.Helpers.Log
{
    public interface ILoggerManager
    {
        //This method generates an informative message
        //A message as parameter
        void LogInformation(string message);

        //This method generates an informative message
        //A message as parameter and object exception
        void LogInformation(string message, Exception ex);

        //This method generates a warning message
        //A message as parameter
        void LogWarning(string message);

        //This method generates a warning message
        //A message as parameter and object exception
        void LogWarning(string message, Exception ex);

        //This method generates an error message
        //A message as param
        void LogError(string message);

        //This method generates an error message
        //A message as parameter and object exception
        void LogError(string message, Exception ex);
    }
}
