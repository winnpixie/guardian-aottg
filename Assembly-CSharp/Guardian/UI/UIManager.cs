namespace Guardian.UI
{
    class UIManager
    {
        public UIBase CurrentScreen;

        public void OpenScreen(UIBase screen)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.OnClose();
            }

            CurrentScreen = screen;

            if (CurrentScreen != null)
            {
                CurrentScreen.OnOpen();
            }
        }
    }
}
