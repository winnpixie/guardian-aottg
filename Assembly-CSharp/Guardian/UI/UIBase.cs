namespace Guardian.UI
{
    abstract class UIBase
    {
        public abstract void Draw();

        public virtual void OnOpen() { }

        public virtual void OnClose() { }
    }
}
