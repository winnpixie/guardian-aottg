namespace Guardian.Ui
{
    abstract class Gui
    {
        public abstract void Draw();

        public virtual void OnOpen() { }

        public virtual void OnClose() { }
    }
}
