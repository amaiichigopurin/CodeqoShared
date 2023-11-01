using UnityEngine;
using System;
using UnityEditor;
using Glitch9.Routina.AI;
using Glitch9.AI.Tools;

namespace CodeqoEditor
{
    public partial class CUILayout
    {
        static string waitingLabel = "Waiting...";
        static bool isWaiting = false;


        public static void CLOVAButtonFromKorean(string label, string from, Action<string> to, params GUILayoutOption[] options)
        {
            if (isWaiting)
            {
                BoxedLabel(new GUIContent(label), GUILayout.MaxWidth(100f));
            }
            else
            {
                if (GUILayout.Button(label, options))
                {
                    if (from == null || to == null || isWaiting) return;
                    isWaiting = true;
                    CLOVATranslator.TranslateToEnglish(from, (en) => {
                        CUITextWindow.Create("네이버 클로바 번역", en, to);
                        isWaiting = false;
                    });
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                isWaiting = false;
            }
        }

        public static void CLOVAButtonFromEnglish(string label, string from, Action<string> to, params GUILayoutOption[] options)
        {
            if (isWaiting)
            {
                BoxedLabel(new GUIContent(label), GUILayout.MaxWidth(100f));
            }
            else
            {
                if (GUILayout.Button(label, options))
                {
                    if (from == null || to == null || isWaiting) return;
                    isWaiting = true;
                    CLOVATranslator.TranslateToKorean(from, (ko) =>
                    {
                        CUITextWindow.Create("네이버 클로바 번역", ko, to);
                        isWaiting = false;
                    });
                }
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                isWaiting = false;
            }
        }        

        public static void OpenAiButtonFromKorean(string label, string from, Action<string> to, params GUILayoutOption[] options)
        {
            if (isWaiting)
            {
                BoxedLabel(new GUIContent("Translating..."), GUILayout.MaxWidth(100f));
            }
            else
            {
                if (GUILayout.Button(label, options))
                {
                    if (from == null || to == null || isWaiting) return;
                    isWaiting = true;
                    OpenAiTranslator.TranslateToEnglish(from, (en) =>
                    {
                        CUITextWindow.Create("OpenAI GPT4", en, to);
                        isWaiting = false;
                    });
                }
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                isWaiting = false;
            }
        }

        public static void OpenAiButtonFromEnglish(string label, string from, Action<string> to, params GUILayoutOption[] options)
        {
            if (isWaiting)
            {
                BoxedLabel(new GUIContent("Translating..."), GUILayout.MaxWidth(100f));
            }
            else
            {
                if (GUILayout.Button(label, options))
                {
                    if (from == null || to == null || isWaiting) return;
                    isWaiting = true;
                    OpenAiTranslator.TranslateToKorean(from, (ko) =>
                    {
                        CUITextWindow.Create("OpenAI GPT4", ko, to);
                        isWaiting = false;
                    });
                }
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                isWaiting = false;
            }
        }


        public static string EnglishDescriptionField(string label, string text, string from, Action<string> to, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.GetStyle("TextArea");
            HorizontalLayout(() => {
                EditorGUILayout.LabelField($"{label} (English)", GUILayout.MaxWidth(2000f));
                GUILayout.FlexibleSpace();
                OpenAiButtonFromKorean("Translate", from, (string result) =>
                {
                    to(result);
                }, GUILayout.Width(100));
            });
            return EditorGUILayout.TextField(text, style, options);
        }

        public static string KoreanDescriptionField(string label, string text, string from, Action<string> to, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.GetStyle("TextArea");
            HorizontalLayout(() =>
            {
                EditorGUILayout.LabelField($"{label} (Korean)", GUILayout.MaxWidth(2000f));
                GUILayout.FlexibleSpace();
                OpenAiButtonFromEnglish("Translate", from, (string result) =>
                {
                    to(result);
                }, GUILayout.Width(100));
            });
            return EditorGUILayout.TextField(text, style, options);
        }

        public static string DescriptionField(string text, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.GetStyle("TextArea");
            return EditorGUILayout.TextArea(text, style, options);
        }

        public static string DescriptionField(SerializedProperty property, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.GetStyle("TextArea");
            return EditorGUILayout.TextArea(property.stringValue, style, options);
        }

        public static string DescriptionField(string label, SerializedProperty property, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.GetStyle("TextArea");
            EditorGUILayout.LabelField(label);
            return EditorGUILayout.TextArea(property.stringValue, style, options);
        }

        public static string EnglishDescriptionField(string text, string from, Action<string> to, params GUILayoutOption[] options) => EnglishDescriptionField("Description", text, from, to, options);
        public static string KoreanDescriptionField(string text, string from, Action<string> to, params GUILayoutOption[] options) => KoreanDescriptionField("Description", text, from, to, options);


        

    }
}

