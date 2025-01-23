public abstract class Observer<T> : ObserverBase where T : Subject
{
    protected T mySubject = default;

    public void Attach()
    {
        mySubject = FindFirstObjectByType<T>();
        mySubject.AttachObserver(this);

        mySubject.OnNotify += NotifyHandle;
    }

    public void Detach()
    {
        mySubject.OnNotify -= NotifyHandle;

        mySubject.DetachObserver(this);
    }
}
