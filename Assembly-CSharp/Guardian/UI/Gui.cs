namespace Guardian.UI
{
    abstract class Gui
    {
        public abstract void Draw();

        public virtual void OnOpen() { }

        public virtual void OnClose() { }
    }
}
