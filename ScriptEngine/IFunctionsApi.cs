namespace ScriptEngine
{
    // TODO: maybe add a hierarchy.
    public interface IFunctionsApi // API should only send events.
    {
        // timer functions.
        int StartTimeout(string functionName, int delay); // A string since we could set it in the UI as well, but a delegate would be better in the end. maybe support both.
        void StopTimeout();

        // navigation
        void GoToNextPage();
        void GoToPreviousPage();
        void GoToLastPage();
        void GoToFirstPage();
        void GoToPage(int id);
        void GoToPage(string name);

        // debug
        void Write(string msg);
        void WriteLine(string msg);

        // system
        void Exit();
    }
}
