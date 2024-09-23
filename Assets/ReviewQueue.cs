using BeauUtil.Debugger;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab {
    public class ReviewQueue : MonoBehaviour {
        public List<ReviewItem> Items = new List<ReviewItem>();

        [SerializeField] private ReviewItem ItemPrefab;
        public void AddNewItem(string guess, CelestialObject refObject, float time, int pts) {
            // TODO: check if item is already in queue
            var NewItem = Instantiate(ItemPrefab, transform);
            NewItem.Populate(this, guess, refObject, time, pts);
            Items.Add(NewItem);
        }       
    }
}
