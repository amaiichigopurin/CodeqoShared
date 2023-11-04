using UnityEditor;

namespace CodeqoEditor.Git
{
    public class SharedAssetGitWindow : GitWindow
    {
        protected override string GIT_URL => "https://github.com/amaiichigopurin/CodeqoShared.git";
        protected override string WORKING_DIR => CUIUtility.GetCodeqoPath() + "/Shared";

        [MenuItem("Window/Codeqo/Shared Asset Git Window")]
        public static void ShowWindow()
        {
            GetWindow<SharedAssetGitWindow>("SharedAsset Git Window");
        }
    }
}
