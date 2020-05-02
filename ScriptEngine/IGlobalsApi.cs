namespace ScriptEngine
{
    public interface IGlobalsApi
    {
        object GetVariable(string propertyName);

        void SetVariable(string propertyName, object value);
    }
}
