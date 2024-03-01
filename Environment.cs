namespace Zarabum.interpreter;

class Environment
{
    private Dictionary<string, object> values = new Dictionary<string, object>();

    public void Define(string name, object value)
    {
        values.Add(name, value);
    }

    public object Get(string name)
    {
        if (values.ContainsKey(name))
        {
            return values[name];
        }
        else
        {
            throw new Exception($"undefined variable <{name}>");
        }
    }
}
