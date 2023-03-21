using BebasFirstLib.Algorithms.Impl;
using BebasFirstLib.Structs.Impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BebasFirstVisualize {
    public class Visualizer : MonoBehaviour {
        public static Visualizer Instance { get; private set; }

        [SerializeField]
        GameObject tileObj;
        [SerializeField]
        GameObject cornerA, cornerB;
        [SerializeField, Range(0f, 1f)]
        float tileScale = 1f;

        [SerializeField]
        Gradient tileGradient, pathGradient;
        [SerializeField]
        Color walkableColor, obstacleColor, treasureColor, krustyColor, goalColor;
        [SerializeField, Range(1, 100)]
        int maxVisitColor;
        [SerializeField, Range(0f, 10f)]
        float fadeDuration, pathScrollDelay;

        Dictionary<MazeTreasureMap.MapTile, TileInfo> tiles;
        bool visualizing = false;
        Rect maxBounds, mapBounds;

        private void Start() {
            if(Instance != null) Destroy(this);
            Instance = this;

            tiles = new Dictionary<MazeTreasureMap.MapTile, TileInfo>();
            Vector2
                v1 = cornerA.transform.position,
                v2 = cornerB.transform.position;
            maxBounds = new Rect(v1, v2 - v1);
        }

        public void Visualize(MazeTreasureMap maze) {
            foreach(var t in tiles)
                t.Value.Destroy();
            tiles.Clear();

            Vector2Int mazeSize = new Vector2Int(maze.Size[0], maze.Size[1]);
            
            float ratio = (float)mazeSize.x / mazeSize.y;
            float width = maxBounds.size.x, height = ratio * width;
            if(height > maxBounds.size.y) {
                height = maxBounds.size.y;
                width = height / ratio;
            }

            mapBounds = new Rect(maxBounds.center.x - width / 2f, maxBounds.center.y - height / 2f, width, height);
            float tileSize = mapBounds.height / mazeSize.x;

            for(int i = 0; i < mazeSize.x; i++) {
                for(int j = 0; j < mazeSize.y; j++) {
                    var pos = Helper.VectorFrom(i, j);
                    var tile = Instantiate(tileObj, new Vector3(mapBounds.xMin + (j + .5f) * tileSize, mapBounds.yMax - (i + .5f) * tileSize, 0f), Quaternion.identity);
                    tile.transform.localScale = Vector3.one * tileScale * tileSize;
                    Color col = walkableColor;
                    switch(maze[pos].Value.Char) {
                        case 'X':
                            col = obstacleColor;
                            break;
                        case 'T':
                            col = treasureColor;
                            break;
                        case 'K':
                            col = krustyColor;
                            break;
                        default: break;
                    }
                    tile.GetComponent<SpriteRenderer>().color = col;
                    tiles.Add(maze[pos], new TileInfo(tile, 0));
                }
            }

            visualizing = true;
        }

        public void Accept(MazeTreasureSearchStep step) {
            if(step.Visited != null) {
                tiles[step.Visited].TimesVisited++;
            }
            if(step.Found) {
                StartCoroutine(tiles[step.Value[step.Value.Length - 1].Value].UpdateColor(goalColor));
                StartCoroutine(ScrollPathColor(step.Value));
            }
        }

        public Color SampleFor(int timesVisited) {
            return tileGradient.Evaluate((float)Mathf.Min(timesVisited, maxVisitColor) / maxVisitColor);
        }

        public void ResetColor() {
            foreach(var t in tiles) {
                t.Value.TimesVisited = 0;
                Color col = walkableColor;
                switch(t.Key.Value.Char) {
                    case 'X':
                        col = obstacleColor;
                        break;
                    case 'T':
                        col = treasureColor;
                        break;
                    case 'K':
                        col = krustyColor;
                        break;
                    default: break;
                }
                StartCoroutine(t.Value.UpdateColor(col));
            }
        }

        IEnumerator ScrollPathColor(BebasFirstLib.Structs.Tree<MazeTreasureMap.MapTile>[] path) {
            for(int i = 0; i < path.Length; i++) {
                var t = path[i];
                StartCoroutine(tiles[t.Value].UpdateColor(pathGradient.Evaluate((float)i / (path.Length - 1))));
                yield return new WaitForSeconds(pathScrollDelay);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(maxBounds.center, maxBounds.size);
            if(visualizing) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(mapBounds.center, mapBounds.size);
            }
        }

        class TileInfo {
            public GameObject Tile { get; private set; }
            public int TimesVisited {
                get {
                    return _timesVisited;
                }
                set {
                    _timesVisited = value;
                    if(value > 0)
                        Instance.StartCoroutine(UpdateColor(Instance.SampleFor(value)));
                }
            }

            int _timesVisited;

            public TileInfo(GameObject tile, int timesVisited) {
                Tile = tile;
                _timesVisited = timesVisited;
            }

            public void Destroy() {
                Object.Destroy(Tile);
            }

            public IEnumerator UpdateColor(Color newCol) {
                var render = Tile.GetComponent<SpriteRenderer>();
                Color d = render.color;
                float t = Time.time;
                while(Time.time < t + Instance.fadeDuration) {
                    Color dc = Color.Lerp(d, newCol, (Time.time - t) / Instance.fadeDuration);
                    Tile.GetComponent<SpriteRenderer>().color = dc;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
