using System.Collections.Generic;

/// <summary>
/// 20180620 SeongJun :
/// </summary>
abstract public class Subject
{
    protected List<Observer> Observers = new List<Observer>();

    public virtual void Attach(Observer observer)
    {
        Observers.Add(observer);
    }

    public virtual void Detach(Observer observer)
    {
        Observers.Remove(observer);
    }

    public virtual void Notify()
    {
        foreach (var o in Observers)
        {
            o.Reactive(this);
        }
    }
}
