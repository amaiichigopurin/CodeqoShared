using static CodeqoEditor.VersionInfo;

namespace CodeqoEditor
{
    public class VersionTypePopup : EditorPopupBase<VersionTypePopup, VersionType>
    {
        protected override VersionType DrawContent(VersionType value)
        {
            CUILayout.EnumToolBar(ref value);
            return value;
        }
    }
}