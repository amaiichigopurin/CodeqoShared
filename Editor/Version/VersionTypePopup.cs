

using CodeqoEditor.Git;

namespace CodeqoEditor
{
    public class VersionTypePopup : EditorPopupBase<VersionTypePopup, GitVersion>
    {
        protected override GitVersion DrawContent(GitVersion value)
        {
            CUILayout.EnumToolBar(ref value);
            return value;
        }
    }
}