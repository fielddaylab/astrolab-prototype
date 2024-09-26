using BeauUtil.Debugger;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {
    public class ReviewQueue : MonoBehaviour {
        public static ReviewQueue Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        public List<ReviewItem> Items = new List<ReviewItem>();

        [SerializeField] private ReviewItem ItemPrefab;
        public void AddNewCelestialItem(string guess, CelestialObject refObject, float time, int pts) {
            // TODO: check if item is already in queue
            var NewItem = Instantiate(ItemPrefab, transform);
            NewItem.PopulateCelestial(this, guess, refObject, time, pts);
            Items.Add(NewItem);
        }

        public void AddNewPuzzleItem(PostcardPuzzle refObject, float time, int pts)
        {
            // TODO: check if item is already in queue
            var NewItem = Instantiate(ItemPrefab, transform);
            NewItem.PopulatePuzzle(this, refObject, time, pts);
            Items.Add(NewItem);
        }
    }
}
