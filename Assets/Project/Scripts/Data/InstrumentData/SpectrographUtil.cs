

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {
    [Flags]
    public enum StarElements {
        Hydrogen = 0x1,
        Helium = 0x2,
        Carbon = 0x4,
        Iron = 0x8,
        Calcium = 0x10,
        Sodium = 0x20,
        Magnesium = 0x40,
        Oxygen = 0x80,
    }

    public static class SpectrographUtil {
        private static readonly float MIN_WAVELENGTH = 300;
        private static readonly float MAX_WAVELENGTH = 900;

        public static readonly Dictionary<StarElements, int[]> Wavelengths = new Dictionary<StarElements, int[]>() {
            [StarElements.Hydrogen  ] = new int[] {656, 486, 434, 410},
            [StarElements.Helium    ] = new int[] {588},
            [StarElements.Carbon    ] = new int[] { },
            [StarElements.Iron      ] = new int[] {517, 496, 467, 438, 431, 382, 358, 302},
            [StarElements.Calcium   ] = new int[] {397, 393},
            [StarElements.Sodium    ] = new int[] {590, 589},
            [StarElements.Magnesium ] = new int[] {517, 516},
            [StarElements.Oxygen    ] = new int[] {890, 823, 759, 687, 628}
        };


        public static int[] GetWavelengths(StarElements elements) {
            List<int> result = new List<int>();
            if (elements.HasFlag(StarElements.Hydrogen)) {
                result.AddRange(Wavelengths[StarElements.Hydrogen]);
            }
            if (elements.HasFlag(StarElements.Helium)) {
                result.AddRange(Wavelengths[StarElements.Helium]);
            }
            if (elements.HasFlag(StarElements.Carbon)) {
                result.AddRange(Wavelengths[StarElements.Carbon]);
            }
            if (elements.HasFlag(StarElements.Iron)) {
                result.AddRange(Wavelengths[StarElements.Iron]);
            }
            if (elements.HasFlag(StarElements.Calcium)) {
                result.AddRange(Wavelengths[StarElements.Calcium]);
            }
            if (elements.HasFlag(StarElements.Sodium)) {
                result.AddRange(Wavelengths[StarElements.Sodium]);
            }
            if (elements.HasFlag(StarElements.Magnesium)) {
                result.AddRange(Wavelengths[StarElements.Magnesium]);
            }
            if (elements.HasFlag(StarElements.Oxygen)) {
                result.AddRange(Wavelengths[StarElements.Oxygen]);
            }
            return result.ToArray();
        }

        public static float[] GetWavelengthsNormalized(StarElements spectrum) {
            return NormalizeWavelengths(GetWavelengths(spectrum));
        }

        public static float NormalizeWavelength(int nanometers) {
            return Mathf.InverseLerp(MIN_WAVELENGTH, MAX_WAVELENGTH, nanometers);
        }
        public static float[] NormalizeWavelengths(int[] nanometers) {
            float[] output = new float[nanometers.Length];
            for (int i = 0; i <  nanometers.Length; i++) {
                output[i] = Mathf.InverseLerp(MIN_WAVELENGTH, MAX_WAVELENGTH, nanometers[i]);
            }
            return output;
        }
    }
}