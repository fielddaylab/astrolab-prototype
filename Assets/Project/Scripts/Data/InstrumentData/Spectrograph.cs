using BeauUtil.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroLab {
    public class Spectrograph : Image {
        [SerializeField] private RectGraphic LinePrefab;
        [SerializeField] private List<RectGraphic> Lines;

        public void DisplaySpectrum(StarElements spectrum) {
            Lines.ForEach(s => {
                Destroy(s.gameObject); 
            });
            Lines.Clear();
            float[] pos = SpectrographUtil.GetWavelengthsNormalized(spectrum);
            if (pos.Length <= 0) {
                this.color = Color.black;
                return;
            }
            for (int i = 0; i < pos.Length; i++) {
                this.color = Color.white;
                RectGraphic line = Instantiate(LinePrefab, rectTransform);
                line.rectTransform.localPosition = new Vector3((pos[i] -0.5f) * rectTransform.rect.width, 0f, 0f);
                line.gameObject.SetActive(true);
                Lines.Add(line);
            }
        }

    }
}