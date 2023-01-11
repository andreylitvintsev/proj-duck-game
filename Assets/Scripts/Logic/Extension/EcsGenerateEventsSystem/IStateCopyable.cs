namespace Logic.Extension.EcsGenerateEventsSystem
{
    public interface IStateCopyable<T> where T : struct, IStateCopyable<T>
    {
        public void CopyState(T anotherObject);
    }
}