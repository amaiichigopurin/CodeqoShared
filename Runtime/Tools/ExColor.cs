using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// A collection of colors that are not defined in UnityEngine.Color
    /// </summary>
    public struct ExColor
    {
        private static readonly Dictionary<string, Color> _colorCache = new Dictionary<string, Color>();
        private static Color GetColor(string colorName, Func<Color> createColorFunc)
        {
            if (_colorCache.TryGetValue(colorName, out Color cachedColor))
            {
                return cachedColor;
            }

            var newColor = createColorFunc();
            _colorCache[colorName] = newColor;
            return newColor;
        }

        // Define colors here, don't include colors that are already defined in UnityEngine.Color
        public static Color orange => GetColor(nameof(orange), () => new Color(.88f, 0.387f, 0f));
        public static Color amber => GetColor(nameof(amber), () => new Color(1f, 0.75f, 0f));
        public static Color cream => GetColor(nameof(cream), () => new Color(1f, 0.992f, 0.816f));
        public static Color pink => GetColor(nameof(pink), () => new Color(1f, 0.753f, 0.796f));
        public static Color purple => GetColor(nameof(purple), () => new Color(0.502f, 0f, 0.502f));
        public static Color violet => GetColor(nameof(violet), () => new Color(0.933f, 0.51f, 0.933f));
        public static Color indigo => GetColor(nameof(indigo), () => new Color(0.294f, 0f, 0.51f));
        public static Color smoke => GetColor(nameof(smoke), () => new Color(0.961f, 0.961f, 0.961f));
        public static Color ash => GetColor(nameof(ash), () => new Color(0.698f, 0.745f, 0.71f));
        public static Color steel => GetColor(nameof(steel), () => new Color(0.502f, 0.502f, 0.502f));
        public static Color charcoal => GetColor(nameof(charcoal), () => new Color(0.212f, 0.212f, 0.212f));
        public static Color shadow => GetColor(nameof(shadow), () => new Color(0.125f, 0.125f, 0.125f));
        public static Color licorice => GetColor(nameof(licorice), () => new Color(0.102f, 0.102f, 0.102f));
        public static Color azure => GetColor(nameof(azure), () => new Color(0.0f, 0.5f, 1.0f));
        public static Color beige => GetColor(nameof(beige), () => new Color(0.96f, 0.96f, 0.86f));
        public static Color coral => GetColor(nameof(coral), () => new Color(1.0f, 0.5f, 0.31f));
        public static Color fuchsia => GetColor(nameof(fuchsia), () => new Color(1.0f, 0.0f, 1.0f));
        public static Color gold => GetColor(nameof(gold), () => new Color(1.0f, 0.84f, 0.0f));
        public static Color ivory => GetColor(nameof(ivory), () => new Color(1.0f, 1.0f, 0.94f));
        public static Color khaki => GetColor(nameof(khaki), () => new Color(0.94f, 0.9f, 0.55f));
        public static Color lavender => GetColor(nameof(lavender), () => new Color(0.9f, 0.9f, 0.98f));
        public static Color maroon => GetColor(nameof(maroon), () => new Color(0.5f, 0.0f, 0.0f));
        public static Color navy => GetColor(nameof(navy), () => new Color(0.0f, 0.0f, 0.5f));
        public static Color olive => GetColor(nameof(olive), () => new Color(0.5f, 0.5f, 0.0f));
        public static Color periwinkle => GetColor(nameof(periwinkle), () => new Color(0.8f, 0.8f, 1.0f));
        public static Color plum => GetColor(nameof(plum), () => new Color(0.87f, 0.63f, 0.87f));
        public static Color quartz => GetColor(nameof(quartz), () => new Color(0.85f, 0.85f, 0.95f));
        public static Color salmon => GetColor(nameof(salmon), () => new Color(0.98f, 0.5f, 0.45f));
        public static Color tan => GetColor(nameof(tan), () => new Color(0.82f, 0.71f, 0.55f));
        public static Color teal => GetColor(nameof(teal), () => new Color(0.0f, 0.5f, 0.5f));
        public static Color turquoise => GetColor(nameof(turquoise), () => new Color(0.25f, 0.88f, 0.82f));
        public static Color umber => GetColor(nameof(umber), () => new Color(0.39f, 0.32f, 0.28f));
        public static Color wheat => GetColor(nameof(wheat), () => new Color(0.96f, 0.87f, 0.7f));
        public static Color jade => GetColor(nameof(jade), () => new Color(0.0f, 0.66f, 0.42f));
        public static Color ruby => GetColor(nameof(ruby), () => new Color(0.88f, 0.07f, 0.37f));
        public static Color sapphire => GetColor(nameof(sapphire), () => new Color(0.06f, 0.32f, 0.73f));
        public static Color emerald => GetColor(nameof(emerald), () => new Color(0.31f, 0.78f, 0.47f));
        public static Color topaz => GetColor(nameof(topaz), () => new Color(1.0f, 0.78f, 0.49f));
        public static Color garnet => GetColor(nameof(garnet), () => new Color(0.6f, 0.22f, 0.21f));
        public static Color amethyst => GetColor(nameof(amethyst), () => new Color(0.6f, 0.4f, 0.8f));
        public static Color citrine => GetColor(nameof(citrine), () => new Color(0.89f, 0.82f, 0.04f));
        public static Color onyx => GetColor(nameof(onyx), () => new Color(0.06f, 0.06f, 0.06f));
        public static Color pearl => GetColor(nameof(pearl), () => new Color(0.94f, 0.92f, 0.84f));
        public static Color zircon => GetColor(nameof(zircon), () => new Color(0.77f, 0.88f, 0.94f));
        public static Color moonstone => GetColor(nameof(moonstone), () => new Color(0.96f, 0.94f, 0.9f));
        public static Color aquamarine => GetColor(nameof(aquamarine), () => new Color(0.5f, 1.0f, 0.83f));
        public static Color opal => GetColor(nameof(opal), () => new Color(0.9f, 0.88f, 0.84f));
        public static Color malachite => GetColor(nameof(malachite), () => new Color(0.04f, 0.85f, 0.32f));
        public static Color beryl => GetColor(nameof(beryl), () => new Color(0.76f, 1.0f, 0.0f));
        public static Color cobalt => GetColor(nameof(cobalt), () => new Color(0.0f, 0.28f, 0.67f));
        public static Color firebrick => GetColor(nameof(firebrick), () => new Color(0.7f, 0.13f, 0.13f));
        public static Color glaucous => GetColor(nameof(glaucous), () => new Color(0.38f, 0.51f, 0.71f));
    }
}