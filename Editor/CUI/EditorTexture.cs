using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public class EditorTexture
    {
        private const string ASSET_PATH = "Shared/Gizmos/Textures/";
        private const string DIR_ANDROID12 = "Android12/";
        private const string DIR_BACKGROUND = "Background/";
        private const string DIR_BORDER = "Border/";
        private const string DIR_BOX = "Box/";
        private const string DIR_CONFIG = "Config/";
        private const string DIR_SLIDER = "Slider/";
        private const string DIR_MEDIA = "Media/";
        private const string DIR_TABBEDPANEL = "TabbedPanel/";
        private const string DIR_TEXTFIELD = "TextField/";
        private const string DIR_TOOLBAR = "ToolBar/";
        private const string DIR_EXTRA = "Extra/";

        private static string GetPath(string assetName) => CUIUtility.GetCodeqoPath() + "/" + ASSET_PATH + assetName;

        private static Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        private static Dictionary<string, EditorTexture2D> _editorTextureCache = new Dictionary<string, EditorTexture2D>();

        private static Texture2D GetCachedTexture(string directory, string assetName)
        {
            string key = directory + "_" + assetName;
            if (!_textureCache.TryGetValue(key, out var texture))
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetPath(directory + assetName));
                _textureCache.Add(key, texture);
            }
            return texture;
        }

        private static EditorTexture2D GetCachedEditorTexture(string directory, string assetName, string assetNameDark)
        {
            string key = directory + "_" + assetName;
            if (!_editorTextureCache.TryGetValue(key, out var editorTexture))
            {
                editorTexture = new EditorTexture2D(
                    AssetDatabase.LoadAssetAtPath<Texture2D>(GetPath(directory + assetName)),
                    AssetDatabase.LoadAssetAtPath<Texture2D>(GetPath(directory + assetNameDark))
                );
                _editorTextureCache.Add(key, editorTexture);
            }
            return editorTexture;
        }
        
        private static EditorTexture2D boxDefault => GetCachedEditorTexture(DIR_BOX, "section-box-light.psd", "section-box-dark.psd");
        private static Texture2D boxGreen => GetCachedTexture(DIR_BOX, "section-box-green.psd");
        private static Texture2D boxYellow => GetCachedTexture(DIR_BOX, "section-box-yellow.psd");
        private static Texture2D boxOrange => GetCachedTexture(DIR_BOX, "section-box-orange.psd");
        private static Texture2D boxPurple => GetCachedTexture(DIR_BOX, "section-box-purple.psd");
        private static Texture2D boxBlue => GetCachedTexture(DIR_BOX, "section-box-blue.psd");

        private static EditorTexture2D textFieldDefault => GetCachedEditorTexture(DIR_TEXTFIELD, "textfield-light.png", "textfield-dark.png");
        private static Texture2D textFieldGreen => GetCachedTexture(DIR_TEXTFIELD, "textfield-green.png");
        private static Texture2D textFieldYellow => GetCachedTexture(DIR_TEXTFIELD, "textfield-yellow.png");
        private static Texture2D textFieldOrange => GetCachedTexture(DIR_TEXTFIELD, "textfield-orange.png");
        private static Texture2D textFieldPurple => GetCachedTexture(DIR_TEXTFIELD, "textfield-purple.png");
        private static Texture2D textFieldBlue => GetCachedTexture(DIR_TEXTFIELD, "textfield-blue.png");

        private static EditorTexture2D borderTop => GetCachedEditorTexture(DIR_BORDER, "section-border-top-light.psd", "section-border-top-dark.psd");
        private static EditorTexture2D borderBottom => GetCachedEditorTexture(DIR_BORDER, "section-border-bottom-light.psd", "section-border-bottom-dark.psd");

        private static Texture2D toolBarButtonOn => GetCachedTexture(DIR_TOOLBAR, "btn-mid-on.psd");
        private static Texture2D toolBarButtonOff => GetCachedTexture(DIR_TOOLBAR, "btn-mid-off.psd");

        private static Texture2D background => GetCachedTexture(DIR_BACKGROUND, "section-background.psd");
        private static Texture2D noImageHighRes => GetCachedTexture(DIR_EXTRA, "no-image-high-res.png");

        private static Texture2D play => GetCachedTexture(DIR_MEDIA, "ic_play_arrow_black_36dp.png");
        private static Texture2D pause => GetCachedTexture(DIR_MEDIA, "ic_pause_black_36dp.png");
        private static Texture2D stop => GetCachedTexture(DIR_MEDIA, "ic_stop_black_36dp.png");
        private static Texture2D next => GetCachedTexture(DIR_MEDIA, "ic_skip_next_black_36dp.png");
        private static Texture2D previous => GetCachedTexture(DIR_MEDIA, "ic_skip_previous_black_36dp.png");
        private static Texture2D fastForward => GetCachedTexture(DIR_MEDIA, "ic_fast_forward_black_36dp.png");
        private static Texture2D rewind => GetCachedTexture(DIR_MEDIA, "ic_fast_rewind_black_36dp.png");
        private static Texture2D volume => GetCachedTexture(DIR_MEDIA, "ic_volumebar.png");
        private static Texture2D seekBar => GetCachedTexture(DIR_MEDIA, "ic_seekbar.png");
        private static Texture2D shuffle => GetCachedTexture(DIR_MEDIA, "baseline_shuffle_black_36.png");
        private static Texture2D repeat => GetCachedTexture(DIR_MEDIA, "baseline_repeat_black_36.png");
        private static Texture2D repeatOne => GetCachedTexture(DIR_MEDIA, "baseline_repeat_one_black_36.png");

        
        private static Texture2D toggleDescriptionOn => GetCachedTexture(DIR_CONFIG, "toggle_description_on.psd");
        private static Texture2D toggleDescriptionOff => GetCachedTexture(DIR_CONFIG, "toggle_description_off.psd");
        private static Texture2D toggleIconOn => GetCachedTexture(DIR_CONFIG, "toggle_icon_on.psd");
        private static Texture2D toggleIconOff => GetCachedTexture(DIR_CONFIG, "toggle_icon_off.psd");
        private static Texture2D toggleLinebreakOn => GetCachedTexture(DIR_CONFIG, "toggle_linebreak_on.psd");
        private static Texture2D toggleLinebreakOff => GetCachedTexture(DIR_CONFIG, "toggle_linebreak_off.psd");
        private static Texture2D toggleNextlineOn => GetCachedTexture(DIR_CONFIG, "toggle_nextline_on.psd");
        private static Texture2D toggleNextlineOff => GetCachedTexture(DIR_CONFIG, "toggle_nextline_off.psd");
        



        public static Texture2D ToolBarButtonOn => toolBarButtonOn;
        public static Texture2D ToolBarButtonOff => toolBarButtonOff;
        public static Texture2D BorderTop => borderTop;
        public static Texture2D BorderBottom => borderBottom;
        public static Texture2D Background => background;
        public static Texture2D NoImageHighRes => noImageHighRes;


        // Media
        public static Texture2D Play => play;
        public static Texture2D Pause => pause;
        public static Texture2D Stop => stop;
        public static Texture2D Next => next;
        public static Texture2D Previous => previous;
        public static Texture2D FastForward => fastForward;
        public static Texture2D Rewind => rewind;
        public static Texture2D Volume => volume;
        public static Texture2D SeekBar => seekBar;
        public static Texture2D Shuffle => shuffle;
        public static Texture2D Repeat => repeat;
        public static Texture2D RepeatOne => repeatOne;

        // Config
        public static Texture2D ToggleDescriptionOn => toggleDescriptionOn;
        public static Texture2D ToggleDescriptionOff => toggleDescriptionOff;
        public static Texture2D ToggleIconOn => toggleIconOn;
        public static Texture2D ToggleIconOff => toggleIconOff;
        public static Texture2D ToggleLinebreakOn => toggleLinebreakOn;
        public static Texture2D ToggleLinebreakOff => toggleLinebreakOff;
        public static Texture2D ToggleNextlineOn => toggleNextlineOn;
        public static Texture2D ToggleNextlineOff => toggleNextlineOff;



        public static Texture2D Box(CUIColor color = 0)
        {     
            return color switch
            {
                CUIColor.Green => boxGreen,
                CUIColor.Yellow => boxYellow,
                CUIColor.Orange => boxOrange,
                CUIColor.Purple => boxPurple,
                CUIColor.Blue => boxBlue,
                _ => (Texture2D)boxDefault,
            };
        }

        public static Texture2D TextField(CUIColor color = 0)
        {
            return color switch
            {
                CUIColor.Green => textFieldGreen,
                CUIColor.Yellow => textFieldYellow,
                CUIColor.Orange => textFieldOrange,
                CUIColor.Purple => textFieldPurple,
                CUIColor.Blue => textFieldBlue,
                _ => textFieldDefault,
            };
        }
    }
}
