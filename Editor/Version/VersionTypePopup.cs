

using CodeqoEditor.Git;

namespace CodeqoEditor
{
    public class VersionTypePopup : EditorPopupBase<VersionTypePopup, VersionIncrement>
    {
        protected override VersionIncrement DrawContent(VersionIncrement value)
        {
            CUILayout.EnumToolBar(ref value);
            return value;
        }
    }
}