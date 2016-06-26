namespace gkh
{
    public interface IParentMenu
    {
        void ShowChildMenu(AbstractChildMenu menu);
        void HideChildMenu(AbstractChildMenu menu);
    }
}