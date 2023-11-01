using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeqoEditor
{
    public class EditorSound
    {
        static AudioClip previewClip = null;
        
        public static string AddressableAudioClipField(SerializedProperty serializedProperty, string addressableGroup, GUIContent label = null, params GUILayoutOption[] options)
        {
            // Get all Addressable names
            var allAddressables = AddressableEditorUtility.GetAllAddressableNames(addressableGroup);
            if (allAddressables == null || allAddressables.Count == 0)
            {
                Debug.LogWarning($"No addressables found in the group {addressableGroup}");
                return serializedProperty.stringValue;
            }

            // If label is null, we create a default one
            if (label == null)
            {
                label = new GUIContent("Select Addressable AudioClip:");
            }

            // Find the index of the current value
            int currentIndex = allAddressables.IndexOf(serializedProperty.stringValue);
            if (currentIndex < 0)
            {
                // Default to first item if the current serialized property is not in the list
                currentIndex = 0;
                serializedProperty.stringValue = allAddressables[currentIndex];
            }

            EditorGUILayout.BeginHorizontal();

            // Render popup
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUILayout.Popup(label, currentIndex, allAddressables.ToArray(), options);
            if (EditorGUI.EndChangeCheck())
            {
                serializedProperty.stringValue = allAddressables[newIndex];
                LoadAudioClip(allAddressables[newIndex]);
            }

            // Preview button
            if (GUILayout.Button("Preview", GUILayout.Width(60)))
            {
                if (previewClip != null)
                {
                    PlayClip(previewClip);
                }
            }

            EditorGUILayout.EndHorizontal();

            return serializedProperty.stringValue;
        }

        static void LoadAudioClip(string address)
        {
            Addressables.LoadAssetAsync<AudioClip>(address).Completed += OnAudioClipLoaded;
        }

        static void OnAudioClipLoaded(AsyncOperationHandle<AudioClip> op)
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                previewClip = op.Result;
            }
            else
            {
                Debug.LogError($"Failed to load AudioClip with address: {op.DebugName}");
            }
        }

        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
            method.Invoke(
               null,
               new object[] { clip, startSample, loop }
           );
        }

        public static void StopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { },
                null
            );
            method.Invoke(
                null,
                new object[] { }
            );
        }
    }
}
