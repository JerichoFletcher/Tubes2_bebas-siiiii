using BebasFirstLib.Algorithms.Impl;
using BebasFirstLib.Structs.Impl;
using BebasFirstLib.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static BebasFirstLib.Structs.Impl.MazeTreasureMap;

namespace BebasFirstVisualize {
    public class Visualizer : MonoBehaviour {
        public static Visualizer Instance { get; private set; }

        [SerializeField]
        GameObject walkableObject, obstacleObject, treasureObject, krustyKrabsObject;
        [SerializeField]
        GameObject cornerA, cornerB;
        [SerializeField, Range(0f, 1f)]
        float tileScale = 1f;

        [SerializeField]
        Gradient tileGradient, pathGradient;
        [SerializeField]
        Color walkableColor, goalColor;
        [SerializeField, Range(1, 100)]
        int maxVisitColor;
        [SerializeField, Range(0f, 10f)]
        float fadeDuration, pathScrollDelay;

        Dictionary<MazeTreasureMap.MapTile, TileInfo> tiles;
        Dictionary<MazeTreasureMap.MapTile, GameObject> displayObjects;
        bool visualizing = false;
        Rect maxBounds, mapBounds;

        Tree<MazeTreasureMap.MapTile>[] foundPath;

        public bool HasPath { get => foundPath != null; }

        private void Start() {
            if(Instance != null) Destroy(this);
            Instance = this;

            tiles = new Dictionary<MazeTreasureMap.MapTile, TileInfo>();
            displayObjects = new Dictionary<Map<MazeTileType>.MapTile, GameObject>();
            Vector2
                v1 = cornerA.transform.position,
                v2 = cornerB.transform.position;
            maxBounds = new Rect(v1, v2 - v1);
            foundPath = null;
        }

        public void Visualize(MazeTreasureMap maze) {
            foundPath = null;
            foreach(var t in tiles)
                t.Value.Destroy();
            foreach(var t in displayObjects)
                Destroy(t.Value);
            tiles.Clear();
            displayObjects.Clear();

            var mazeSize = new Vector2Int(maze.Size[0], maze.Size[1]);
            
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
                    var mapPos = Helper.VectorFrom(i, j);
                    var type = maze[mapPos].Value;
                    var tileObj = walkableObject;
                    if(type == MazeTileType.Obstacle)
                        tileObj = obstacleObject;

                    var worldPos = new Vector3(mapBounds.xMin + (j + .5f) * tileSize, mapBounds.yMax - (i + .5f) * tileSize, 0f);
                    var tile = Instantiate(tileObj, worldPos, Quaternion.identity);
                    tile.transform.localScale = (type == MazeTileType.Obstacle ? 1f : tileScale) * tileSize * Vector3.one;

                    var tInfo = new TileInfo(tile, 0);
                    if(type != MazeTileType.Obstacle) {
                        Color col = walkableColor;
                        //switch(maze[mapPos].Value.Char) {
                        //    case 'X':
                        //        col = obstacleColor;
                        //        break;
                        //    case 'T':
                        //        col = treasureColor;
                        //        break;
                        //    case 'K':
                        //        col = krustyColor;
                        //        break;
                        //    default: break;
                        //}
                        tInfo.Renderer.color = col;
                    }
                    tiles.Add(maze[mapPos], tInfo);

                    if(type == MazeTileType.Treasure || type == MazeTileType.KrustyKrabs) {
                        var display = Instantiate(type == MazeTileType.Treasure ? treasureObject : krustyKrabsObject, worldPos, Quaternion.identity);
                        display.transform.localScale = tileSize * Vector3.one;
                        displayObjects.Add(maze[mapPos], display);
                    }
                }
            }

            visualizing = true;
        }

        public void Accept(MazeTreasureSearchStep step) {
            foundPath = null;
            if(step.Visited != null) {
                tiles[step.Visited].TimesVisited++;
            }
            if(step.Found) {
                foundPath = step.Value;
                StartCoroutine(tiles[step.Value[^1].Value].UpdateColor(goalColor));
                StartCoroutine(ScrollPathColor());
            }
        }

        public Color SampleFor(int timesVisited) {
            return tileGradient.Evaluate((float)Mathf.Min(timesVisited, maxVisitColor) / maxVisitColor);
        }

        public void ResetDisplay() {
            foreach(var t in tiles) {
                if(t.Key.Value == MazeTileType.Obstacle) continue;
                t.Value.TimesVisited = 0;
                Color col = walkableColor;
                StartCoroutine(t.Value.UpdateColor(col));
            }
            //foreach(var t in displayObjects)
            //    if(t.Value.TryGetComponent<SpriteRenderer>(out var render)) render.enabled = true;
        }

        IEnumerator ScrollPathColor() {
            if(foundPath == null) yield break;
            for(int i = 0; i < foundPath.Length; i++) {
                var t = tiles[foundPath[i].Value];
                t.Emphasis();
                StartCoroutine(t.UpdateColor(pathGradient.Evaluate((float)i / (foundPath.Length - 1))));
                yield return new WaitForSeconds(pathScrollDelay);
            }
        }

        public void OnShowPath() {
            StartCoroutine(ScrollPathColor());
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

            public SpriteRenderer Renderer { get => _renderer != null ? _renderer : (_renderer = Tile.GetComponentInChildren<SpriteRenderer>()); }
            public Animation Anim { get => _anim != null ? _anim : (_anim = Tile.GetComponent<Animation>()); }

            int _timesVisited;
            SpriteRenderer _renderer;
            Animation _anim;

            public TileInfo(GameObject tile, int timesVisited) {
                Tile = tile;
                _timesVisited = timesVisited;
                _renderer = null;
            }

            public void Destroy() {
                Object.Destroy(Tile);
            }

            public IEnumerator UpdateColor(Color newCol) {
                Color d = Renderer.color;
                float t = Time.time;
                while(Time.time < t + Instance.fadeDuration) {
                    Color dc = Color.Lerp(d, newCol, (Time.time - t) / Instance.fadeDuration);
                    Renderer.color = dc;
                    yield return new WaitForEndOfFrame();
                }
            }

            public void Emphasis() {
                Anim.Play(PlayMode.StopAll);
            }
        }
    }
}
