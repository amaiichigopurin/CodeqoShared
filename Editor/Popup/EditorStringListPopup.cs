using Microsoft.IdentityModel.Tokens;

namespace CodeqoEditor
{
    public class EditorStringListPopup : EditorPopupBase<EditorStringListPopup, string>
    {
        /// <summary>
        /// 스트링을 복수로 보내서 1개를 선택하게 하는 팝업
        /// </summary>
        protected override string DrawContent(string value)
        {
            if (_valueList.IsNullOrEmpty()) return value;
            int index = _valueList.IndexOf(value);
            index = CUILayout.ListToolbarField(index, _valueList);
            if (index < 0) return value;
            if (index >= _valueList.Count) return value;
            return _valueList[index];
        }
    }
}