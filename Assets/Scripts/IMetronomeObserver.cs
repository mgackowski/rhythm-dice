public interface IMetronomeObserver
{
    void PreNotify(MetronomeTick tick);
    void Notify(MetronomeTick tick);

}
